using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionEntryBySupplierHController : ControllerBase
    {
        private readonly IAcquisitionEntryBySupplierHRepository _acquisitionEntryBySupplierHRepository;

        public AcquisitionEntryBySupplierHController(IAcquisitionEntryBySupplierHRepository acquisitionEntryBySupplierHRepository)
        {
            _acquisitionEntryBySupplierHRepository = acquisitionEntryBySupplierHRepository;
        }

        // GET: api/<AcquisitionEntryBySupplierHController>
        [HttpGet]
        public async Task<IActionResult> GetAllEntryBySupplierH()
        {
            return Ok(await _acquisitionEntryBySupplierHRepository.GetAllEntryBySupplierH());
        }

        // GET api/<AcquisitionEntryBySupplierHController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntryBySupplierH(int id)
        {
            return Ok(await _acquisitionEntryBySupplierHRepository.GetById(id));
        }

        // GET api/<AcquisitionEntryBySupplierHController>/date
        [HttpGet("date")]
        public async Task<IActionResult> GetEntriesBySupplierHForDate(DateTime startDate, DateTime endDate)
        {
            return Ok(await _acquisitionEntryBySupplierHRepository.GetByDates(startDate, endDate));
        }

        // POST api/<AcquisitionEntryBySupplierHController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionEntryBySupplierH acquisitionEntryBySupplierH)
        {
            Response respCreateAcquisitionEntryBySupplierH = new Response();
            respCreateAcquisitionEntryBySupplierH.responseIdEntryH = new ResponseIdEntryH();
            respCreateAcquisitionEntryBySupplierH.conflicts = new List<Conflicts>();

            if (acquisitionEntryBySupplierH == null ||
                !DateTime.TryParse(acquisitionEntryBySupplierH.Date.ToString(), out _) ||
                string.IsNullOrWhiteSpace(acquisitionEntryBySupplierH.Type) ||
                string.IsNullOrWhiteSpace(acquisitionEntryBySupplierH.Elaborated) ||
                //string.IsNullOrWhiteSpace(acquisitionEntryBySupplierH.Observations) ||
                acquisitionEntryBySupplierH.IdSuppliers == 0
                )
            {
                respCreateAcquisitionEntryBySupplierH.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionEntryBySupplierH);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionEntryBySupplierHRepository.InsertEntryBySupplierH(acquisitionEntryBySupplierH);

            if (created == 0)
            {
                respCreateAcquisitionEntryBySupplierH.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateAcquisitionEntryBySupplierH);
            }

            respCreateAcquisitionEntryBySupplierH.responseIdEntryH.Id = created;
            respCreateAcquisitionEntryBySupplierH.conflicts.Add(new Conflicts { Problems = false, Description = "Entrada Creada Correctamente" });

            return Created("created", respCreateAcquisitionEntryBySupplierH);
        }

        // PUT api/<AcquisitionEntryBySupplierHController>/5
        //[HttpPut]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AcquisitionEntryBySupplierHController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
