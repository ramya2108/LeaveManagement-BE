using AutoMapper;
using EmployeeLeaveApplication.Models;
using EmployeeLeaveApplication.Services;
using EmployeeLeaveApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly JsonServerService _jsonServerService;
        
        public LeavesController(IMapper mapper, JsonServerService jsonServerService) 
        { 
            _mapper = mapper;
            _jsonServerService = jsonServerService;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Apply(LeaveRequest leaveRequest)
        {
            if(leaveRequest.EndDate < leaveRequest.StartDate)
            {
                return BadRequest("End date cannot be earlier than start date.");
            }
            var leave = _mapper.Map<Leave>(leaveRequest);
            var response = await _jsonServerService.AddLeaveAsync(leave);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(int id, LeaveRequest leaveRequest)
        {            
            var leave = await _jsonServerService.GetLeaveRequestAsync(id);
            if (leave is null)
            {
                return NotFound("Leave request not found.");
            }
            leave.ActionedBy = id;
            leave.Status = leaveRequest.Status;
            var response = await _jsonServerService.UpdateLeaveAsync(leave);
            return Ok(response);
        }  
    }
}
