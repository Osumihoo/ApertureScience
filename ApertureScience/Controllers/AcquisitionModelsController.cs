using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionModelsController : ControllerBase
    {
        private readonly IAcquisitionModelRepository _acquisitionModelRepository;

        public AcquisitionModelsController(IAcquisitionModelRepository acquisitionModelRepository)
        {
            _acquisitionModelRepository = acquisitionModelRepository;
        }
        // GET: api/<AcquisitionModelController>
        [HttpGet]
        public async Task<IActionResult> GetAllModels()
        {
            return Ok(await _acquisitionModelRepository.GetAllModels());
        }

        // GET api/<AcquisitionModelController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModel(int id)
        {
            return Ok(await _acquisitionModelRepository.GetById(id));
        }

        // POST api/<AcquisitionModelController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionModel acquisitionModel)
        {
            Response respCreateAcquisitionModel = new Response();
            respCreateAcquisitionModel.conflicts = new List<Conflicts>();

            if (acquisitionModel == null ||
                string.IsNullOrWhiteSpace(acquisitionModel.Name)
                )
            {
                respCreateAcquisitionModel.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionModel);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionModelRepository.InsertModel(acquisitionModel);

            if (created == false)
            {
                respCreateAcquisitionModel.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionModel);
            }

            respCreateAcquisitionModel.conflicts.Add(new Conflicts { Problems = false, Description = "Modelo Creado Correctamente" });

            return Created("created", respCreateAcquisitionModel);
        }

        // PUT api/<AcquisitionModelController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateModel([FromBody] AcquisitionModel acquisitionModel)
        {
            Response respUpdateAcquisitionModel = new Response();
            respUpdateAcquisitionModel.conflicts = new List<Conflicts>();

            if (acquisitionModel == null)
            {
                respUpdateAcquisitionModel.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionModel);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionModelRepository.UpdateModel(acquisitionModel);

            if (update == false)
            {
                respUpdateAcquisitionModel.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionModel);
            }

            respUpdateAcquisitionModel.conflicts.Add(new Conflicts { Problems = false, Description = "Modelo Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionModel);
        }

        // DELETE api/<AcquisitionModelController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            Response respDeleteAcquisitionModel = new Response();
            respDeleteAcquisitionModel.conflicts = new List<Conflicts>();

            var delete = await _acquisitionModelRepository.DeleteModel(new AcquisitionModel { Id = id });

            if (delete == false)
            {
                respDeleteAcquisitionModel.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAcquisitionModel);
            }
            respDeleteAcquisitionModel.conflicts.Add(new Conflicts { Problems = false, Description = "Modelo Eliminado Correctamente" });

            return Ok(respDeleteAcquisitionModel);
        }
    }
}
