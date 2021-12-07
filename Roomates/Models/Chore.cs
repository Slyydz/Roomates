using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roomates.Models;

namespace Roomates.Models
{
    public class Chore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int roommateChoreId {get; set;}
    }
}
