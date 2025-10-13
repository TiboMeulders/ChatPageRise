using Microsoft.EntityFrameworkCore;
using Rise.Domain.Entities;

namespace Rise.Persistence.Repositories;

public interface IFriendRepository
{
    Task<List<FriendRequest>> GetFriendsAsync(int userId);
    Task<FriendRequest?> GetFriendshipAsync(int userId1, int userId2);
    Task AddRequestAsync(FriendRequest request);
    Task SaveChangesAsync();
}

public class FriendRepository : IFriendRepository
{
    private readonly ApplicationDbContext _context;

    public FriendRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FriendRequest>> GetFriendsAsync(int userId)
    {
        return await _context.FriendRequests
            .Where(f => (f.UserId1 == userId || f.UserId2 == userId) && f.accepted)
            .ToListAsync();
    }

    public async Task<FriendRequest?> GetFriendshipAsync(int userId1, int userId2)
    {
        return await _context.FriendRequests
            .FirstOrDefaultAsync(f =>
                (f.UserId1 == userId1 && f.UserId2 == userId2) ||
                (f.UserId1 == userId2 && f.UserId2 == userId1));
    }

    public async Task AddRequestAsync(FriendRequest request)
    {
        await _context.FriendRequests.AddAsync(request);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}