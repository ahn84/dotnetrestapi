namespace DotNetRestApi.Controllers.Dto
{
    public class ApiResponse
    {
        public int ErrorCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}