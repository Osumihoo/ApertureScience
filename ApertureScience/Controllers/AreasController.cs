using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Relational;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;

        public AreasController(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }
        // GET: api/<AreasController>
        [HttpGet]
        public async Task<IActionResult> GetAllAreas()
        {
            return Ok(await _areaRepository.GetAllAreas());
        }

        // GET api/<AreasController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArea(int id)
        {
            return Ok(await _areaRepository.GetById(id));
        }

        // POST api/<AreasController>
        [HttpPost]
        public async Task<IActionResult> CreateArea([FromBody] Area area)
        {
            Response respCreateArea = new Response();
            respCreateArea.conflicts = new List<Conflicts>();

            if (area == null || string.IsNullOrWhiteSpace(area.Name) || area.IdAddress == null)
            {
                respCreateArea.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateArea);

            }
            if (area.IdAddress == 0)
            {
                respCreateArea.conflicts.Add(new Conflicts { Problems = true, Description = "No puedes crear un area sin asignarla a una dirección" });
                return BadRequest(respCreateArea);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _areaRepository.InsertArea(area);

            if (created == false)
            {
                respCreateArea.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateArea);
            }

            respCreateArea.conflicts.Add(new Conflicts { Problems = false, Description = "Area Creado Correctamente" });

            return Created("created", respCreateArea);
        }

        // PUT api/<AreasController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateArea([FromBody] Area area)
        {
            Response respUpdateArea = new Response();
            respUpdateArea.conflicts = new List<Conflicts>();

            if (area == null)
            {
                respUpdateArea.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateArea);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _areaRepository.UpdateArea(area);

            if (update == false)
            {
                respUpdateArea.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateArea);
            }

            respUpdateArea.conflicts.Add(new Conflicts { Problems = false, Description = "Area Actualizado Correctamente" });
            //return NoContent();
            return Ok(respUpdateArea);
        }

        // DELETE api/<AreasController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(int id)
        {
            Response respDeleteArea = new Response();
            respDeleteArea.conflicts = new List<Conflicts>();

            var delete = await _areaRepository.DeleteArea(new Area { Id = id });

            if (delete == false)
            {
                respDeleteArea.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteArea);
            }
            respDeleteArea.conflicts.Add(new Conflicts { Problems = false, Description = "Area Eliminado Correctamente" });
            //return NoContent();
            return Ok(respDeleteArea);
        }
    }
}
