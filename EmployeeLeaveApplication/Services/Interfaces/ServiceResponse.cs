namespace EmployeeLeaveApplication.Services.Interfaces
{
    public class ServiceResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ServiceResponse()
        {
            Status = true;
        }
    }
}