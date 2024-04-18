using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionReleaseLController : ControllerBase
    {
        private readonly IAcquisitionReleaseLRepository _acquisitionReleaseLRepository;

        public AcquisitionReleaseLController(IAcquisitionReleaseLRepository acquisitionReleaseLRepository)
        {
            _acquisitionReleaseLRepository = acquisitionReleaseLRepository;
        }
        // GET: api/<AcquisitionReleaseLController>
        [HttpGet]
        public async Task<IActionResult> GetAllReleaseL()
        {
            return Ok(await _acquisitionReleaseLRepository.GetAllReleaseL());
        }

        // GET api/<AcquisitionReleaseLController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReleaseL(int id)
        {
            return Ok(await _acquisitionReleaseLRepository.GetById(id));
        }

        [HttpGet("byheader")]
        public async Task<IActionResult> GetAllReleaseLByHeader(int id)
        {
            return Ok(await _acquisitionReleaseLRepository.GetAllReleaseLByHeader(id));
        }

        // POST api/<AcquisitionReleaseLController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IEnumerable<AcquisitionReleaseL> acquisitionReleaseList)
        {
            Response respCreateAcquisitionReleaseL = new Response();
            respCreateAcquisitionReleaseL.conflicts = new List<Conflicts>();

            foreach (var releaseL in acquisitionReleaseList)
            {
                if (releaseL == null ||
                    releaseL.Quantity == 0 ||
                    releaseL.UnitPrice == 0 ||
                    releaseL.Total == 0 ||
                    releaseL.IdAcReleaseH == 0 ||
                    releaseL.IdAcSupplies == 0
                )
                {
                    respCreateAcquisitionReleaseL.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                    return BadRequest(respCreateAcquisitionReleaseL);

                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionReleaseLRepository.InsertReleaseL(acquisitionReleaseList);

            if (created == false)
            {
                respCreateAcquisitionReleaseL.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateAcquisitionReleaseL);
            }

            respCreateAcquisitionReleaseL.conflicts.Add(new Conflicts { Problems = false, Description = "Salida Creada Correctamente" });

            return Created("created", respCreateAcquisitionReleaseL);
        }

        // PUT api/<AcquisitionReleaseLController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AcquisitionReleaseLController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
