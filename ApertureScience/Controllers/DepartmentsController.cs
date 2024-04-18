using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI.Relational;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        // GET: api/<DepartmentsController>
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            return Ok(await _departmentRepository.GetAllDepartments()); ;
        }

        // GET api/<DepartmentsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            return Ok(await _departmentRepository.GetById(id));
        }

        // POST api/<DepartmentsController>
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            Response respCreateDepartment = new Response();
            respCreateDepartment.conflicts = new List<Conflicts>();
            
            if (department == null || string.IsNullOrWhiteSpace(department.Name) || string.IsNullOrWhiteSpace(department.Code) || string.IsNullOrWhiteSpace(department.Type) || department.IdAddress == null || department.IdAddress == 0)
            {
                respCreateDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateDepartment);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _departmentRepository.InsertDepartment(department);

            if (created == false)
            {
                respCreateDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateDepartment);
            }

            respCreateDepartment.conflicts.Add(new Conflicts { Problems = false, Description = "Departamento Creado Correctamente" });

            return Created("created", respCreateDepartment);
        }

        // PUT api/<DepartmentsController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromBody] Department department)
        {
            Response respUpdateDepartment = new Response();
            respUpdateDepartment.conflicts = new List<Conflicts>();

            if (department == null)
            {
                respUpdateDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateDepartment);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _departmentRepository.UpdateDepartment(department);

            if (update == false)
            {
                respUpdateDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateDepartment);
            }

            respUpdateDepartment.conflicts.Add(new Conflicts { Problems = false, Description = "Departamento Actualizado Correctamente" });
            //return NoContent();
            return Ok(respUpdateDepartment);
        }

        // DELETE api/<DepartmentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            Response respDeleteDepartment = new Response();
            respDeleteDepartment.conflicts = new List<Conflicts>();

            var delete = await _departmentRepository.DeleteDepartment(new Department { Id = id });

            if (delete == false)
            {
                respDeleteDepartment.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteDepartment);
            }
            respDeleteDepartment.conflicts.Add(new Conflicts { Problems = false, Description = "Departamento Eliminado Correctamente" });
            //return NoContent();
            return Ok(respDeleteDepartment);
        }
    }
}
