using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionVehiclesController : ControllerBase
    {
        private readonly IAcquisitionVehicleRepository _acquisitionVehicleRepository;

        public AcquisitionVehiclesController(IAcquisitionVehicleRepository acquisitionVehicleRepository)
        {
            _acquisitionVehicleRepository = acquisitionVehicleRepository;
        }

        // GET: api/<AcquisitionVehiclesController>
        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            return Ok(await _acquisitionVehicleRepository.GetAllVehicles());
        }

        // GET api/<AcquisitionVehiclesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            return Ok(await _acquisitionVehicleRepository.GetById(id));
        }

        // POST api/<AcquisitionVehiclesController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionVehicle acquisitionVehicle)
        {
            Response respCreateAcquisitionVehicle = new Response();
            respCreateAcquisitionVehicle.conflicts = new List<Conflicts>();

            if (acquisitionVehicle == null ||
                string.IsNullOrWhiteSpace(acquisitionVehicle.Name) ||
                string.IsNullOrWhiteSpace(acquisitionVehicle.Plate) 
                )
            {
                respCreateAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionVehicle);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionVehicleRepository.InsertVehicle(acquisitionVehicle);

            if (created == false)
            {
                respCreateAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionVehicle);
            }

            respCreateAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = false, Description = "Vehículo Creado Correctamente" });

            return Created("created", respCreateAcquisitionVehicle);
        }

        // PUT api/<AcquisitionVehiclesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateVehicle([FromBody] AcquisitionVehicle acquisitionVehicle)
        {
            Response respUpdateAcquisitionVehicle = new Response();
            respUpdateAcquisitionVehicle.conflicts = new List<Conflicts>();

            if (acquisitionVehicle == null)
            {
                respUpdateAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionVehicle);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionVehicleRepository.UpdateVehicle(acquisitionVehicle);

            if (update == false)
            {
                respUpdateAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionVehicle);
            }

            respUpdateAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = false, Description = "Vehículo Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionVehicle);
        }

        // DELETE api/<AcquisitionVehiclesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            Response respDeleteAcquisitionVehicle = new Response();
            respDeleteAcquisitionVehicle.conflicts = new List<Conflicts>();

            var delete = await _acquisitionVehicleRepository.DeleteVehicle(new AcquisitionVehicle { Id = id });

            if (delete == false)
            {
                respDeleteAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAcquisitionVehicle);
            }
            respDeleteAcquisitionVehicle.conflicts.Add(new Conflicts { Problems = false, Description = "Vehículo Eliminado Correctamente" });

            return Ok(respDeleteAcquisitionVehicle);
        }
    }
}
