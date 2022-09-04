using kloudscript.Test.API.Utility;
using System.ComponentModel.DataAnnotations;

namespace kloudscript.Test.API.Entity
{
    public class UrlRequestEntity
    {
        [Required]
        [UrlValidateAttr(ErrorMessage = "Please enter a valid Url")]
        public string? RequestUrl { get; set; }
        public DateTime RequestDate { get; set; }
        public UrlRequestEntity()
        {
            RequestDate=DateTime.Now;
        }
    }
}
