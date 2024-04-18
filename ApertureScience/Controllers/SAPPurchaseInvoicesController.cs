using ApertureScience.Data;
using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SAPPurchaseInvoicesController : ControllerBase
    {
        private readonly ISAPPurchaseInvoicesRepository _SAPSAPPurchaseInvoicesRepository;

        public SAPPurchaseInvoicesController(ISAPPurchaseInvoicesRepository sapPurchaseInvoicesRepository)
        {
            _SAPSAPPurchaseInvoicesRepository = sapPurchaseInvoicesRepository;
        }

        // POST api/<SAPPurchaseInvoicesController>
        [HttpPost]
        public async Task<IActionResult> SAPPurchaseInvoices([FromBody] SAPPurchaseInvoices sapPurchaseInvoices)
        {
            Response respCreateSAPPurchaseInvoices = new Response();
            respCreateSAPPurchaseInvoices.conflicts = new List<Conflicts>();

            if (sapPurchaseInvoices == null ||
                string.IsNullOrWhiteSpace(sapPurchaseInvoices.CardCode) ||
                sapPurchaseInvoices.DocumentLines == null )
            {
                respCreateSAPPurchaseInvoices.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateSAPPurchaseInvoices);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string coockie = await SapLogin();
            var created = await _SAPSAPPurchaseInvoicesRepository.CreatePurchaseInvoices(sapPurchaseInvoices, coockie);

            if (created.conflicts != null && created.conflicts.Any(c => c.Problems == true))
            {
                var conflictoConProblemas = created.conflicts.FirstOrDefault(c => c.Problems);
                respCreateSAPPurchaseInvoices.conflicts.Add(new Conflicts { Problems = true, Description = conflictoConProblemas.Description });

                return BadRequest(respCreateSAPPurchaseInvoices);
            }

            respCreateSAPPurchaseInvoices.conflicts.Add(new Conflicts { Problems = false, Description = "Factura Creada Correctamente" });

            return Created("created", respCreateSAPPurchaseInvoices);
        }

        private async Task<string> SapLogin()
        {
            // Crear una instancia de la clase SAPLogin
            var sapLoginInstance = new SAPLogin();

            // Llamar al método SAPLog en la instancia creada
            var cookie = await sapLoginInstance.SAPLog();

            return cookie;
        }

    }
}
