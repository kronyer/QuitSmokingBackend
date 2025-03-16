using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuitSmoking.Application.Services;
using QuitSmoking.Domain.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuitSmoking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly ISmokingHistoryService _smokingHistoryService;
        private readonly ISmokingHistoryDomainService _domainSmokingHistoryService;
        private readonly ISmokingProgressServices _smokingProgressService;
        private readonly IMapper _mapper;

        public UserProgressController(ISmokingHistoryService smokingHistoryService, IMapper mapper, ISmokingHistoryDomainService domainSmokingHistoryService, ISmokingProgressServices smokingProgressService)
        {
            _smokingHistoryService = smokingHistoryService;
            _mapper = mapper;
            _domainSmokingHistoryService = domainSmokingHistoryService;
            _smokingProgressService = smokingProgressService;
        }
        [HttpGet("current-challenge")]
        public async Task<IActionResult> GetCurrentChallenge()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var challenge = await _smokingProgressService.GetCurrentChallenge(userId);
            if (challenge == null)
            {
                return NotFound(new { Error = "No current challenge found." });
            }
            return Ok(challenge);
        }

        [HttpGet("is-success")]
        public async Task<IActionResult> IsSuccess()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception();
            }

            var success = await _smokingProgressService.IsSuccess(userId);
            return Ok(success);
        }

        [HttpGet("challenge-results")]
        public async Task<IActionResult> ChallengeResults()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not found");
            }
            var success = await _smokingProgressService.ChallengeResume(userId);
            return Ok(success);
        }


    }
}

