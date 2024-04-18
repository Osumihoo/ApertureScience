using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionReleaseHController : ControllerBase
    {
        private readonly IAcquisitionReleaseHRepository _acquisitionReleaseHRepository;

        public AcquisitionReleaseHController(IAcquisitionReleaseHRepository acquisitionReleaseHRepository)
        {
            _acquisitionReleaseHRepository = acquisitionReleaseHRepository;
        }

        // GET: api/<AcquisitionReleaseH>
        [HttpGet]
        public async Task<IActionResult> GetAllReleaseH()
        {
            return Ok(await _acquisitionReleaseHRepository.GetAllReleaseH());
        }

        // GET api/<AcquisitionReleaseH>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReleaseH(int id)
        {
            return Ok(await _acquisitionReleaseHRepository.GetById(id));
        }

        // GET api/<AcquisitionReleaseH>/date
        [HttpGet("date")]
        public async Task<IActionResult> GetReleaseHForDate(DateTime startDate, DateTime endDate)
        {
            return Ok(await _acquisitionReleaseHRepository.GetByDates(startDate, endDate));
        }

        // POST api/<AcquisitionReleaseH>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionReleaseH acquisitionReleaseH)
        {
            Response respCreateAcquisitionReleaseH = new Response();
            respCreateAcquisitionReleaseH.responseIdReleaseH = new ResponseIdReleaseH();
            respCreateAcquisitionReleaseH.conflicts = new List<Conflicts>();

            if (respCreateAcquisitionReleaseH == null ||
                !DateTime.TryParse(acquisitionReleaseH.Date.ToString(), out _) ||
                string.IsNullOrWhiteSpace(acquisitionReleaseH.Type) ||
                string.IsNullOrWhiteSpace(acquisitionReleaseH.Elaborated) ||
                //string.IsNullOrWhiteSpace(acquisitionReleaseH.Observations) ||
                acquisitionReleaseH.IdAddressRelease == 0 || 
                acquisitionReleaseH.IdAddressEntry == 0 || 
                acquisitionReleaseH.IdAcCarrier == 0 ||
                acquisitionReleaseH.IdAcVehicle == 0 ||
                acquisitionReleaseH.IdDepartment == 0
                )
            {
                respCreateAcquisitionReleaseH.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionReleaseH);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionReleaseHRepository.InsertReleaseH(acquisitionReleaseH);

            if (created == 0)
            {
                respCreateAcquisitionReleaseH.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateAcquisitionReleaseH);
            }

            respCreateAcquisitionReleaseH.responseIdReleaseH.Id = created;
            respCreateAcquisitionReleaseH.conflicts.Add(new Conflicts { Problems = false, Description = "Salida Creada Correctamente" });

            return Created("created", respCreateAcquisitionReleaseH);
        }

        // PUT api/<AcquisitionReleaseH>/5
        [HttpPut]
        public async Task<IActionResult> AuthorizeReleaseH(int id, int status, int code)
        {
            Response respAuthorizeReleaseH = new Response();
            respAuthorizeReleaseH.conflicts = new List<Conflicts>();
            

            if (id == 0 || status == 0)
            {
                respAuthorizeReleaseH.conflicts.Add(new Conflicts { Problems = true, Description = "Ninguno de los campos puede ser 0" });
                return BadRequest(respAuthorizeReleaseH);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var codeCorrect = await _acquisitionReleaseHRepository.GetById(id);

            if (codeCorrect.ReceptionCode != code)
            {
                respAuthorizeReleaseH.conflicts.Add(new Conflicts { Problems = true, Description = "El código es incorrecto" });
                return BadRequest(respAuthorizeReleaseH);
            }

            var update = await _acquisitionReleaseHRepository.AuthorizeReleaseH(id, status);

            if (update == false)
            {
                respAuthorizeReleaseH.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Autorizada" });
                return BadRequest(respAuthorizeReleaseH);
            }

            respAuthorizeReleaseH.conflicts.Add(new Conflicts { Problems = false, Description = "Orden Autorizada Correctamente" });
            return Ok(respAuthorizeReleaseH);
        }

        //// DELETE api/<AcquisitionReleaseH>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
