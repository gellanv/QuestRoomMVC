using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestRoomMVC.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number 0501234567")]
        public string Phone { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
