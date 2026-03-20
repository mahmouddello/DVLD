using System;

namespace DVLD.EntityLayer
{
    public class Driver
    {
        public int Id { get; set; } = -1;
        public int PersonId { get; set; } = -1;
        public int CreatedByUserId { get; set; } = -1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsNew => Id == -1;

        // Navigation
        public Person PersonInfo { get; set; } = null;
        public User CreatorUserInfo { get; set; } = null;

        // default constructor
        public Driver(int personId, int createdByUserId)
        {
            PersonId = personId;
            CreatedByUserId = createdByUserId;
            CreatedAt = DateTime.Now;
        }

        // parametrized constructor
        public Driver(int id,  int personId, int createdByUserId, DateTime createdAt)
        {
            Id = id;
            PersonId = personId;
            CreatedByUserId = createdByUserId;
            CreatedAt = createdAt;
        }
    }
}
