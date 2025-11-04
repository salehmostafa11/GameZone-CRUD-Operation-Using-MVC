namespace GameZone.CustomValidations
{
    public class AllowedFileSizeAttribute:ValidationAttribute
    {
        private readonly int _allowedSizeInMegaBytes;

        public AllowedFileSizeAttribute(int allowedSize)
        {
            _allowedSizeInMegaBytes = allowedSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
             var file = value as IFormFile;

            if(file != null)
            {
                if(file.Length > (_allowedSizeInMegaBytes*1024*1024) )
                {
                    return new ValidationResult($"File must be less than {_allowedSizeInMegaBytes}MB");
                }
            }
            return ValidationResult.Success;
        }
    }
}
