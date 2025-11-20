using Lab5_ChristianMamani.Models;
using Lab5_ChristianMamani.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsistenciasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AsistenciasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var asistencias = await _unitOfWork.Repository<Asistencia>().GetAllAsync();
            return Ok(asistencias);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asistencia = await _unitOfWork.Repository<Asistencia>().GetByIdAsync(id);
            if (asistencia == null)
                return NotFound();

            return Ok(asistencia);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Asistencia asistencia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Asistencia>().InsertAsync(asistencia);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = asistencia.IdAsistencia }, asistencia);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Asistencia asistencia)
        {
            if (id != asistencia.IdAsistencia)
                return BadRequest("El ID no coincide.");

            _unitOfWork.Repository<Asistencia>().Update(asistencia);
            await _unitOfWork.Complete();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asistencia = await _unitOfWork.Repository<Asistencia>().GetByIdAsync(id);
            if (asistencia == null)
                return NotFound();

            _unitOfWork.Repository<Asistencia>().Delete(asistencia);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
