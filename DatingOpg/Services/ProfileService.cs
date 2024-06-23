using DatingOpg.Models;
using DatingOpg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DatingOpg.Services
{
    public class ProfileService
    {
        private readonly DtingContext _context;
        private readonly AuthHelperService _authHelperService;


        public ProfileService(DtingContext context, AuthHelperService authHelperService)
        {
            _context = context;
            _authHelperService = authHelperService;
        }

        public async Task<Profile> GetProfileByAccountIdAsync(int accountId)
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.AccountId == accountId);
        }
    }
}
