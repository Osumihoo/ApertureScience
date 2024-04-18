using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionEntriesLController : ControllerBase
    {
        private readonly IAcquisitionEntryLRepository _acquisitionEntryLRepository;

        public AcquisitionEntriesLController(IAcquisitionEntryLRepository acquisitionEntryLRepository)
        {
            _acquisitionEntryLRepository = acquisitionEntryLRepository;
        }
        // GET: api/<AcquisitionEntriesLController>
        [HttpGet]
        public async Task<IActionResult> GetAllEntriesL()
        {
            return Ok(await _acquisitionEntryLRepository.GetAllEntryL());
        }

        // GET api/<AcquisitionEntriesLController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntryL(int id)
        {
            return Ok(await _acquisitionEntryLRepository.GetById(id));
        }

        [HttpGet("byheader")]
        public async Task<IActionResult> GetAllEntriesLByHeader(int id)
        {
            return Ok(await _acquisitionEntryLRepository.GetAllEntryLByHeader(id));
        }

        // POST api/<AcquisitionEntriesLController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IEnumerable<AcquisitionEntryL> acquisitionEntryList)
        {
            Response respCreateAcquisitionEntryL = new Response();
            respCreateAcquisitionEntryL.conflicts = new List<Conflicts>();

            foreach (var entryL in acquisitionEntryList)
            {
                if (entryL == null ||
                entryL.Quantity == 0 ||
                entryL.UnitPrice == 0 ||
                entryL.Total == 0 ||
                entryL.IdAcEntryH == 0 ||
                entryL.IdAcSupplies == 0
                )
                {
                    respCreateAcquisitionEntryL.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                    return BadRequest(respCreateAcquisitionEntryL);

                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionEntryLRepository.InsertEntryL(acquisitionEntryList);

            if (created == false)
            {
                respCreateAcquisitionEntryL.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateAcquisitionEntryL);
            }

            respCreateAcquisitionEntryL.conflicts.Add(new Conflicts { Problems = false, Description = "Entrada Creada Correctamente" });

            return Created("created", respCreateAcquisitionEntryL);
        }

        //// PUT api/<AcquisitionEntriesLController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AcquisitionEntriesLController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
