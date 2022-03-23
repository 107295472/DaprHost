using System;

namespace DomainBase
{
    public interface IEntityUpdate
    {
        long? ModifiedUserId { get; set; }
        string ModifiedUserName { get; set; }
        DateTime? ModifiedTime { get; set; }
    }
}