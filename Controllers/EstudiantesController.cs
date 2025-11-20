using Lab5_ChristianMamani.Models;
using Lab5_ChristianMamani.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EstudiantesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estudiantes = await _unitOfWork.Repository<Estudiante>().GetAllAsync();
            return Ok(estudiantes);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var estudiante = await _unitOfWork.Repository<Estudiante>().GetByIdAsync(id);
            if (estudiante == null)
                return NotFound();

            return Ok(estudiante);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Estudiante>().InsertAsync(estudiante);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = estudiante.IdEstudiante }, estudiante);
        }

        // PUT: api/Estudiantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Estudiante estudiante)
        {
            if (id != estudiante.IdEstudiante)
                return BadRequest("El ID no coincide.");

            _unitOfWork.Repository<Estudiante>().Update(estudiante);
            await _unitOfWork.Complete();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var estudiante = await _unitOfWork.Repository<Estudiante>().GetByIdAsync(id);
            if (estudiante == null)
                return NotFound();

            _unitOfWork.Repository<Estudiante>().Delete(estudiante);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
