using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Services;
using kloudscript.Test.API.Utility; 
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace kloudscript.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateByZipController : BaseController
    {
        IStateByZipService stateByZipService;
        ILogger<StateByZipController> logger;
        object? nullObject = null;
        public StateByZipController(IStateByZipService _stateByZipService, ILogger<StateByZipController> _logger)
        {
            stateByZipService=_stateByZipService;
            logger = _logger;
        }

        [HttpGet("GetDetailByZip")]
        public async Task<IActionResult> GetDetailByZipCode(string zipCode)
        {
            try
            {
                if (string.IsNullOrEmpty(zipCode)==false)
                {
                    object? result = await stateByZipService.GetStateByZipAsync(zipCode);
                    return SetResponse(HttpStatusCode.OK, true, result, CommongMsg.Success);
                }
                else
                {                     
                    return SetResponse(HttpStatusCode.BadRequest, false, nullObject, CommongMsg.InputValueBlank);
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
