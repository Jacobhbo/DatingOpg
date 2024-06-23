using DatingOpg.Models;
using DatingOpg.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DatingOpg.Services
{
    public class ProfileService
    {
        private readonly DtingContext _context;
        private readonly AuthHelperService _authHelperService;

        public event Action OnLikeReceived;
        public event Action OnMatch;

        public ProfileService(DtingContext context, AuthHelperService authHelperService)
        {
            _context = context;
            _authHelperService = authHelperService;
        }

        public async Task<Profile> GetProfileByAccountIdAsync(int accountId)
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.AccountId == accountId);
        }


        public async Task<List<Profile>> SearchProfilesAsync(SearchCriteria criteria)
        {
            var query = _context.Profiles.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.NickName))
            {
                query = query.Where(p => p.NickName.Contains(criteria.NickName));
            }
            if (!string.IsNullOrEmpty(criteria.Gender))
            {
                query = query.Where(p => p.Gender == criteria.Gender);
            }
            if (criteria.MinAge.HasValue)
            {
                var minBirthDate = DateTime.Now.AddYears(-criteria.MinAge.Value);
                query = query.Where(p => p.BirthDate <= minBirthDate);
            }
            if (criteria.MaxAge.HasValue)
            {
                var maxBirthDate = DateTime.Now.AddYears(-criteria.MaxAge.Value);
                query = query.Where(p => p.BirthDate >= maxBirthDate);
            }

            return await query.ToListAsync();
        }

        public async Task LikeProfileAsync(int profileId)
        {
            var account = await _authHelperService.GetAuthenticatedAccountAsync();
            if (account == null) return;

            var like = new Like
            {
                SenderId = account.AccountId,
                ReceiverId = profileId,
                status = 1
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            OnLikeReceived?.Invoke();

            var receiverLikesSender = await _context.Likes.AnyAsync(l => l.SenderId == profileId && l.ReceiverId == account.Profile.ProfileId && l.status == 1);
            if (receiverLikesSender)
            {
                OnMatch?.Invoke();
            }
        }

        public async Task<List<Like>> GetLikesReceivedAsync()
        {
            var account = await _authHelperService.GetAuthenticatedAccountAsync();
            if (account == null) return new List<Like>();

            return await _context.Likes
                .Include(l => l.Sender)
                .Where(l => l.ReceiverId == account.Profile.ProfileId)
                .ToListAsync();
        }

        public async Task<List<Like>> GetMatchesAsync()
        {
            var account = await _authHelperService.GetAuthenticatedAccountAsync();
            if (account == null) return new List<Like>();

            var receivedLikes = await _context.Likes
                .Include(l => l.Sender)
                .Where(l => l.ReceiverId == account.Profile.ProfileId && l.status == 1)
                .ToListAsync();

            var sentLikes = await _context.Likes
                .Include(l => l.Receiver)
                .Where(l => l.SenderId == account.Profile.ProfileId && l.status == 1)
                .ToListAsync();

            var matches = receivedLikes
                .Where(received => sentLikes.Any(sent => sent.ReceiverId == received.Sender.Profile.ProfileId))
                .ToList();

            return matches;
        }
    }
}
