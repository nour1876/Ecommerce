
namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode ,string message = null) { 
        StatusCode = statusCode;
        Message = message ?? 
                GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad request , you have made",
                401 => "Authorized , you are not",
                404 => "Resource Not Found",
                500 => "Server Error",
                _ => null

            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
