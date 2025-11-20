using Lab5_ChristianMamani.Models;
using Lab5_ChristianMamani.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CursosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cursos = await _unitOfWork.Repository<Curso>().GetAllAsync();
            return Ok(cursos);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _unitOfWork.Repository<Curso>().GetByIdAsync(id);
            if (curso == null)
                return NotFound();

            return Ok(curso);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Curso curso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Curso>().InsertAsync(curso);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = curso.IdCurso }, curso);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Curso curso)
        {
            if (id != curso.IdCurso)
                return BadRequest("El ID no coincide.");

            _unitOfWork.Repository<Curso>().Update(curso);
            await _unitOfWork.Complete();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var curso = await _unitOfWork.Repository<Curso>().GetByIdAsync(id);
            if (curso == null)
                return NotFound();

            _unitOfWork.Repository<Curso>().Delete(curso);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
