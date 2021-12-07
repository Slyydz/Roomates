using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roomates.Models
{
    public class Roommate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RentPortion { get; set; }
        public DateTime MovedInDate { get; set; }
        public Room Room { get; set; }

        public string Details
        {
            get
            {
                return $"{FirstName} {LastName} has a rent of {RentPortion} and their move in date is {MovedInDate}";
            }
        }

        public Roommate(string firstName, string lastName, int rentPortion, DateTime movedInDate, int roomId)
        {
            FirstName = firstName;
            LastName = lastName;
            RentPortion = rentPortion;
            MovedInDate = movedInDate;
            Room = new Room {
                Id = roomId
            };
        }

        public Roommate()
        {

        }
    }

    
}
