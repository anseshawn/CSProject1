namespace Project1.DTO
{
    public class ResponseDTO<T>
    {
        public Int32? Code { get; set; }
        public String? Message { get; set; }
        public T? Data { get; set; }
    }
}
