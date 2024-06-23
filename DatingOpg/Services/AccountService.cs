using DatingOpg.Models;
using DatingOpg.Data;
using Microsoft.EntityFrameworkCore;


namespace DatingOpg.Services
{
    public class AccountService
    {
        private readonly DtingContext _context;

        public AccountService(DtingContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByUserNameAsync(string userName)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.UserName == userName);
        }

    }
}
