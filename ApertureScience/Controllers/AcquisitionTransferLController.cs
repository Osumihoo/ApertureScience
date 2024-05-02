using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionTransferLController : ControllerBase
    {
        private readonly IAcquisitionTransferLRepository _acquisitionTransferLRepository;

        public AcquisitionTransferLController(IAcquisitionTransferLRepository acquisitionTransferLRepository)
        {
            _acquisitionTransferLRepository = acquisitionTransferLRepository;
        }

        // GET: api/<AcquisitionTransferLController>
        [HttpGet]
        public async Task<IActionResult> GetAllTransferL()
        {
            return Ok(await _acquisitionTransferLRepository.GetAllTransferL());
        }

        // GET api/<AcquisitionTransferLController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransferL(int id)
        {
            return Ok(await _acquisitionTransferLRepository.GetById(id));
        }

        [HttpGet("byheader")]
        public async Task<IActionResult> GetAllTransferLByHeader(int id)
        {
            return Ok(await _acquisitionTransferLRepository.GetTransferLByHeader(id));
        }

        // POST api/<AcquisitionTransferLController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IEnumerable<AcquisitionTransferL> acquisitionTransferL)
        {
            Response respCreateAcquisitionTransferL = new Response();
            respCreateAcquisitionTransferL.conflicts = new List<Conflicts>();

            foreach (var transferL in acquisitionTransferL)
            {
                if (transferL == null ||
                    string.IsNullOrWhiteSpace(transferL.Code) ||
                    string.IsNullOrWhiteSpace(transferL.Description) ||
                    transferL.Quantity == 0 ||
                    transferL.IdAcTransferH == 0 ||
                    transferL.IdAcSupplies == 0
                )
                {
                    respCreateAcquisitionTransferL.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                    return BadRequest(respCreateAcquisitionTransferL);

                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionTransferLRepository.InsertTransferL(acquisitionTransferL);

            if (created == false)
            {
                respCreateAcquisitionTransferL.conflicts.Add(new Conflicts { Problems = true, Description = "No se pudo insertar" });

                return BadRequest(respCreateAcquisitionTransferL);
            }

            respCreateAcquisitionTransferL.conflicts.Add(new Conflicts { Problems = false, Description = "Transferencia Creada Correctamente" });

            return Created("created", respCreateAcquisitionTransferL);
        }
    }
}
