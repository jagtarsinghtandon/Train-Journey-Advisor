using System;
using System.Collections.ObjectModel;

namespace Train
{
    public class Tickets
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Concession { get; set; }
        public string TicketType { get; set; } 
        public string Total { get; set; }
        public ObservableCollection<string> Path { get; set; }
        public DateTime Date { get; set; }
    }
}