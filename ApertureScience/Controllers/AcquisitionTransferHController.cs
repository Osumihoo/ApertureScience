using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionTransferHController : ControllerBase
    {
        private readonly IAcquisitionTransferHRepository _acquisitionTransferHRepository;

        public AcquisitionTransferHController(IAcquisitionTransferHRepository acquisitionTransferHRepository)
        {
            _acquisitionTransferHRepository = acquisitionTransferHRepository;
        }

        // GET: api/<AcquisitionTransferHController>
        [HttpGet]
        public async Task<IActionResult> GetAllTransferH()
        {
            return Ok(await _acquisitionTransferHRepository.GetAllTransferH());
        }

        // GET api/<AcquisitionTransferHController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransferH(int id)
        {
            return Ok(await _acquisitionTransferHRepository.GetById(id));
        }

        // GET api/<AcquisitionTransferHController>/date
        [HttpGet("date")]
        public async Task<IActionResult> GetReleaseHForDate(DateTime startDate, DateTime endDate)
        {
            return Ok(await _acquisitionTransferHRepository.GetByDates(startDate, endDate));
        }

        // POST api/<AcquisitionTransferHController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionTransferH acquisitionTransferH)
        {
            Response respCreateTransferH = new Response();
            respCreateTransferH.responseIdReleaseH = new ResponseIdReleaseH();
            respCreateTransferH.conflicts = new List<Conflicts>();

            if (acquisitionTransferH == null ||
                !DateTime.TryParse(acquisitionTransferH.Date.ToString(), out _) ||
                string.IsNullOrWhiteSpace(acquisitionTransferH.Type) ||
                string.IsNullOrWhiteSpace(acquisitionTransferH.Elaborated) ||
                //string.IsNullOrWhiteSpace(acquisitionReleaseH.Observations) ||
                acquisitionTransferH.IdAddressRelease == 0 ||
                acquisitionTransferH.IdAddressEntry == 0 ||
                acquisitionTransferH.IdAcCarrier == 0 ||
                acquisitionTransferH.IdAcVehicle == 0 ||
                acquisitionTransferH.IdDepartmentEntry == 0 ||
                acquisitionTransferH.IdDepartmentRelease == 0
                )
            {
                respCreateTransferH.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateTransferH);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionTransferHRepository.InsertTransferH(acquisitionTransferH);

            if (created == 0)
            {
                respCreateTransferH.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateTransferH);
            }

            respCreateTransferH.responseIdReleaseH.Id = created;
            respCreateTransferH.conflicts.Add(new Conflicts { Problems = false, Description = "Transferencía Creada Correctamente" });

            return Created("created", respCreateTransferH);
        }

        // PUT api/<AcquisitionTransferHRController>/release
        [HttpPut("release")]
        public async Task<IActionResult> AuthorizeTransferHR(int id, int status, int code)
        {
            Response respAuthorizeTransferHR = new Response();
            respAuthorizeTransferHR.conflicts = new List<Conflicts>();


            if (id == 0 || status != 2)
            {
                respAuthorizeTransferHR.conflicts.Add(new Conflicts { Problems = true, Description = "Ninguno de los campos puede ser 0" });
                return BadRequest(respAuthorizeTransferHR);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var codeCorrect = await _acquisitionTransferHRepository.GetById(id);

            if (codeCorrect.ReleaseCode != code)
            {
                respAuthorizeTransferHR.conflicts.Add(new Conflicts { Problems = true, Description = "El código es incorrecto" });
                return BadRequest(respAuthorizeTransferHR);
            }

            var update = await _acquisitionTransferHRepository.AuthorizeTransferReleaseH(id, status);

            if (update == false)
            {
                respAuthorizeTransferHR.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Autorizada" });
                return BadRequest(respAuthorizeTransferHR);
            }

            respAuthorizeTransferHR.conflicts.Add(new Conflicts { Problems = false, Description = "Transferencía Autorizada Correctamente" });
            return Ok(respAuthorizeTransferHR);
        }

        // DELETE api/<AcquisitionTransferHEController>/entry
        [HttpPut("entry")]
        public async Task<IActionResult> AuthorizeTransferHE(int id, int status, int code)
        {
            Response respAuthorizeTransferHE = new Response();
            respAuthorizeTransferHE.conflicts = new List<Conflicts>();


            if (id == 0 || status != 1)
            {
                respAuthorizeTransferHE.conflicts.Add(new Conflicts { Problems = true, Description = "Ninguno de los campos puede ser 0" });
                return BadRequest(respAuthorizeTransferHE);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var codeCorrect = await _acquisitionTransferHRepository.GetById(id);

            if (codeCorrect.ReceptionCode != code)
            {
                respAuthorizeTransferHE.conflicts.Add(new Conflicts { Problems = true, Description = "El código es incorrecto" });
                return BadRequest(respAuthorizeTransferHE);
            }

            var update = await _acquisitionTransferHRepository.AuthorizeTransferEntryH(id, status);

            if (update == false)
            {
                respAuthorizeTransferHE.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Autorizada" });
                return BadRequest(respAuthorizeTransferHE);
            }

            respAuthorizeTransferHE.conflicts.Add(new Conflicts { Problems = false, Description = "Transferencía Autorizada Correctamente" });
            return Ok(respAuthorizeTransferHE);
        }
    }
}
