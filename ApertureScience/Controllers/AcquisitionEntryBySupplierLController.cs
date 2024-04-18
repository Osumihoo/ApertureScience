using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionEntryBySupplierLController : ControllerBase
    {
        private readonly IAcquisitionEntryBySupplierLRepository _acquisitionEntryBySupplierLRepository;

        public AcquisitionEntryBySupplierLController(IAcquisitionEntryBySupplierLRepository acquisitionEntryBySupplierLRepository)
        {
            _acquisitionEntryBySupplierLRepository = acquisitionEntryBySupplierLRepository;
        }

        // GET: api/<AcquisitionEntryBySupplierLController>
        [HttpGet]
        public async Task<IActionResult> GetAllEntriesBySupplierL()
        {
            return Ok(await _acquisitionEntryBySupplierLRepository.GetAllEntryBySupplierL());
        }

        // GET api/<AcquisitionEntryBySupplierLController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntryBySupplierL(int id)
        {
            return Ok(await _acquisitionEntryBySupplierLRepository.GetById(id));
        }

        [HttpGet("byheader")]
        public async Task<IActionResult> GetAllEntriesBySupplierLByHeader(int id)
        {
            return Ok(await _acquisitionEntryBySupplierLRepository.GetAllEntryBySupplierLByHeader(id));
        }


        // POST api/<AcquisitionEntryBySupplierLController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IEnumerable<AcquisitionEntryBySupplierL> acquisitionEntryBySupplierL)
        {
            Response respCreateAcquisitionEntryBySupplierL = new Response();
            respCreateAcquisitionEntryBySupplierL.conflicts = new List<Conflicts>();

            foreach (var entryL in acquisitionEntryBySupplierL)
            {
                if (entryL == null ||
                entryL.Quantity == 0 ||
                entryL.UnitPrice == 0 ||
                entryL.Total == 0 ||
                //entryL.SupplierGuarantee == 0 ||
                //entryL.BrandGuarantee == 0 ||
                string.IsNullOrWhiteSpace(entryL.InvoiceSheets) ||
                entryL.IdAcEntrySuppliersH == 0 ||
                entryL.IdAcSupplies == 0
                )
                {
                    respCreateAcquisitionEntryBySupplierL.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                    return BadRequest(respCreateAcquisitionEntryBySupplierL);

                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionEntryBySupplierLRepository.InsertEntryBySupplierL(acquisitionEntryBySupplierL);

            if (created == false)
            {
                respCreateAcquisitionEntryBySupplierL.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateAcquisitionEntryBySupplierL);
            }

            respCreateAcquisitionEntryBySupplierL.conflicts.Add(new Conflicts { Problems = false, Description = "Entrada Creada Correctamente" });

            return Created("created", respCreateAcquisitionEntryBySupplierL);
        }

        // PUT api/<AcquisitionEntryBySupplierLController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AcquisitionEntryBySupplierLController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
