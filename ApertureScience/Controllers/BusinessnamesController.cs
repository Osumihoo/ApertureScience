using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessnamesController : ControllerBase
    {
        private readonly IBusinessnameRepository _businessnameRepository;

        public BusinessnamesController(IBusinessnameRepository businessnameRepository)
        {
            _businessnameRepository = businessnameRepository;
        }

        // GET: api/<BusinessnameController>
        [HttpGet]
        public async Task<IActionResult> GetAllBusinessnames()
        {
            return Ok(await _businessnameRepository.GetAllBusinessname());
        }

        // GET api/<BusinessnameController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessname(int id)
        {
            return Ok(await _businessnameRepository.GetById(id));
        }

        // POST api/<BusinessnameController>
        [HttpPost]
        public async Task<IActionResult> CreateBusinessname([FromBody] Businessname businessname)
        {
            Response respCreateBusinessname = new Response();
            respCreateBusinessname.conflicts = new List<Conflicts>();

            if (businessname == null || string.IsNullOrWhiteSpace(businessname.Name))
            {
                respCreateBusinessname.conflicts.Add(new Conflicts { Problems = true, Description = "El campo de Nombre es obligatorio" });
                return BadRequest(respCreateBusinessname);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _businessnameRepository.InsertBusinessname(businessname);

            if (created == false)
            {
                respCreateBusinessname.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateBusinessname);
            }

            respCreateBusinessname.conflicts.Add(new Conflicts { Problems = false, Description = "Razón Social Creada Correctamente" });

            return Created("created", respCreateBusinessname);
        }

        // PUT api/<BusinessnameController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateRol([FromBody] Businessname businessname)
        {
            Response respUpdateBusinessname = new Response();
            respUpdateBusinessname.conflicts = new List<Conflicts>();

            if (businessname == null)
            {
                respUpdateBusinessname.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateBusinessname);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _businessnameRepository.UpdateBusinessname(businessname);

            if (update == false)
            {
                respUpdateBusinessname.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateBusinessname);
            }

            respUpdateBusinessname.conflicts.Add(new Conflicts { Problems = false, Description = "Razón Social Actualizada Correctamente" });
            return Ok(respUpdateBusinessname);
        }

        // DELETE api/<BusinessnameController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessname(int id)
        {
            Response respDeleteBusinessname = new Response();
            respDeleteBusinessname.conflicts = new List<Conflicts>();

            var delete = await _businessnameRepository.DeleteBusinessname(new Businessname { Id = id });

            if (delete == false)
            {
                respDeleteBusinessname.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminada" });
                return BadRequest(respDeleteBusinessname);
            }
            respDeleteBusinessname.conflicts.Add(new Conflicts { Problems = false, Description = "Razón Social Eliminada Correctamente" });
            return Ok(respDeleteBusinessname);
        }
    }
}
