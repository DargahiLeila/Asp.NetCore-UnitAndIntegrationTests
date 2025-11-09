using System.ComponentModel.DataAnnotations;

namespace UnitTest.ViewModel.User
{
    public class UserAddEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "نام ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام وارد نشده است.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "تعداد کاراکترها باید بین 3 تا 100 حرف باشد")]
        [RegularExpression(@"[0-9A-Zا-یa-z_\s\-\(\)\.]+", ErrorMessage = "در {0} کاراکترهای نامعتبر وارد شده است.")]
        public string Name { get; set; }

        public bool IsDeleted {  get; set; }    
        
    }
}
