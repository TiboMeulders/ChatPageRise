using Rise.Persistence.Repositories;
using Rise.Shared.DTOs;
using Rise.Domain.Entities;

namespace Rise.Services;

// public class FriendService
// {
//     
// }

public interface IFriendService
    {
        Task<IEnumerable<FriendDto>> GetFriendsAsync(int userId);
        Task<object> SendRequestAsync(int currentUserId, int toUserId);
        Task<object> AcceptRequestAsync(int currentUserId, int fromUserId);
        Task<FriendCheckDto> CheckFriendStatusAsync(int currentUserId, int targetUserId);
    }

    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _repo;

        public FriendService(IFriendRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<FriendDto>> GetFriendsAsync(int userId)
        {
            var friends = await _repo.GetFriendsAsync(userId);

            return friends.Select(f => new FriendDto
            {
                Id = f.UserId1 == userId ? f.UserId2 : f.UserId1,
                Name = null, // Optional: join with User table
                Email = null,
                Accepted = f.accepted
            });
        }

        public async Task<object> SendRequestAsync(int currentUserId, int toUserId)
        {
            var existing = await _repo.GetFriendshipAsync(currentUserId, toUserId);
            if (existing != null)
                return new { success = false, message = "Friend request already exists." };

            var request = new FriendRequest
            {
                UserId1 = currentUserId,
                UserId2 = toUserId,
                accepted = false,
                blocked = false
            };

            await _repo.AddRequestAsync(request);
            await _repo.SaveChangesAsync();

            return new { success = true, message = "Friend request sent." };
        }

        public async Task<object> AcceptRequestAsync(int currentUserId, int fromUserId)
        {
            var request = await _repo.GetFriendshipAsync(currentUserId, fromUserId);
            if (request == null)
                return new { success = false, message = "Friend request not found." };

            request.accepted = true;
            await _repo.SaveChangesAsync();

            return new { success = true, message = "Friend request accepted." };
        }

        public async Task<FriendCheckDto> CheckFriendStatusAsync(int currentUserId, int targetUserId)
        {
            var friendship = await _repo.GetFriendshipAsync(currentUserId, targetUserId);
            if (friendship == null)
                return new FriendCheckDto { IsFriend = false, Accepted = false, Blocked = false };

            return new FriendCheckDto
            {
                IsFriend = true,
                Accepted = friendship.accepted,
                Blocked = friendship.blocked
            };
        }
    }