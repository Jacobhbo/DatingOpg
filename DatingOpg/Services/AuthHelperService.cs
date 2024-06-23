using DatingOpg.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DatingOpg.Services
{
    public class AuthHelperService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly AccountService _accountService;

        public AuthHelperService(AuthenticationStateProvider authenticationStateProvider, AccountService accountService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _accountService = accountService;
        }

        public async Task<Account> GetAuthenticatedAccountAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                return await _accountService.GetAccountByUserNameAsync(user.Identity.Name);
            }

            return null;
        }
    }
}
