using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roommates.Repositories;
using Roomates.Models;

namespace Roomates.Repositories
{
    class RoomateRepository : BaseRepository
    {
        public RoomateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Roommate.FirstName, Roommate.RentPortion, Room.Name FROM Roommate JOIN Room on Room.Id = Roommate.RoomId WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room()
                                {
                                   Name = reader.GetString(reader.GetOrdinal("Name"))
                                }
                            };
                        }

                        return roommate;
                    }
                }
            }
        }
    }
}
