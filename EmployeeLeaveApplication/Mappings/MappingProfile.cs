using AutoMapper;
using EmployeeLeaveApplication.Models;

namespace EmployeeLeaveApplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map LeaveRequest → LeaveRequestDto
            CreateMap<LeaveRequest, Leave>()                
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => (src.EndDate - src.StartDate).Days + 1));

            // Map LeaveRequestDto → LeaveRequest (reverse map)
            CreateMap<Leave, LeaveRequest>();
        }
    }
}
