using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuitSmoking.Application.DTOs;
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
        private readonly IMapper _mapper;

        public CigarretesController(ICigarretesService cigarretesService, IMapper mapper)
        {
            _cigarretesService = cigarretesService;
            _mapper = mapper;
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
    }
}




