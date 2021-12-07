using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roomates.Models;
using Roomates.Repositories;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoomateRepository roomateRepo = new RoomateRepository(CONNECTION_STRING);

            DateTime date = new DateTime(2015, 12, 25);

            Roommate sprintRoommate1 = new Roommate("Wade","Watts", 15, date, 3);
            Roommate sprintRoommate2 = new Roommate
            {
        
                    FirstName = "Peter",
                    LastName = "Parker",
                    RentPortion = 20,
                    MovedInDate = date,
                    Room = new Room()
                    {
                        Id = 4

                    }
            };

            Console.WriteLine($"First Roommate: {sprintRoommate1.FirstName} {sprintRoommate1.LastName}");
            Console.WriteLine($"Second Roommate: {sprintRoommate2.FirstName} {sprintRoommate2.LastName}");

            Console.WriteLine();

            Console.WriteLine($"First Roommate Details: {sprintRoommate1.Details}");
            Console.WriteLine($"Second Roommate Details: {sprintRoommate2.Details}");
            Console.ReadLine();

            bool runProgram = true;

            while (runProgram)
            {
                
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);
                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "Show all chores":
                        List<Chore> chores = choreRepo.GetAll();

                        foreach(Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case "Search for chore":
                        int choreId = int.Parse(Console.ReadLine());
                        
                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name} ");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case "Add a chore":
                        Console.Write("Enter chore name: ");
                        string choreName = Console.ReadLine();

                        Chore newChore = new Chore
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(newChore);

                        Console.WriteLine($"{newChore.Name} has been added and assigned an Id of {newChore.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "Select a roommate":
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate newRoommate = roomateRepo.GetById(roommateId);

                        Console.WriteLine($"{newRoommate.Id} - {newRoommate.FirstName} is in the {newRoommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case "Show unassigned chores":
                        List<Chore> getUnassigned = choreRepo.GetUnassignedChores();

                        foreach(Chore c in getUnassigned)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name} is unassigned");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case "Assign chore to roommate":
                        List<Chore> showUnassigned = choreRepo.GetUnassignedChores();

                        foreach (Chore c in showUnassigned)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name} is unassigned");
                        }
                        Console.Write("Which chore would you like to select? ");
                        int assignChoreId = int.Parse(Console.ReadLine());

                        List<Roommate> showRoommates = roomateRepo.GetAll();

                        foreach(Roommate r in showRoommates)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} {r.LastName}");
                        }

                        Console.Write("Which roommate would you like to have do that chore? ");
                        int assignRoommateId = int.Parse(Console.ReadLine());

                        choreRepo.AssignChore(assignRoommateId, assignChoreId);

                        Console.WriteLine();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Process Complete!");
                        Console.WriteLine("---------------");
                        Console.WriteLine();
                        Console.Write("Press any key to continue.");
                        Console.ReadLine();

                        break;
                    case "Update a room":
                        List<Room> roomList = roomRepo.GetAll();

                        foreach(Room r in roomList)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }

                        Console.Write("Select a room to update: ");
                        int updateChoice = int.Parse(Console.ReadLine());

                        Console.WriteLine();
                        Room selectedRoom = roomList.FirstOrDefault(r => r.Id == updateChoice);

                        Console.Write("Enter a new name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.WriteLine();

                        Console.Write("Enter a new maximum occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room updated!");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();

                        break;
                    case "Delete a Room":
                        List<Room> roomListDel = roomRepo.GetAll();

                        foreach (Room r in roomListDel)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }

                        Console.Write("Which room do you want to delete? ");
                        int delId = int.Parse(Console.ReadLine());

                        roomRepo.Delete(delId);

                        Console.WriteLine();
                        Console.WriteLine("Room succesfully deleted");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();

                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Select a roommate",
                "Show unassigned chores",
                "Assign chore to roommate",
                "Update a Room",
                "Delete a Room",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }

    }
}

