using Lab5_ChristianMamani.Models;
using Lab5_ChristianMamani.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluacionesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EvaluacionesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var evaluaciones = await _unitOfWork.Repository<Evaluacione>().GetAllAsync();
            return Ok(evaluaciones);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evaluacion = await _unitOfWork.Repository<Evaluacione>().GetByIdAsync(id);
            if (evaluacion == null)
                return NotFound();

            return Ok(evaluacion);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Evaluacione evaluacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Evaluacione>().InsertAsync(evaluacion);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = evaluacion.IdEvaluacion }, evaluacion);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Evaluacione evaluacion)
        {
            if (id != evaluacion.IdEvaluacion)
                return BadRequest("El ID no coincide.");

            _unitOfWork.Repository<Evaluacione>().Update(evaluacion);
            await _unitOfWork.Complete();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evaluacion = await _unitOfWork.Repository<Evaluacione>().GetByIdAsync(id);
            if (evaluacion == null)
                return NotFound();

            _unitOfWork.Repository<Evaluacione>().Delete(evaluacion);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
