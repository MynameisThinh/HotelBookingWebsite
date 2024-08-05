namespace HotelBookingWebsite.Models
{
    public readonly record struct MethodResult(bool IsSuccess,string? ErrorMessage)
    {
        public static MethodResult Success() => new(true, null);
        public static MethodResult Failure(string errorMessage) => new(false, errorMessage);
        public static implicit operator MethodResult(bool isSuccess) => new (isSuccess, default); //implicit để định nghĩa chuyển đổi dữ liệu mà k cần ép kiểu
        public static implicit operator MethodResult(string errorMessage) => Failure(errorMessage);
    }
    public record MethodResult<TData>(bool IsSuccess, string? ErrorMessage, TData Data) //Tạo thông báo kiểu generic
    {
        
        public static MethodResult<TData> Success(TData Data) => new(true, null, Data);
        public static MethodResult<TData> Failure(string errorMessage) => new(false, errorMessage, default!);
        public static implicit operator MethodResult<TData>(TData data) => Success(data); //implicit để định nghĩa chuyển đổi dữ liệu mà k cần ép kiểu
        public static implicit operator MethodResult<TData>(string errorMessage) => Failure(errorMessage);
    }
}
