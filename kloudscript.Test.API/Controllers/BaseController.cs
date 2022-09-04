using kloudscript.Test.API.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace kloudscript.Test.API.Controllers
{
    public class BaseController:ControllerBase
    {
        [NonAction]
        public IActionResult SetResponse(HttpStatusCode httpStatus, bool _result, object? _data, string _message)
        {
            return new ObjectResult(new ResponseEntity(httpStatus, _result, _data, _message))
            {
                StatusCode = (int)httpStatus
            };
        }
    }
}
