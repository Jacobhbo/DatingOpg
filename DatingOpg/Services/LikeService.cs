using DatingOpg.Models;
using DatingOpg.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingOpg.Services
{
    public class LikeService
    {
        private readonly DtingContext _context;

        public LikeService(DtingContext context)
        {
            _context = context;
        }

        public async Task AddLikeAsync(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            // Check for mutual like
            var mutualLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.SenderId == like.ReceiverId && l.ReceiverId == like.SenderId && l.status == 1);

            if (mutualLike != null)
            {
                // Create a chat if mutual like exists
                var chat = new Chat
                {
                    SenderId = like.SenderId,
                    ReceiverId = like.ReceiverId,
                    Message = "You have a new match!",
                    Status = 0 // Assuming 0 means "unread"
                };
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckForMutualLikeAsync(int senderId, int receiverId)
        {
            var mutualLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.SenderId == receiverId && l.ReceiverId == senderId && l.status == 1);

            return mutualLike != null;
        }
    }
}
