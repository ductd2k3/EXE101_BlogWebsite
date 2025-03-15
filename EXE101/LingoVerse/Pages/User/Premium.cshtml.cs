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

                    // Kiểm tra subscription hiện tại
                    var existingSubscription = (await _userPremiumSubscriptionService.GetAllAsync())
                        .FirstOrDefault(s => s.UserId == user.UserId && s.IsActive && !s.IsDeleted);

                    if (existingSubscription != null)
                    {
                        // Gia hạn subscription hiện tại
                        existingSubscription.EndDate = existingSubscription.EndDate.AddDays(plan.DurationDays); // Cộng thêm thời gian
                        await _userPremiumSubscriptionService.UpdateAsync(existingSubscription);

                        // Gửi email xác nhận gia hạn
                        var emailMessage = new EmailMessage
                        {
                            To = user.Email,
                            Subject = "Xác nhận gia hạn gói Premium",
                            Body = $@"
                        Xin chào {user.Username},<br/><br/>
                        Gói Premium '{plan.PlanName}' của bạn đã được gia hạn thành công!<br/>
                        - Thời gian kết thúc mới: {existingSubscription.EndDate:dd/MM/yyyy}<br/><br/>
                        Cảm ơn bạn đã tiếp tục tin tưởng LingoVerse!<br/>
                        Trân trọng,<br/>
                        Đội ngũ LingoVerse",
                            IsHtml = true
                        };
                        await _emailService.SendEmailAsync(emailMessage);

                        return new JsonResult(new { success = true, message = "Thanh toán thành công và đã gia hạn gói Premium" });
                    }
                    else
                    {
                        // Tạo subscription mới nếu chưa có
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

                        // Gửi email xác nhận kích hoạt mới
                        var emailMessage = new EmailMessage
                        {
                            To = user.Email,
                            Subject = "Xác nhận kích hoạt gói Premium",
                            Body = $@"
                        Xin chào {user.Username},<br/><br/>
                        Gói Premium '{plan.PlanName}' của bạn đã được kích hoạt thành công!<br/>
                        - Thời gian bắt đầu: {subscription.StartDate:dd/MM/yyyy}<br/>
                        - Thời gian kết thúc: {subscription.EndDate:dd/MM/yyyy}<br/><br/>
                        Cảm ơn bạn đã tin tưởng LingoVerse!<br/>
                        Trân trọng,<br/>
                        Đội ngũ LingoVerse",
                            IsHtml = true
                        };
                        await _emailService.SendEmailAsync(emailMessage);

                        return new JsonResult(new { success = true, message = "Thanh toán thành công và đã kích hoạt gói Premium" });
                    }
                }

                return new JsonResult(new { success = false, message = "Không tìm thấy giao dịch khớp" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Lỗi server: {ex.Message}" });
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