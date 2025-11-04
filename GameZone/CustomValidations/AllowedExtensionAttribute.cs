namespace GameZone.CustomValidations
{
    public class AllowedExtensionAttribute:ValidationAttribute
    {
        private readonly string _allowedextensions;
        public AllowedExtensionAttribute(string allowedExtension)
        {
            _allowedextensions = allowedExtension;
        }

        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile; // نتعامل مع الاوبجكت علي انه فورم فايل
            if(file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                var isAllowed = _allowedextensions.Split(',')
                    .Contains(fileExtension,StringComparer.OrdinalIgnoreCase);
                if(!isAllowed)
                    return new ValidationResult ($"Only{_allowedextensions} are allowed! ");

            }
            return ValidationResult.Success;
        }
    }
}
