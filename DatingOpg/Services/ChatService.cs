using DatingOpg.Data;
using DatingOpg.Models;
using DatingOpg.Services;
using Microsoft.EntityFrameworkCore;

public class ChatService
{
    private readonly DtingContext _context;
    private readonly AuthHelperService _authHelperService;

    public ChatService(DtingContext context, AuthHelperService authHelperService)
    {
        _context = context;
        _authHelperService = authHelperService;
    }

    public async Task SendMessageAsync(int receiverId, string message)
    {
        var account = await _authHelperService.GetAuthenticatedAccountAsync();
        if (account == null) return;

        var chat = new Chat
        {
            SenderId = account.AccountId,
            ReceiverId = receiverId,
            Message = message,
            Status = 1
        };

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Chat>> GetMessagesAsync(int receiverId)
    {
        var account = await _authHelperService.GetAuthenticatedAccountAsync();
        if (account == null) return new List<Chat>();

        return await _context.Chats
            .Where(c => (c.SenderId == account.AccountId && c.ReceiverId == receiverId) ||
                        (c.ReceiverId == account.AccountId && c.SenderId == receiverId))
            .OrderBy(c => c.ChatId)
            .ToListAsync();
    }
}
