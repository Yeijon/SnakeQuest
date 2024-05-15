using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeQuest.model
{
    public class DataItem
    {
       // public int Id { get; set; }
        public string Name { get; set; }
        public int Score {  get; set; }
        public DateTime Time { get; set; }
    }
}
