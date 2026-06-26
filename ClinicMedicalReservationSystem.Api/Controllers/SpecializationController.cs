using ClinicMedicalReservationSystem.Application.DTOs;
using ClinicMedicalReservationSystem.Application.Interfaces;
using ClinicMedicalReservationSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicMedicalReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        #region Fields
        private readonly ISpecializationService _specializationService;
        #endregion

        #region Constructor
        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }
        #endregion

        #region CRUD Operations 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationReviewDto>>> GetAllAsync([FromQuery]string? search)
        {
            var specializationDtos = await _specializationService.GetAllAsync(search);
            return Ok(specializationDtos);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody]SpecializationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _specializationService.AddAsync(dto);

            if(!result.IsSuccess)
                return BadRequest(new {message = result.Message});

            return Ok(new {message = result.Message});
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecializationReviewDto>> GetByIdAsync([FromRoute]int id)
        {
            var result = await _specializationService.GetByIdAsync(id);
           
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<SpecializationReviewDto>> UpdateAsync([FromRoute] int id, [FromBody] SpecializationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var specializationDto = await _specializationService.UpdateAsync(id, dto);

            if (specializationDto==null)
                return BadRequest(new { message = "Can't update specialization name."});

            return Ok(specializationDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var result = await _specializationService.DeleteById(id);
            if(!result.IsSuccess)
                return BadRequest(new {message = result.Message});

            return Ok(new { message = result.Message });
        }

        #endregion

    }
}
