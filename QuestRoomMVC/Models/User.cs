using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace QuestRoomMVC.Models
{
    public class User : IdentityUser
    {
        public ICollection<Order> Orders { get; set; }
    }
}
