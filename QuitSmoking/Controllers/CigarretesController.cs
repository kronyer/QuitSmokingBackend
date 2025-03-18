using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuitSmoking.Application.DTOs;
using QuitSmoking.Application.Services;
using QuitSmoking.Domain.Entities;
using QuitSmoking.Domain.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuitSmoking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CigarretesController : ControllerBase
    {
        private readonly ICigarretesService _cigarretesService;
        private readonly ISmokingProgressServices _smokingProgressService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager; // Add this line //pm interface?


        public CigarretesController(ICigarretesService cigarretesService, IMapper mapper, 
            UserManager<ApplicationUser> userManager, ISmokingProgressServices smokingProgressService)
        {
            _cigarretesService = cigarretesService;
            _mapper = mapper;
            _userManager = userManager;
            _smokingProgressService = smokingProgressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CigarreteDto>>> GetAll()
        {
            var cigarretes = await _cigarretesService.GetAllAsync();
            var cigarretesDto = _mapper.Map<IEnumerable<CigarreteDto>>(cigarretes);
            return Ok(cigarretesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CigarreteDto>> GetById(int id)
        {

            var cigarretes = await _cigarretesService.GetByIdAsync(id);
            if (cigarretes == null)
            {
                return NotFound();
            }
            var cigarretesDto = _mapper.Map<CigarreteDto>(cigarretes);
            return Ok(cigarretesDto);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CigarreteDto cigarretesDto)
        {
            var cigarretes = _mapper.Map<UserCigarrete>(cigarretesDto);
            await _cigarretesService.AddAsync(cigarretes);
            return CreatedAtAction(nameof(GetById), new { id = cigarretes.Id }, cigarretes);
        }
        [HttpPost("first-acesss")]
        [Authorize]
        public async Task<ActionResult> FirstAccess([FromBody] FirstAccessDto firstAccessDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var cigarretes = new UserCigarrete() { Brand = firstAccessDto.Brand, PricePerBox=firstAccessDto.PricePerBox};
            await _cigarretesService.AddAsync(cigarretes);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.CigarreteId = cigarretes.Id;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            var resultProgress = await _smokingProgressService.CreateFirstChallenge(userId, firstAccessDto.CigarretesPerDay);

            if (resultProgress is true)
            {
                return Ok();
            }

            return BadRequest();

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CigarreteDto cigarretesDto)
        {
            var cigarretes = _mapper.Map<UserCigarrete>(cigarretesDto);
            if (id != cigarretes.Id)
            {
                return BadRequest();
            }

            await _cigarretesService.UpdateAsync(cigarretes);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _cigarretesService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("update-user-cigarrete-id/{userId}")]
        public async Task<ActionResult> UpdateUserCigarreteId([FromBody] int cigarreteId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.CigarreteId = cigarreteId;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(errors);
        }

    }
}




