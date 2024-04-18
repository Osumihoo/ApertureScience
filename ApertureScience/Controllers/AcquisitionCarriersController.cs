using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionCarriersController : ControllerBase
    {
        private readonly IAcquisitionCarrierRepository _acquisitionCarrierRepository;

        public AcquisitionCarriersController(IAcquisitionCarrierRepository acquisitionCarrierRepository)
        {
            _acquisitionCarrierRepository = acquisitionCarrierRepository;
        }

        // GET: api/<AcquisitionCarriersController>
        [HttpGet]
        public async Task<IActionResult> GetAllCarriers()
        {
            return Ok(await _acquisitionCarrierRepository.GetAllCarriers());
        }

        // GET api/<AcquisitionCarriersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarrier(int id)
        {
            return Ok(await _acquisitionCarrierRepository.GetById(id));
        }
        
        // POST api/<AcquisitionCarriersController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionCarrier acquisitionCarrier)
        {
            Response respCreateAcquisitionCarrier = new Response();
            respCreateAcquisitionCarrier.conflicts = new List<Conflicts>();

            if (acquisitionCarrier == null ||
                string.IsNullOrWhiteSpace(acquisitionCarrier.Name) ||
                string.IsNullOrWhiteSpace(acquisitionCarrier.LastNameP) ||
                string.IsNullOrWhiteSpace(acquisitionCarrier.LastNameM) 
                )
            {
                respCreateAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionCarrier);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionCarrierRepository.InsertCarrier(acquisitionCarrier);

            if (created == false)
            {
                respCreateAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionCarrier);
            }

            respCreateAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = false, Description = "Transportista Creado Correctamente" });

            return Created("created", respCreateAcquisitionCarrier);
        }

        // PUT api/<AcquisitionCarriersController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateCarrier([FromBody] AcquisitionCarrier acquisitionCarrier)
        {
            Response respUpdateAcquisitionCarrier = new Response();
            respUpdateAcquisitionCarrier.conflicts = new List<Conflicts>();

            if (acquisitionCarrier == null)
            {
                respUpdateAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionCarrier);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionCarrierRepository.UpdateCarrier(acquisitionCarrier);

            if (update == false)
            {
                respUpdateAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionCarrier);
            }

            respUpdateAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = false, Description = "Transpportista Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionCarrier);
        }

        // DELETE api/<AcquisitionCarriersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrier(int id)
        {
            Response respDeleteAcquisitionCarrier = new Response();
            respDeleteAcquisitionCarrier.conflicts = new List<Conflicts>();

            var delete = await _acquisitionCarrierRepository.DeleteCarrier(new AcquisitionCarrier { Id = id });

            if (delete == false)
            {
                respDeleteAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAcquisitionCarrier);
            }
            respDeleteAcquisitionCarrier.conflicts.Add(new Conflicts { Problems = false, Description = "Transportista Eliminado Correctamente" });

            return Ok(respDeleteAcquisitionCarrier);
        }
    }
}
