using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Services;
using kloudscript.Test.API.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Net;

namespace kloudscript.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortController : BaseController
    {
        private readonly IUrlshorteningService urlShortService;
        private readonly ILogger<UrlShortController> logger;
        private IOptions<AppSettingsEntity> appSettings;
        private object? nullObject = null;
        public UrlShortController(IUrlshorteningService _urlShortService, ILogger<UrlShortController> _logger, IOptions<AppSettingsEntity> _appSettings)
        {
            urlShortService = _urlShortService;
            logger = _logger;
            appSettings= _appSettings;
        }
        [HttpPost("GenerateUrl")]
        public async Task<IActionResult> CreateShortenUrl(UrlRequestEntity originalUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = await urlShortService.GenerateShortUrl(originalUrl.RequestUrl, appSettings.Value.SlidingExpiry, appSettings.Value.AbsExpiry);
                    return SetResponse(HttpStatusCode.OK, true, result, CommongMsg.Success);
                }
                else
                {
                    string errorMessage = string.Empty;
                    ModelState.TryGetValue("RequestUrl", out ModelStateEntry? entityMessage);
                    if (entityMessage != null && entityMessage.Errors != null && entityMessage.Errors.Count > 0)
                    {
                        errorMessage = entityMessage.Errors[0].ErrorMessage;
                    }
                    return SetResponse(HttpStatusCode.BadRequest, false, nullObject, errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommongMsg.ExceptionMsg);
            }
        }

        [HttpGet("GetUrl")] 
        public async Task<IActionResult> GetOriginalUrl(string shortanUrl)
        {
             
            string url = string.Empty;
            try
            {
                url = await urlShortService.RedirectToUrl(shortanUrl);
                if (string.IsNullOrEmpty(url) == false)
                {
                    RedirectResult redirectUrl = Redirect(url);
                    return SetResponse(HttpStatusCode.Redirect, false, redirectUrl.Url, CommongMsg.Success);
                }
                else
                {
                    return SetResponse(HttpStatusCode.NotFound, false, nullObject, CommongMsg.UrlNotFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommongMsg.ExceptionMsg);
            } 
        } 
    }
}
