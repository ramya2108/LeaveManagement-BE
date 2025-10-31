namespace EmployeeLeaveApplication.Models
{
    public class LeaveRequest
    {        
        public int Id { get; set; }
        public int UserId { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public string? Status { get; set; }
    }
}
