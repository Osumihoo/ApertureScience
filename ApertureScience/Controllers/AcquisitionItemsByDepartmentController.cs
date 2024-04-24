using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionItemsByDepartmentController : ControllerBase
    {

        private readonly IAcquisitionItemByDepartmentRepository _acquisitionItemByDepartmentRepository;

        public AcquisitionItemsByDepartmentController(IAcquisitionItemByDepartmentRepository acquisitionItemByDepartmentRepository)
        {
            _acquisitionItemByDepartmentRepository = acquisitionItemByDepartmentRepository;
        }

        // GET: api/<AcquisitionItemsByDepartmentController>
        [HttpGet]
        public async Task<IActionResult> GetAllItemByDepartments()
        {
            return Ok(await _acquisitionItemByDepartmentRepository.GetAllItemByDepartments());
        }

        // GET api/<AcquisitionItemsByDepartmentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByDepartment(int id)
        {
            return Ok(await _acquisitionItemByDepartmentRepository.GetByDepartment(id));
        }

        // POST api/<AcquisitionItemsByDepartmentController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionItemByDepartment acquisitionItemByDepartment)
        {
            Response respCreateAcquisitionItemByDepartment = new Response();
            respCreateAcquisitionItemByDepartment.conflicts = new List<Conflicts>();

            if (acquisitionItemByDepartment == null ||
                string.IsNullOrWhiteSpace(acquisitionItemByDepartment.CodeAcSupplies) ||
                acquisitionItemByDepartment.OnHand == 0 ||
                acquisitionItemByDepartment.Remaining == 0 ||
                acquisitionItemByDepartment.Total == 0 ||
                acquisitionItemByDepartment.IdDepartment == 0 
                )
            {
                respCreateAcquisitionItemByDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionItemByDepartment);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionItemByDepartmentRepository.InsertItemByDepartment(acquisitionItemByDepartment);

            if (created == false)
            {
                respCreateAcquisitionItemByDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionItemByDepartment);
            }

            respCreateAcquisitionItemByDepartment.conflicts.Add(new Conflicts { Problems = false, Description = "Unidad de Medida Creado Correctamente" });

            return Created("created", respCreateAcquisitionItemByDepartment);
        }

        // PUT api/<AcquisitionItemsByDepartmentController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateItemByDepartment([FromBody] AcquisitionItemByDepartment acquisitionItemByDepartment)
        {
            Response respUpdateAcquisitionItemByDepartment = new Response();
            respUpdateAcquisitionItemByDepartment.conflicts = new List<Conflicts>();

            if (acquisitionItemByDepartment == null)
            {
                respUpdateAcquisitionItemByDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionItemByDepartment);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionItemByDepartmentRepository.UpdateItemByDepartment(acquisitionItemByDepartment);

            if (update == false)
            {
                respUpdateAcquisitionItemByDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionItemByDepartment);
            }

            respUpdateAcquisitionItemByDepartment.conflicts.Add(new Conflicts { Problems = false, Description = "Unidad de Medida Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionItemByDepartment);
        }

    }
}
