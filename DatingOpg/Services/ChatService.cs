using DatingOpg.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DatingOpg.Models;

namespace DatingOpg.Services
{
    public class ChatService
    {
        private readonly DtingContext _context;

        public ChatService(DtingContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> GetChatMessagesAsync(int senderId, int receiverId)
        {
            return await _context.Chats
                .Where(c => (c.SenderId == senderId && c.ReceiverId == receiverId) ||
                            (c.SenderId == receiverId && c.ReceiverId == senderId))
                .OrderBy(c => c.ChatId)
                .ToListAsync();
        }

        public async Task AddChatMessageAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }
    }
}
