using DateTimeConversion.Models;

namespace DateTimeConversion
{
    internal class Core : ICore
    {
        private readonly AccountDbContext _dbContext;

        public Core(AccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RunTasksAsync()
        {
            //seed data
            try
            {
                await SeedData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            DateTime dateTime = Convert.ToDateTime("2022-07-07").ToUniversalTime().Date;

            GetData(dateTime);

            DateTime dateTime1 = DateTime.UtcNow.AddHours(1).Date;

            GetData(dateTime1);
        }

        private void GetData(DateTime transactionDate)
        {
            //get data using today as Date

            int numToSkip = 0;
            int pageSize = 10;

            var transactions = _dbContext.AccountTransactions
                .Where(transaction => transaction.TransactionDate.Date == transactionDate.Date)
                .OrderByDescending(transaction => transaction.TransactionDate)
                .Skip(numToSkip)
                .Take(pageSize);

            Console.WriteLine(transactions.Count());
        }

        private async Task SeedData()
        {
            if (_dbContext.AccountTransactions.Count() > 0)
            {
                return;
            }

            Guid accountId = Guid.NewGuid();

            List<AccountTransaction> accountTransactions = new List<AccountTransaction>
            {
                new AccountTransaction
                {
                    AccountId = accountId,
                    TransactionId  = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    TransactionDate = DateTime.UtcNow,
                    Amount = 1000,
                    Balance = 10000,
                    TransactionStatus = 1,
                    TransactionType = 1
                },
                new AccountTransaction
                {
                    AccountId = accountId,
                    TransactionId  = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    TransactionDate = DateTime.UtcNow,
                    Amount = 2000,
                    Balance = 13000,
                    TransactionStatus = 1,
                    TransactionType = 2
                },
                new AccountTransaction
                {
                    AccountId = accountId,
                    TransactionId  = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    TransactionDate = DateTime.UtcNow,
                    Amount = 3000,
                    Balance = 16000,
                    TransactionStatus = 2,
                    TransactionType = 1
                }
            };

            await _dbContext.AccountTransactions.AddRangeAsync(accountTransactions);

            var result = await _dbContext.SaveChangesAsync();

            Console.WriteLine($"{result} data seeded");
        }
    }
}