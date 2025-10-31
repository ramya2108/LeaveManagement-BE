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
            if (leaveRequest.EndDate < leaveRequest.StartDate)
            {
                return BadRequest("End date cannot be earlier than start date.");
            }
            leaveRequest.LeaveStatusId = 1;
            var leave = _mapper.Map<Leave>(leaveRequest);
            var response = await _jsonServerService.AddLeaveAsync(leave);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(LeaveRequest leaveRequest)
        {
            var leave = await _jsonServerService.GetLeaveRequestAsync(leaveRequest.Id);
            if (leave is null)
            {
                return NotFound("Leave request not found.");
            }
            if (leave.LeaveStatusId == 2 || leave.LeaveStatusId == 3)
            {
                return BadRequest("Leave request has already been actioned and cannot be updated.");
            }
            leave.ActionedBy = leaveRequest.ActionedBy;
            leave.LeaveStatusId = leaveRequest.LeaveStatusId;
            var response = await _jsonServerService.UpdateLeaveAsync(leave);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var response = await _jsonServerService.GetLeaveTypessAsync();
            return Ok(response);
        }
    }
}
