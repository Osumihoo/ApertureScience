using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolRepository _rolRepository;

        public RolesController(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }
        // GET: api/<RolesController>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _rolRepository.GetAllRoles());
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRol(int id)
        {
            return Ok(await _rolRepository.GetById(id));
        }

        // POST api/<RolesController>
        [HttpPost]
        public async Task<IActionResult> CreateRol([FromBody] Rol rol)
        {
            Response respCreateRol = new Response();
            respCreateRol.conflicts = new List<Conflicts>();

            if (rol == null || string.IsNullOrWhiteSpace(rol.Type))
            {
                respCreateRol.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateRol);
                
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _rolRepository.InsertRol(rol);

            if (created == false)
            {
                respCreateRol.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateRol);
            }

            respCreateRol.conflicts.Add(new Conflicts { Problems = false, Description = "Rol Creado Correctamente" });

            return Created("created", respCreateRol);
        }

        // PUT api/<RolesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateRol([FromBody] Rol rol)
        {
            Response respUpdateRol = new Response();
            respUpdateRol.conflicts = new List<Conflicts>();

            if (rol == null)
            {
                respUpdateRol.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateRol);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _rolRepository.UpdateRol(rol);
            
            if (update == false)
            {
                respUpdateRol.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateRol);
            }

            respUpdateRol.conflicts.Add(new Conflicts { Problems = false, Description = "Rol Actualizado Correctamente" });
            //return NoContent();
            return Ok(respUpdateRol);
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            Response respDeleteRol = new Response();
            respDeleteRol.conflicts = new List<Conflicts>();

            var delete = await _rolRepository.DeleteRol(new Rol { Id = id });

            if (delete == false)
            {
                respDeleteRol.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteRol);
            }
            respDeleteRol.conflicts.Add(new Conflicts { Problems = false, Description = "Rol Eliminado Correctamente" });
            //return NoContent();
            return Ok(respDeleteRol);
        }
    }
}
