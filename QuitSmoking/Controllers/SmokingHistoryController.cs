using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitSmoking.Application.DTOs;
using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuitSmoking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SmokingHistoryController : ControllerBase
    {
        private readonly ISmokingHistoryService _smokingHistoryService;
        private readonly IMapper _mapper;

        public SmokingHistoryController(ISmokingHistoryService smokingHistoryService, IMapper mapper)
        {
            _smokingHistoryService = smokingHistoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SmokingHistoryGetDto>>> GetAllByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var smokingHistories = await _smokingHistoryService.GetAllAsync();
            var userSmokingHistories = smokingHistories.Where(sh => sh.UserId == userId).ToList();
            var userSmokingHistoriesDto = _mapper.Map<IEnumerable<SmokingHistoryGetDto>>(userSmokingHistories);
            return Ok(userSmokingHistoriesDto);
        }

        [HttpGet("last-five-hours")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetLastFiveHours()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var lastFiveHours = await _smokingHistoryService.GetLastFiveHoursAsync(userId);
            return Ok(lastFiveHours);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SmokingHistoryGetDto>> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var smokingHistory = await _smokingHistoryService.GetByIdAsync(id);
            if (smokingHistory == null)
            {
                return NotFound();
            }
            if (smokingHistory.UserId != userId)
            {
                return Forbid();
            }
            var smokingHistoryDto = _mapper.Map<SmokingHistoryGetDto>(smokingHistory);
            return Ok(smokingHistoryDto);
        }

        [HttpPost]
        public async Task<ActionResult> Add()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var smokingHistory = await _smokingHistoryService.AddAsync(userId);
            return CreatedAtAction(nameof(GetById), new { id = smokingHistory.Id }, smokingHistory);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] SmokingHistoryUpdateDto smokingHistoryUpdateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var smokingHistory = _mapper.Map<SmokingHistory>(smokingHistoryUpdateDto);
            if (id != smokingHistory.Id)
            {
                return BadRequest();
            }
            var existingSmokingHistory = await _smokingHistoryService.GetByIdAsync(id);
            if (existingSmokingHistory == null || existingSmokingHistory.UserId != userId)
            {
                return Forbid();
            }

            await _smokingHistoryService.UpdateAsync(smokingHistory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var smokingHistory = await _smokingHistoryService.GetByIdAsync(id);
            if (smokingHistory == null)
            {
                return NotFound();
            }
            if (smokingHistory.UserId != userId)
            {
                return Forbid();
            }

            await _smokingHistoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}












