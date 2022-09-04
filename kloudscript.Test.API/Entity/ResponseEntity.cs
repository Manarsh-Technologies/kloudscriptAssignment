using System.Net;

namespace kloudscript.Test.API.Entity
{
    public class ResponseEntity
    {
        public ResponseEntity()
        {

        }
        public ResponseEntity(HttpStatusCode _statusCode,bool _result,object? _data,string? _message)
        {
            statusCode = HttpStatusCode.OK;
            result = _result;
            data = _data;
            message = _message;
        }
        public HttpStatusCode statusCode { get; set; }
        public bool result { get; set; }
        public object? data { get; set; }
        public string? message { get; set; }
    }
}
