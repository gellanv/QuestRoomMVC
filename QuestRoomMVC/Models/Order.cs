using System;
using System.ComponentModel.DataAnnotations;

namespace QuestRoomMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOrder { get; set; }
        [Required]
        public string TimeOrder { get; set; }

        public string UserId { get; set; }
        public int RoomId { get; set; }
       
        public User _user { get; set; }
        public Room Room { get; set; }
    }
}
