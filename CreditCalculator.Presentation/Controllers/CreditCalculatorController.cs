using CreditCalculator.Before;
using Microsoft.AspNetCore.Mvc;

namespace CreditCalculator.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditCalculatorController : ControllerBase
    {

        [HttpPost]
        public bool CreateCustomer(CustomerInput input)
        {
            var customerService = new CustomerService();

            return customerService.AddCustomer(input.FirstName, input.LastName, input.Email, input.DateOfBirth, input.CompanyId);

        }

        public record CustomerInput(string FirstName, string LastName, string Email, DateTime DateOfBirth, int CompanyId);

    }
}
