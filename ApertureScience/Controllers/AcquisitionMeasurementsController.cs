using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionMeasurementsController : ControllerBase
    {
        private readonly IAcquisitionMeasurementRepository _acquisitionMeasurementRepository;

        public AcquisitionMeasurementsController(IAcquisitionMeasurementRepository acquisitionMeasurementRepository)
        {
            _acquisitionMeasurementRepository = acquisitionMeasurementRepository;
        }
        // GET: api/<AcquisitionMeasurementsController>
        [HttpGet]
        public async Task<IActionResult> GetAllMeasurements()
        {
            return Ok(await _acquisitionMeasurementRepository.GetAllMeasurements());
        }

        // GET api/<AcquisitionMeasurementsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeasurement(int id)
        {
            return Ok(await _acquisitionMeasurementRepository.GetById(id));
        }

        // POST api/<AcquisitionMeasurementsController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionMeasurement acquisitionMeasurement)
        {
            Response respCreateAcquisitionMeasurement = new Response();
            respCreateAcquisitionMeasurement.conflicts = new List<Conflicts>();

            if (acquisitionMeasurement == null ||
                string.IsNullOrWhiteSpace(acquisitionMeasurement.Name)
                )
            {
                respCreateAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionMeasurement);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionMeasurementRepository.InsertMeasurement(acquisitionMeasurement);

            if (created == false)
            {
                respCreateAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionMeasurement);
            }

            respCreateAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = false, Description = "Unidad de Medida Creado Correctamente" });

            return Created("created", respCreateAcquisitionMeasurement);
        }

        // PUT api/<AcquisitionMeasurementsController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateMeasurement([FromBody] AcquisitionMeasurement acquisitionMeasurement)
        {
            Response respUpdateAcquisitionMeasurement = new Response();
            respUpdateAcquisitionMeasurement.conflicts = new List<Conflicts>();

            if (acquisitionMeasurement == null)
            {
                respUpdateAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionMeasurement);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionMeasurementRepository.UpdateMeasurement(acquisitionMeasurement);

            if (update == false)
            {
                respUpdateAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionMeasurement);
            }

            respUpdateAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = false, Description = "Unidad de Medida Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionMeasurement);
        }

        // DELETE api/<AcquisitionMeasurementsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            Response respDeleteAcquisitionMeasurement = new Response();
            respDeleteAcquisitionMeasurement.conflicts = new List<Conflicts>();

            var delete = await _acquisitionMeasurementRepository.DeleteMeasurement(new AcquisitionMeasurement { Id = id });

            if (delete == false)
            {
                respDeleteAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAcquisitionMeasurement);
            }
            respDeleteAcquisitionMeasurement.conflicts.Add(new Conflicts { Problems = false, Description = "Unidad de Medida Eliminado Correctamente" });

            return Ok(respDeleteAcquisitionMeasurement);
        }
    }
}
