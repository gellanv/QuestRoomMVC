using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestRoomMVC.Models
{
    public enum FearLevel
    {
        Нестрашный, Страшный
    }
    public enum DifficultLevel
    {
        Сложный, Средний, Легкий
    }
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 5)]
        [Required]
        public int CountPlayers { get; set; }
        [Required]
        public DifficultLevel DifficultLevel { get; set; }
        [Required]
        public FearLevel FearLevel { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
