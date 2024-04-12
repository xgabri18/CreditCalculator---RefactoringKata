namespace CreditCalculator.After;

public class CustomerService
{
    public bool AddCustomer(
        string firstName,
        string lastName,
        string email,
        DateTime dateOfBirth,
        int companyId)
    {
        if (!CheckCredentials(firstName, lastName, email)) return false;

        var age = CalculateAge(dateOfBirth);
        
        if (age < 21)
        {
            return false;
        }

        var companyRepository = new CompanyRepository();
        var company = companyRepository.GetById(companyId);

        var customer = new Customer
        {
            Company = company,
            DateOfBirth = dateOfBirth,
            EmailAddress = email,
            FirstName = firstName,
            LastName = lastName
        };

        CreditCheck(customer, company);

        if (customer.HasCreditLimit && customer.CreditLimit < 500)
        {
            return false;
        }

        var customerRepository = new CustomerRepository();
        customerRepository.AddCustomer(customer);

        return true;
    }


    // Comment
    private bool CheckCredentials(string name, string surname, string email)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
        {
            return false;
        }

        if (!email.Contains('@') && !email.Contains('.'))
        {
            return false;
        }
        return true;
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var now = DateTime.Now;
        var age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month ||
            now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)
        {
            age--;
        }
        return age;
    }

    private void CreditCheck(Customer customer, Company company)
    {
        if (company.Type == "VeryImportantClient")
        {
            // Skip credit check
            customer.HasCreditLimit = false;
        }
        else if (company.Type == "ImportantClient")
        {
            // Do credit check and double credit limit
            customer.HasCreditLimit = true;
            var creditService = new CustomerCreditServiceClient();

            var creditLimit = creditService.GetCreditLimit(
                customer.FirstName,
                customer.LastName,
                customer.DateOfBirth);

            creditLimit *= 2;
            customer.CreditLimit = creditLimit;
        }
        else
        {
            // Do credit check
            customer.HasCreditLimit = true;
            var creditService = new CustomerCreditServiceClient();

            var creditLimit = creditService.GetCreditLimit(
                customer.FirstName,
                customer.LastName,
                customer.DateOfBirth);

            customer.CreditLimit = creditLimit;
        }
    }
}