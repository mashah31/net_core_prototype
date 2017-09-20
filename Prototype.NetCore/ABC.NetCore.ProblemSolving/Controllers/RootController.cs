using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ABC.NetCore.Models;
using ABC.NetCore.ProblemSolving.Models;

namespace ABC.NetCore.ProblemSolving.Controllers
{
    [Route("/api")]
    public class RootController: Controller
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new Root
            {
                Self = Link.To(nameof(GetRoot)),
                Customers = Link.To(nameof(CustomersController.GetCustomersAsync)),
                SAPParts = Link.To(nameof(SAPController.GetSAPPartsAsync)),
                SAPEmployees = Link.To(nameof(SAPController.GetSAPEmployeesAsync))
            };
            return Ok(response);
        }
    }
}
