using DatingOpg.Models;
using DatingOpg.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<Profile> GetProfileByIdAsync(int profileId)
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.ProfileId == profileId);
        }

        public async Task<List<Profile>> SearchProfilesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Profile>();
            }

            return await _context.Profiles
                .Where(p => p.NickName.Contains(searchTerm))
                .ToListAsync();
        }

        public List<Profile> SearchProfiles(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Profile>();
            }

            return _context.Profiles
                .Where(p => p.NickName.Contains(searchTerm))
                .ToList();
        }

        public async Task<List<Profile>> GetMutualLikesAsync(int accountId)
        {
            var likedProfiles = await _context.Likes
                .Where(l => l.SenderId == accountId && l.status == 1)
                .Select(l => l.ReceiverId)
                .ToListAsync();

            var mutualLikes = await _context.Likes
                .Where(l => likedProfiles.Contains(l.SenderId) && l.ReceiverId == accountId && l.status == 1)
                .Select(l => l.SenderId)
                .ToListAsync();

            return await _context.Profiles
                .Where(p => mutualLikes.Contains(p.AccountId))
                .ToListAsync();
        }

        public async Task<List<Profile>> GetAllProfilesAsync()
        {
            return await _context.Profiles.ToListAsync();
        }
    }
}
