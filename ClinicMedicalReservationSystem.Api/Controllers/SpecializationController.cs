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
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody]SpecializationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _specializationService.AddAsync(dto);

            if(!result.IsSuccess)
                return BadRequest(new {message = result.Message});

            return Ok(new {message = result.Message});
        }
        #endregion

    }
}
