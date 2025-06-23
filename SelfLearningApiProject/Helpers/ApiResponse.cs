namespace SelfLearningApiProject.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(string message, T data)
        {
            Message = message;
            Data = data;
        }
    }
}
