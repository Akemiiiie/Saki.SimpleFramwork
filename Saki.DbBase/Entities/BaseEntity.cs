using System;
using System.Security.Principal;

namespace Saki.IDbBase.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public void MarkDeleted() => IsDeleted = true;
    }
}
