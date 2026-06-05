namespace BlazorEFDemo.Common
{
    public class ServiceResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;

        public static ServiceResult Ok(string message = "Success")
            => new() { Success = true, Message = message };

        public static ServiceResult Fail(string message)
            => new() { Success = false, Message = message };
    }

    //// Generic version — carries data back to caller
    //public class ServiceResult<T> : ServiceResult
    //{
    //    public bool Success { get; private set; }
    //    public string Message { get; private set; } = string.Empty;

    //    public T? Data { get; private set; }

    //    public static ServiceResult<T> Ok(T data, string message = "Success")
    //        => new() { Success = true, Message = message, Data = data };

    //    public new static ServiceResult<T> Fail(string message)
    //        => new() { Success = false, Message = message };
    //}

}
