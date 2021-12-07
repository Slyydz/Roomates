using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roomates.Models;
using System.Collections.Generic;
using Roommates.Repositories;

namespace Roomates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAll()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    List<Chore> chores = new List<Chore>();

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idColumnPos = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPos);

                            int nameColumnPos = reader.GetOrdinal("Name");

                            string nameValue = reader.GetString(nameColumnPos);


                            Chore room = new Chore()
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(room);
                        }
                    return chores;
                    }
                }
            }
        }

        public Chore GetById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id",id);

                    Chore chore = null;

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                        }
                    }
                    return chore;
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                        cmd.CommandText = "SELECT * from Chore LEFT JOIN RoommateChore on RoommateChore.ChoreId = Chore.Id where RoommateChore.RoommateId IS NULL";
                        
                    List<Chore> unassigned = new List<Chore>();

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore unChore = new Chore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };

                            unassigned.Add(unChore);
                        }

                        return unassigned;
                    }
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@roommateId, @choreId)";
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    
                    cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Chore chore)
        {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Chore
                                    SET Name = @name
                                    WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@name", chore.Name);
                        cmd.Parameters.AddWithValue("@id", chore.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

        }

        public void Delete(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Chore WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }            
            }
        }
    }
}
