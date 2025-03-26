using LingoVerse.Models;
using LingoVerse.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LingoVerse.Pages.Manager
{
    public class Admin_ManagePremiumModel : PageModel
    {
        private readonly IGenericService<PremiumPlan> _premiumPlanService;

        public Admin_ManagePremiumModel(IGenericService<PremiumPlan> premiumPlanService)
        {
            _premiumPlanService = premiumPlanService;
        }

        public IEnumerable<PremiumPlan> PremiumPlans { get; set; }

        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            PremiumPlans = await _premiumPlanService.GetAllAsync();
            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostUpdatePlanAsync(int PlanId, string PlanName, string Description, int DurationDays, decimal Price)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(PlanName) || string.IsNullOrWhiteSpace(Description) || DurationDays <= 0 || Price <= 0)
            {
                ModelState.AddModelError("", "Vui lòng điền đầy đủ và hợp lệ thông tin.");
                // Tải lại danh sách để hiển thị trang
                PremiumPlans = await _premiumPlanService.GetAllAsync();
                return Page();
            }

            // Lấy plan từ dịch vụ
            var plan = await _premiumPlanService.GetByIdAsync(PlanId);
            if (plan == null)
            {
                ModelState.AddModelError("", "Không tìm thấy gói với ID này.");
                PremiumPlans = await _premiumPlanService.GetAllAsync();
                return Page();
            }

            // Cập nhật thông tin
            plan.PlanName = PlanName;
            plan.Description = Description;
            plan.DurationDays = DurationDays;
            plan.Price = Price;

            try
            {
                // Đảm bảo cập nhật hoàn tất trước khi tiếp tục
                await _premiumPlanService.UpdateAsync(plan);
                TempData["SuccessMessage"] = "Cập nhật gói thành công!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Có lỗi xảy ra khi cập nhật: {ex.Message}");
                PremiumPlans = await _premiumPlanService.GetAllAsync();
                return Page();
            }

            // Tải lại danh sách sau khi cập nhật thành công
            PremiumPlans = await _premiumPlanService.GetAllAsync();
            return Page();
        }
    }
}