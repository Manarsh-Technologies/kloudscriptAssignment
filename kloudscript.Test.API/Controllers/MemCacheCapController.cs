using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Services;
using kloudscript.Test.API.Utility; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace kloudscript.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemCacheCapController : BaseController
    {
        private readonly IMemeoryConfigService memeoryConfigService;
        private readonly ILogger<MemCacheCapController> logger;
        private readonly IOptions<AppSettingsEntity> appSettings;
        object? nullObject = null;
        public MemCacheCapController(IMemeoryConfigService _memeoryConfigService, ILogger<MemCacheCapController> _logger, IOptions<AppSettingsEntity> _appSettings)
        {
            memeoryConfigService = _memeoryConfigService;
            logger = _logger;
            appSettings = _appSettings;
        }
        [HttpPost("SetKeyValueInMemory")]
        public async Task<IActionResult> SetValueInMemory(CacheMemoryEntity? cacheMemoryEntity)
        { 
            try
            {
                if (cacheMemoryEntity == null)
                    return SetResponse(HttpStatusCode.BadRequest, false, nullObject, CommongMsg.InputValueBlank);
                else
                {
                    if (ModelState.IsValid)
                    {
                        bool result = await memeoryConfigService.SetObjectInMemroy(cacheMemoryEntity.CacheKey, cacheMemoryEntity.CacheValue, appSettings.Value.SlidingExpiry,cacheMemoryEntity.ExpirationTime);
                        return SetResponse(HttpStatusCode.OK, result, nullObject, CommongMsg.Success);
                    }
                    else
                    {
                        return SetResponse(HttpStatusCode.BadRequest, false, nullObject, CommongMsg.InputValueBlank);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommongMsg.ExceptionMsg);
            }
        } 

        [HttpGet("GetMemoryDataByKey")]
        public async Task<IActionResult> GetValueFromMemory(string memoryKey)
        { 
            string url = string.Empty;
            try
            {
                object data = await memeoryConfigService.GetObjectFromMemory(memoryKey);
                if (data != null)
                {
                    return SetResponse(HttpStatusCode.OK, true, data, CommongMsg.Success);
                }
                else
                {
                    return SetResponse(HttpStatusCode.NotFound, false, nullObject, CommongMsg.NoValueFound);
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
