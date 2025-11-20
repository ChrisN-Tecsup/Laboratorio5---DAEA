using Lab5_ChristianMamani.Models;
using Lab5_ChristianMamani.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MateriasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Materias
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var materias = await _unitOfWork.Repository<Materia>().GetAllAsync();
            return Ok(materias);
        }

        // GET: api/Materias/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var materia = await _unitOfWork.Repository<Materia>().GetByIdAsync(id);
            if (materia == null)
                return NotFound();

            return Ok(materia);
        }

        // POST: api/Materias
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Materia materia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Materia>().InsertAsync(materia);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = materia.IdMateria }, materia);
        }

        // PUT: api/Materias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Materia materia)
        {
            if (id != materia.IdMateria)
                return BadRequest("El ID no coincide.");

            _unitOfWork.Repository<Materia>().Update(materia);
            await _unitOfWork.Complete();

            return NoContent();
        }

        // DELETE: api/Materias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var materia = await _unitOfWork.Repository<Materia>().GetByIdAsync(id);
            if (materia == null)
                return NotFound();

            _unitOfWork.Repository<Materia>().Delete(materia);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
