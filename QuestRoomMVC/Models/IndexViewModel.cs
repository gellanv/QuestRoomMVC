using System.Collections.Generic;

namespace QuestRoomMVC.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
