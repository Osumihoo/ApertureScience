using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionSuppliesController : ControllerBase
    {
        private readonly IAcquisitionSupplyRepository _supplyRepository;

        public AcquisitionSuppliesController(IAcquisitionSupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }
        // GET: api/<SuppliesController>
        [HttpGet]
        public async Task<IActionResult> GetAllSupplies()
        {
            return Ok(await _supplyRepository.GetAllSupplies());
        }

        // GET api/<SuppliesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupply(int id)
        {
            return Ok(await _supplyRepository.GetById(id));
        }

        // GET api/<SuppliesController>/latest
        [HttpGet("latest")]
        public async Task<IActionResult> GetTheLast()
        {
            return Ok(await _supplyRepository.GetTheLast());
        }

        // POST api/<SuppliesController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionSupply supply)
        {
            Response respCreateSupply = new Response();
            respCreateSupply.conflicts = new List<Conflicts>();

            if (supply == null ||
                string.IsNullOrWhiteSpace(supply.Code) ||
                supply.Quantity == 0 ||
                supply.UnitCost == 0 ||
                supply.IdAcProduct == 0 ||
                supply.IdAcBrand == 0 ||
                supply.IdAcModel == 0 ||
                supply.IdAcMeasurement == 0 ||
                supply.IdAcCategory == 0
                )
            {
                respCreateSupply.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateSupply);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _supplyRepository.InsertSupply(supply);

            if (created == false)
            {
                respCreateSupply.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateSupply);
            }

            respCreateSupply.conflicts.Add(new Conflicts { Problems = false, Description = "Consumible Creado Correctamente" });

            return Created("created", respCreateSupply);
        }

        // PUT api/<SuppliesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateSupply([FromBody] AcquisitionSupply supply)
        {
            Response respUpdateSupply = new Response();
            respUpdateSupply.conflicts = new List<Conflicts>();

            if (supply == null)
            {
                respUpdateSupply.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateSupply);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _supplyRepository.UpdateSupply(supply);

            if (update == false)
            {
                respUpdateSupply.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateSupply);
            }

            respUpdateSupply.conflicts.Add(new Conflicts { Problems = false, Description = "Consumible Actualizado Correctamente" });
            return Ok(respUpdateSupply);
        }

        [HttpPut("out")]
        public async Task<IActionResult> OutSupply([FromBody] IEnumerable<AcquisitionSupplyNewQuantity> acquisitionSupplyNewQuantity)
        {
            Response respUpdateOutSupply = new Response();
            respUpdateOutSupply.conflicts = new List<Conflicts>();

            foreach (var newQuantity in acquisitionSupplyNewQuantity)
            {
                if (newQuantity == null ||
                    newQuantity.SupplyId == 0 ||
                    newQuantity.NewQuantity == 0 
                )
                {
                    respUpdateOutSupply.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un producto para Actualizar" });
                    return BadRequest(respUpdateOutSupply);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _supplyRepository.OutSupply(acquisitionSupplyNewQuantity);

            if (update == false)
            {
                respUpdateOutSupply.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateOutSupply);
            }

            respUpdateOutSupply.conflicts.Add(new Conflicts { Problems = false, Description = "Consumible Actualizado Correctamente" });
            return Ok(respUpdateOutSupply);
        }

        [HttpPut("in")]
        public async Task<IActionResult> InSupply([FromBody] IEnumerable<AcquisitionSupplyNewQuantity> acquisitionSupplyNewQuantity)
        {
            Response respUpdateOutSupply = new Response();
            respUpdateOutSupply.conflicts = new List<Conflicts>();

            foreach (var newQuantity in acquisitionSupplyNewQuantity)
            {
                if (newQuantity == null ||
                    newQuantity.SupplyId == 0 
                    //newQuantity.NewQuantity == 0
                )
                {
                    respUpdateOutSupply.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un producto para Actualizar" });
                    return BadRequest(respUpdateOutSupply);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _supplyRepository.InSupply(acquisitionSupplyNewQuantity);

            if (update == false)
            {
                respUpdateOutSupply.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateOutSupply);
            }

            respUpdateOutSupply.conflicts.Add(new Conflicts { Problems = false, Description = "Consumible Actualizado Correctamente" });
            return Ok(respUpdateOutSupply);
        }


        // DELETE api/<SuppliesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupply(int id)
        {
            Response respDeleteSupply = new Response();
            respDeleteSupply.conflicts = new List<Conflicts>();

            var delete = await _supplyRepository.DeleteSupply(new AcquisitionSupply { Id = id });

            if (delete == false)
            {
                respDeleteSupply.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteSupply);
            }
            respDeleteSupply.conflicts.Add(new Conflicts { Problems = false, Description = "Consumible Eliminado Correctamente" });

            return Ok(respDeleteSupply);
        }
    }
}
