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

        public List<Roommate> GetAll()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Roommate";

                    List<Roommate> roommates = new List<Roommate>();

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           Roommate roomate = new Roommate
                           {
                               Id = reader.GetInt32(reader.GetOrdinal("Id")),
                               FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                               LastName = reader.GetString(reader.GetOrdinal("LastName"))
                           };

                        roommates.Add(roomate);
                        }

                        return roommates;
                    }

                    
                }
            }
        }

        public Roommate getAssignedRoommate(int choreId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Roommate.FirstName, Roommate.LastName, Roommate.Id FROM Roommate JOIN RoommateChore on RoommateChore.RoommateId = Roommate.Id WHERE RoommateChore.ChoreId = @choreId";

                    cmd.Parameters.AddWithValue("@choreId", choreId);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate assignedRoommate = null;

                        while (reader.Read())
                        {
                            assignedRoommate = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            };
                        }

                        return assignedRoommate;
                    }
                }
            }
        }
    }
}
