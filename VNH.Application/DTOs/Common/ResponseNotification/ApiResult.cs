namespace VNH.Application.DTOs.Common.ResponseNotification
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? ResultObj { get; set; }
    }
}
