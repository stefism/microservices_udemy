using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utility
{
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                string extension = Path.GetExtension(file.FileName);

                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult($"File extension is not allowed. Use only this extensions: {string.Join(", ", _extensions)}."); 
                }
            }

            return ValidationResult.Success;
        }
    }
}
