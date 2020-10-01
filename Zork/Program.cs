using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Zork
{
    class Program
    {
        private static string CurrentRoom
        {
            get
            {
                return Rooms[Location];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;

            while(command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                //string outputString;
                switch(command)
                {
                    case Commands.LOOK:
                        Console.WriteLine("This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door.");
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

                //Console.WriteLine(outputString);
            }

            //1.2
            /*string inputString = Console.ReadLine();
            Commands command = ToCommand(inputString.Trim().ToUpper());
            Console.WriteLine(command);*/

            //1.1
            /*inputString = inputString.ToUpper();

            if(inputString == "QUIT")
            {
                Console.WriteLine("Thank you for playing.");
            }
            else if(inputString == "LOOK")
            {
                Console.WriteLine("This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork! lies by the door.");
            }
            else
            {
                Console.WriteLine("Unrecognized command.");
            }*/
        }

        private static Commands ToCommand(string commandString)
        {
            if(Enum.TryParse<Commands>(commandString, true, out Commands result))
            {
                return result;
            }
            else
            {
                return Commands.UNKNOWN;
            }
        }

        private static bool Move(Commands movement)
        {
            Assert.IsTrue(IsDirection(movement), "Invalid Direction.");

            bool success = true;

            switch(movement)
            {
                case Commands.NORTH:
                    success = false;
                    break;

                case Commands.SOUTH:
                    success = false;
                    break; 

                case Commands.EAST when Location < Rooms.GetLength(0) - 1:
                    Location++;
                    break; 

                case Commands.WEST when Location > 0:
                    Location--;
                    break;

                default:
                    success = false;
                    break;
            }

            return success;
        }

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static readonly string[] Rooms =
        {
            "Forest", "West of House", "Behind House", "Clearing", "Canyon View"
        };

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private static int Location = 1;
    }
}
