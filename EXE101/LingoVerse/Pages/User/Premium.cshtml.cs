using LingoVerse.Models;
using LingoVerse.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace LingoVerse.Pages.User
{
    public class PremiumModel : PageModel
    {
        private readonly IGenericService<PremiumPlan> _premiumPlanService;
        private readonly IGenericService<UserPremiumSubscription> _userPremiumSubscriptionService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public PremiumModel(
            IGenericService<PremiumPlan> premiumPlanService,
            IGenericService<UserPremiumSubscription> userPremiumSubscriptionService,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _premiumPlanService = premiumPlanService;
            _userPremiumSubscriptionService = userPremiumSubscriptionService;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _emailService = emailService;
        }

        public IEnumerable<PremiumPlan> premiumPlans { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            premiumPlans = await _premiumPlanService.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetCheckPayment(string paymentCode, int planId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiKey = _configuration["Sepay:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    return new JsonResult(new { success = false, message = "API key không được cấu hình" });
                }

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var url = "https://my.sepay.vn/userapi/transactions/list?account_number=00003480126&limit=20";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new JsonResult(new { success = false, message = $"Không thể kết nối tới Sepay API: {response.StatusCode}" });
                }

                var json = await response.Content.ReadAsStringAsync();
                var sepayResponse = JsonSerializer.Deserialize<SepayTransactionResponse>(json);

                if (sepayResponse?.Status != 200 || sepayResponse?.Messages?.Success != true)
                {
                    return new JsonResult(new { success = false, message = "Phản hồi từ Sepay không thành công" });
                }

                var matchedTransaction = sepayResponse.Transactions?.FirstOrDefault(tx =>
                    tx.TransactionContent?.Trim().ToLower() == paymentCode?.Trim().ToLower());

                if (matchedTransaction != null)
                {
                    var userJson = _httpContextAccessor.HttpContext?.Session.GetString("UserSession");
                    if (string.IsNullOrEmpty(userJson))
                    {
                        return new JsonResult(new { success = false, message = "Người dùng chưa đăng nhập" });
                    }

                    var user = JsonSerializer.Deserialize<Models.User>(userJson);
                    var plans = await _premiumPlanService.GetAllAsync();
                    var plan = plans.FirstOrDefault(p => p.PlanId == planId);
                    if (plan == null)
                    {
                        return new JsonResult(new { success = false, message = "Gói Premium không tồn tại" });
                    }

                    var existingSubscription = (await _userPremiumSubscriptionService.GetAllAsync())
                        .FirstOrDefault(s => s.UserId == user.UserId && s.IsActive && !s.IsDeleted);

                    if (existingSubscription != null)
                    {
                        // Gia hạn subscription hiện tại
                        existingSubscription.EndDate = existingSubscription.EndDate.AddDays(plan.DurationDays);
                        await _userPremiumSubscriptionService.UpdateAsync(existingSubscription);
                        return new JsonResult(new
                        {
                            success = true,
                            message = "Thanh toán thành công và đã gia hạn gói Premium",
                            isRenewal = true,
                            planId = planId
                        });
                    }
                    else
                    {
                        // Tạo subscription mới
                        var subscription = new UserPremiumSubscription
                        {
                            UserId = user.UserId,
                            PlanId = planId,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddDays(plan.DurationDays),
                            IsActive = true,
                            IsDeleted = false
                        };
                        await _userPremiumSubscriptionService.AddAsync(subscription);
                        return new JsonResult(new
                        {
                            success = true,
                            message = "Thanh toán thành công và đã kích hoạt gói Premium",
                            isRenewal = false,
                            planId = planId
                        });
                    }
                }

                return new JsonResult(new { success = false, message = "Không tìm thấy giao dịch khớp" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnGetSendConfirmationEmail(int planId, bool isRenewal)
        {
            try
            {
                var userJson = _httpContextAccessor.HttpContext?.Session.GetString("UserSession");
                if (string.IsNullOrEmpty(userJson))
                {
                    return new JsonResult(new { success = false, message = "Người dùng chưa đăng nhập" });
                }

                var user = JsonSerializer.Deserialize<Models.User>(userJson);
                var plans = await _premiumPlanService.GetAllAsync();
                var plan = plans.FirstOrDefault(p => p.PlanId == planId);
                if (plan == null)
                {
                    return new JsonResult(new { success = false, message = "Gói Premium không tồn tại" });
                }

                var existingSubscription = (await _userPremiumSubscriptionService.GetAllAsync())
                    .FirstOrDefault(s => s.UserId == user.UserId && s.IsActive && !s.IsDeleted);

                if (existingSubscription == null)
                {
                    return new JsonResult(new { success = false, message = "Không tìm thấy subscription" });
                }

                EmailMessage emailMessage;
                if (isRenewal)
                {
                    // Email gia hạn
                    emailMessage = new EmailMessage
                    {
                        To = user.Email,
                        Subject = "Xác Nhận Gia Hạn Gói Premium Thành Công",
                        Body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: #4CAF50; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background: #ffffff; padding: 20px; border: 1px solid #eee; border-radius: 0 0 5px 5px; }}
        .button {{ display: inline-block; padding: 10px 20px; background: #4CAF50; color: white; text-decoration: none; border-radius: 5px; }}
        .footer {{ text-align: center; font-size: 12px; color: #777; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Xin chào {user.Username}!</h2>
        </div>
        <div class='content'>
            <p>Chúng tôi rất vui mừng thông báo rằng gói Premium <strong>'{plan.PlanName}'</strong> của bạn đã được gia hạn thành công.</p>
            <p><strong>Thông tin chi tiết:</strong></p>
            <ul>
                <li>Thời gian kết thúc mới: <strong>{existingSubscription.EndDate:dd/MM/yyyy}</strong></li>
            </ul>
            <p>Cảm ơn bạn đã tiếp tục đồng hành cùng LingoVerse!</p>
            <p><a href='mailto:support@lingoverse.com' class='button'>Liên hệ hỗ trợ</a></p>
        </div>
        <div class='footer'>
            <p>Trân trọng,<br/>Đội ngũ LingoVerse<br/>© 2025 LingoVerse. All rights reserved.</p>
        </div>
    </div>
</body>
</html>",
                        IsHtml = true
                    };
                }
                else
                {
                    // Email đăng ký mới
                    emailMessage = new EmailMessage
                    {
                        To = user.Email,
                        Subject = "Xác Nhận Kích Hoạt Gói Premium Thành Công",
                        Body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: #4CAF50; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background: #ffffff; padding: 20px; border: 1px solid #eee; border-radius: 0 0 5px 5px; }}
        .button {{ display: inline-block; padding: 10px 20px; background: #4CAF50; color: white; text-decoration: none; border-radius: 5px; }}
        .footer {{ text-align: center; font-size: 12px; color: #777; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Xin chào {user.Username}!</h2>
        </div>
        <div class='content'>
            <p>Chúc mừng bạn! Gói Premium <strong>'{plan.PlanName}'</strong> của bạn đã được kích hoạt thành công.</p>
            <p><strong>Thông tin chi tiết:</strong></p>
            <ul>
                <li>Thời gian bắt đầu: <strong>{existingSubscription.StartDate:dd/MM/yyyy}</strong></li>
                <li>Thời gian kết thúc: <strong>{existingSubscription.EndDate:dd/MM/yyyy}</strong></li>
            </ul>
            <p>Cảm ơn bạn đã tin tưởng và lựa chọn LingoVerse!</p>
            <p><a href='mailto:support@lingoverse.com' class='button'>Liên hệ hỗ trợ</a></p>
        </div>
        <div class='footer'>
            <p>Trân trọng,<br/>Đội ngũ LingoVerse<br/>© 2025 LingoVerse. All rights reserved.</p>
        </div>
    </div>
</body>
</html>",
                        IsHtml = true
                    };
                }

                await _emailService.SendEmailAsync(emailMessage);
                return new JsonResult(new { success = true, message = "Email xác nhận đã được gửi" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Lỗi khi gửi email: {ex.Message}" });
            }
        }

        // Định nghĩa các class phụ trợ
        public class SepayTransactionResponse
        {
            [JsonPropertyName("status")]
            public int Status { get; set; }

            [JsonPropertyName("error")]
            public object Error { get; set; }

            [JsonPropertyName("messages")]
            public Messages Messages { get; set; }

            [JsonPropertyName("transactions")]
            public List<Transaction> Transactions { get; set; }
        }

        public class Messages
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }
        }

        public class Transaction
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("transaction_content")]
            public string TransactionContent { get; set; }

            // Các thuộc tính khác nếu cần
        }
    }
}