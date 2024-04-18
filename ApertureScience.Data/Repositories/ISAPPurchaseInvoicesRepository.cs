using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface ISAPPurchaseInvoicesRepository
    {
        Task<Response> CreatePurchaseInvoices(SAPPurchaseInvoices sapPurchaseInvoices, string hola);
    }
}
