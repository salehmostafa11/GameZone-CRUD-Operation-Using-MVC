namespace GameZone.ViewModels
{
    public class AccountFormViewModel
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
