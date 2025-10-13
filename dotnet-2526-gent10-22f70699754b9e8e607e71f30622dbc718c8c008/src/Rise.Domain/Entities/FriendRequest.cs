namespace Rise.Domain.Entities;
    public class FriendRequest
    {
        public int UserId1 { get; set; } // sender
        public int UserId2 { get; set; } // receiver
        public bool accepted { get; set; }
        public bool blocked { get; set; }
    }
