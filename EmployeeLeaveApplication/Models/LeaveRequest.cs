namespace EmployeeLeaveApplication.Models
{
    public class LeaveRequest
    {        
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
        public int LeaveStatusId { get; set; }
        public int? ActionedBy { get; set; }
    }
}
