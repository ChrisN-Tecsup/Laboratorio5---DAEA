using Lab5_ChristianMamani.Models;
using Lab5_ChristianMamani.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesoresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfesoresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profesores = await _unitOfWork.Repository<Profesore>().GetAllAsync();
            return Ok(profesores);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesor = await _unitOfWork.Repository<Profesore>().GetByIdAsync(id);
            if (profesor == null)
                return NotFound();

            return Ok(profesor);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Profesore profesor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Profesore>().InsertAsync(profesor);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = profesor.IdProfesor }, profesor);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Profesore profesor)
        {
            if (id != profesor.IdProfesor)
                return BadRequest("El ID no coincide.");

            _unitOfWork.Repository<Profesore>().Update(profesor);
            await _unitOfWork.Complete();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profesor = await _unitOfWork.Repository<Profesore>().GetByIdAsync(id);
            if (profesor == null)
                return NotFound();

            _unitOfWork.Repository<Profesore>().Delete(profesor);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
