using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    class Program
    {
        private static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;
            const string defaultRoomsFilename = "Rooms.json";
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFilename] : defaultRoomsFilename);
            InitializeRooms(roomsFilename);

            Room previousRoom = null;

            while(command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);
                if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
                
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                switch(command)
                {
                    case Commands.LOOK:
                        Console.WriteLine(CurrentRoom.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (Move(command) == true)
                        {
                            Console.WriteLine($"You moved {command}.");
                        }
                        else
                        {
                            Console.WriteLine("The way is shut!");
                        }
                        break;

                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing the game!");
                        break;

                    default:
                        Console.WriteLine("Unrecognized command.");
                        break;
                }
            }
        }

        private static Commands ToCommand(string commandString) =>
            Enum.TryParse(commandString, true, out Commands result) ?result : Commands.UNKNOWN;

        private static bool Move(Commands movement)
        {
            Assert.IsTrue(IsDirection(movement), "Invalid Direction.");

            bool success = true;

            switch(movement)
            {
                case Commands.NORTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;

                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    break; 

                case Commands.EAST when Location.Column > 0:
                    Location.Column--;
                    break; 

                case Commands.WEST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;

                default:
                    success = false;
                    break;
            }

            return success;
        }

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static Room[,] Rooms;

        private enum Fields
        {
            Name = 0,
            Description
        }

        private enum CommandLineArguments
        {
            RoomsFilename = 0
        }

        private static void InitializeRooms(string roomsFilename) => Rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFilename));


        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private static (int Row, int Column) Location = (1, 1);
    }
}
