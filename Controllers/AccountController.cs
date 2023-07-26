using Microsoft.AspNetCore.Mvc;
using BankAccountAPI.Models;

namespace API_BankAccount.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static List<Account> accounts = new List<Account>();

        [HttpGet]
        public ActionResult<IEnumerable<Account>> GetAccounts()
        {
            return accounts;
        }

        [HttpPost]
        public ActionResult<Account> CreateAccount([FromBody] Account account)
        {
            account.AccountNumber = Guid.NewGuid().ToString(); // Generating a unique account number
            account.OpeningDate = DateTime.Now;
            account.Balance = 0;
            account.TransferHistory = new List<string>();
            account.AccountStatus = "Active";

            accounts.Add(account);

            return CreatedAtAction(nameof(GetAccount), new { accountNumber = account.AccountNumber }, account);
        }

        [HttpGet("{accountNumber}")]
        public ActionResult<Account> GetAccount(string accountNumber)
        {
            var account = accounts.Find(c => c.AccountNumber == accountNumber);

            if (account == null)
                return NotFound();

            return account;
        }

        [HttpPost("{accountNumber}/withdraw")]
        public IActionResult Withdraw(string accountNumber, [FromBody] decimal amount)
        {
            var account = accounts.Find(c => c.AccountNumber == accountNumber);

            if (account == null)
                return NotFound();

            if (amount <= 0)
                return BadRequest("The withdrawal amount must be greater than zero.");

            if (account.Balance < amount)
                return BadRequest("Insufficient balance.");

            account.Balance -= amount;
            account.TransferHistory.Add($"Withdrawal: -{amount:C}");

            return NoContent();
        }

        [HttpPost("{accountNumber}/deposit")]
        public IActionResult Deposit(string accountNumber, [FromBody] decimal amount)
        {
            var account = accounts.Find(c => c.AccountNumber == accountNumber);

            if (account == null)
                return NotFound();

            if (amount <= 0)
                return BadRequest("The deposit amount must be greater than zero.");

            account.Balance += amount;
            account.TransferHistory.Add($"Deposit: +{amount:C}");

            return NoContent();
        }

        [HttpPost("{sourceAccountNumber}/transfer")]
        public IActionResult TransferTo(string sourceAccountNumber, [FromBody] Transfer transfer)
        {
            var sourceAccount = accounts.Find(c => c.AccountNumber == sourceAccountNumber);
            var destinationAccount = accounts.Find(c => c.AccountNumber == transfer.DestinationAccountNumber);

            if (sourceAccount == null || destinationAccount == null)
                return NotFound();

            if (sourceAccount.Balance < transfer.Amount)
                return BadRequest("Insufficient funds to perform the transfer.");

            sourceAccount.Balance -= transfer.Amount;
            sourceAccount.TransferHistory.Add($"Transfer to {destinationAccount.AccountNumber}: -{transfer.Amount:C}");

            destinationAccount.Balance += transfer.Amount;
            destinationAccount.TransferHistory.Add($"Transfer from {sourceAccount.AccountNumber}: +{transfer.Amount:C}");

            return NoContent();
        }

        public class Transfer
        {
            public string DestinationAccountNumber { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
