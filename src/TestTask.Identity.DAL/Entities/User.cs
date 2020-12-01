using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TestTask.Identity.DAL.Entities
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
