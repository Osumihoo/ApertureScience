using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionEntriesHController : ControllerBase
    {
        private readonly IAcquisitionEntryHRepository _acquisitionEntryHRepository;

        public AcquisitionEntriesHController(IAcquisitionEntryHRepository acquisitionEntryHRepository)
        {
            _acquisitionEntryHRepository = acquisitionEntryHRepository;
        }
        // GET: api/<AquisitionEntryHController>
        [HttpGet]
        public async Task<IActionResult> GetAllEntriesH()
        {
            return Ok(await _acquisitionEntryHRepository.GetAllEntryH());
        }

        // GET api/<AquisitionEntryHController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntryH(int id)
        {
            return Ok(await _acquisitionEntryHRepository.GetById(id));
        }

        // GET api/<AquisitionEntryHController>/date
        [HttpGet("date")]
        public async Task<IActionResult> GetEntriesHForDate(DateTime startDate, DateTime endDate)
        {
            return Ok(await _acquisitionEntryHRepository.GetByDates(startDate, endDate));
        }

        // POST api/<AquisitionEntryHController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionEntryH acquisitionEntryH)
        {
            Response respCreateAcquisitionEntryH = new Response();
            respCreateAcquisitionEntryH.responseIdEntryH = new ResponseIdEntryH();
            respCreateAcquisitionEntryH.conflicts = new List<Conflicts>();

            if (acquisitionEntryH == null ||
                !DateTime.TryParse(acquisitionEntryH.Date.ToString(), out _) ||
                string.IsNullOrWhiteSpace(acquisitionEntryH.Type) ||
                string.IsNullOrWhiteSpace(acquisitionEntryH.Elaborated) ||
                string.IsNullOrWhiteSpace(acquisitionEntryH.Observations) ||
                acquisitionEntryH.IdAcReleaseH == 0 ||
                acquisitionEntryH.IdAddressRelease == 0 ||
                acquisitionEntryH.IdAddressEntry == 0 ||
                acquisitionEntryH.IdAcCarrier == 0 ||
                acquisitionEntryH.IdAcVehicle == 0
                )
            {
                respCreateAcquisitionEntryH.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionEntryH);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionEntryHRepository.InsertEntryH(acquisitionEntryH);

            if (created == 0)
            {
                respCreateAcquisitionEntryH.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateAcquisitionEntryH);
            }

            respCreateAcquisitionEntryH.responseIdEntryH.Id = created;
            respCreateAcquisitionEntryH.conflicts.Add(new Conflicts { Problems = false, Description = "Entrada Creada Correctamente" });

            return Created("created", respCreateAcquisitionEntryH);
        }

        // PUT api/<AquisitionEntryHController>/5
        [HttpPut]
        public async Task<IActionResult> AuthorizeEntryH(int id, int status, int code)
        {
            Response respAuthorizeEntryH = new Response();
            respAuthorizeEntryH.conflicts = new List<Conflicts>();


            if (id == 0 || status == 0)
            {
                respAuthorizeEntryH.conflicts.Add(new Conflicts { Problems = true, Description = "Ninguno de los campos puede ser 0" });
                return BadRequest(respAuthorizeEntryH);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var codeCorrect = await _acquisitionEntryHRepository.GetById(id);

            if (codeCorrect.ReceptionCode != code)
            {
                respAuthorizeEntryH.conflicts.Add(new Conflicts { Problems = true, Description = "El código es incorrecto" });
                return BadRequest(respAuthorizeEntryH);
            }

            var update = await _acquisitionEntryHRepository.AuthorizeEntryH(id, status);

            if (update == false)
            {
                respAuthorizeEntryH.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Autorizada" });
                return BadRequest(respAuthorizeEntryH);
            }

            respAuthorizeEntryH.conflicts.Add(new Conflicts { Problems = false, Description = "Orden Autorizada Correctamente" });
            return Ok(respAuthorizeEntryH);
        }

        //// DELETE api/<AquisitionEntryHController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
