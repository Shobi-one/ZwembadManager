using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ZwembaadManager.Classes;

namespace ZwembaadManager
{
    internal class Program
    {
        static void Main()
        {
            Debug.WriteLine("=== Loading JSON Data ===\n");

            var addresses = Load<List<Address>>("Addresses.json");
            var clubs = Load<List<Club>>("Clubs.json");
            var functions = Load<List<Function>>("Functions.json");
            var jurysMembers = Load<List<JurysMember>>("JurysMembers.json");
            var meets = Load<List<Meet>>("Meets.json");
            var meetFunctions = Load<List<MeetFunction>>("MeetFunctions.json");
            var swimmingPools = Load<List<SwimmingPool>>("SwimmingPools.json");
            var users = Load<List<User>>("Users.json");

            Print("Addresses", addresses);
            Print("Clubs", clubs);
            Print("Functions", functions);
            Print("JurysMembers", jurysMembers);
            Print("Meets", meets);
            Print("MeetFunctions", meetFunctions);
            Print("SwimmingPools", swimmingPools);
            Print("Users", users);

            Debug.WriteLine("\n✓ Finished Loading");
        }

        private static T Load<T>(string file)
        {
            try
            {
                return IO.ReadJsonResource<T>(file);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading {file}: {ex.Message}");
                return default!;
            }
        }

        private static void Print<T>(string name, IList<T> list)
        {
            Debug.WriteLine($"\n=== {name} ({list?.Count ?? 0}) ===");

            if (list == null)
            {
                Debug.WriteLine("No data!");
                return;
            }

            foreach (var item in list.Take(3)) // preview first 3
                Debug.WriteLine(item);
        }
    }
}