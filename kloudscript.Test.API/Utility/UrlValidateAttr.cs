using System.ComponentModel.DataAnnotations;

namespace kloudscript.Test.API.Utility
{
    public class UrlValidateAttr : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? Url, ValidationContext validationContext)
        {
            Uri? uriResult;
            if (Url != null)
            {
                bool result = Uri.TryCreate(Url.ToString(), UriKind.Absolute, out uriResult)
                  && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (result)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Please enter a valid Url");
                }
            }
            else
            {
                return new ValidationResult("Url can not be blank");
            }
        }
    }
}
