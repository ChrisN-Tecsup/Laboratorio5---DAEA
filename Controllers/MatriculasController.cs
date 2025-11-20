using Lab5_ChristianMamani.Models;
using Lab5_ChristianMamani.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MatriculasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var matriculas = await _unitOfWork.Repository<Matricula>().GetAllAsync();
            return Ok(matriculas);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var matricula = await _unitOfWork.Repository<Matricula>().GetByIdAsync(id);
            if (matricula == null)
                return NotFound();

            return Ok(matricula);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Matricula matricula)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Matricula>().InsertAsync(matricula);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = matricula.IdMatricula }, matricula);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Matricula matricula)
        {
            if (id != matricula.IdMatricula)
                return BadRequest("El ID no coincide.");

            _unitOfWork.Repository<Matricula>().Update(matricula);
            await _unitOfWork.Complete();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var matricula = await _unitOfWork.Repository<Matricula>().GetByIdAsync(id);
            if (matricula == null)
                return NotFound();

            _unitOfWork.Repository<Matricula>().Delete(matricula);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
