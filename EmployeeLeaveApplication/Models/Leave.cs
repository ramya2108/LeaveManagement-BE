namespace EmployeeLeaveApplication.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Days { get; set; }
        public string? Status { get; set; }
        public int? ActionedBy { get; set; }
    }
}
