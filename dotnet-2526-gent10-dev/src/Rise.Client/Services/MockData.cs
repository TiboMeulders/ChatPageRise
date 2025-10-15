using System;
using System.Collections.Generic;

namespace Rise.Client.Services
{
    public static class MockData
    {
        // Mock data voor de ingelogde gebruiker (Tom Kluskens)
        public static Person LoggedInUser => new Person 
        { 
            Id = 1, 
            Name = "Tom", 
            Age = "23", 
            Bio = "Software developer en muziekliefhebber", 
            BirthDate = "10/08/2002", 
            FunFacts = new List<string> { "Ik speel piano", "Ik ben een nachtmens" } 
        };

        // Mock data voor vriendschapsverzoeken
        public static List<Person> FriendshipRequests => new()
        {
            new Person { Id = 10, Name = "Jan", Age = "21", Bio = "Ik hou van voetbal en gamen", BirthDate = "24/06/2004", FunFacts = new List<string> { "Ik voebal graag", "Ik speel piano" } },
            new Person { Id = 11, Name = "Emma", Age = "19", Bio = "Student psychologie, hou van lezen", BirthDate = "15/03/2006", FunFacts = new List<string> { "Ik lees graag", "Ik hou van films" } },
            new Person { Id = 12, Name = "Lisa", Age = "20", Bio = "Kunstenares en natuurliefhebber", BirthDate = "22/11/2005", FunFacts = new List<string> { "Ik hou van reizen", "Ik lees graag" } }
        };

        // Mock data voor vrienden
        public static List<Friend> Friends => new()
        {
            new Friend { Id = 2, Name = "Piet", Age = "21", IsOnline = true, Bio = "Altijd klaar voor een avontuur!", BirthDate = "05/07/2004", FunFacts = new List<string> { "Ik hou van reizen", "Ik ben sportief" } },
            new Friend { Id = 3, Name = "Anna", Age = "22", IsOnline = false, Bio = "Nog geen bio", BirthDate = "12/01/2003", FunFacts = new List<string> { "Ik eet graag", "Ik ga graag uit" } },
            new Friend { Id = 4, Name = "Jef", Age = "21", IsOnline = true, Bio = "Sportfanaat en reisliefhebber", BirthDate = "30/04/2004", FunFacts = new List<string> { "Ik ben sportief", "Ik hou van reizen" } },
            new Friend { Id = 5, Name = "Marie", Age = "20", IsOnline = false, Bio = "Fotografe in de dop", BirthDate = "18/09/2005", FunFacts = new List<string> { "Ik hou van films", "Ik hou van koken" } },
            new Friend { Id = 6, Name = "Kevin", Age = "24", IsOnline = true, Bio = "Chef-kok en foodblogger", BirthDate = "03/12/2001", FunFacts = new List<string> { "Ik hou van koken", "Ik eet graag" } }
        };

        // Mock data voor voorgestelde vrienden
        public static List<Person> SuggestedFriends => new()
        {
            new Person { Id = 20, Name = "Sarah", Age = "21", Bio = "Danseres en yogainstructeur", BirthDate = "14/02/2004", FunFacts = new List<string> { "Ik ben sportief", "Ik ben een nachtmens" } },
            new Person { Id = 21, Name = "Mike", Age = "23", Bio = "Filmmaker en cinefiel", BirthDate = "07/06/2002", FunFacts = new List<string> { "Ik hou van films", "Ik speel piano" } },
            new Person { Id = 22, Name = "Luna", Age = "20", Bio = "Nog geen bio", BirthDate = "25/10/2005", FunFacts = new List<string> { "Ik lees graag", "Ik ga graag uit" } },
            new Person { Id = 23, Name = "David", Age = "22", Bio = "Muzikant en producer", BirthDate = "11/05/2003", FunFacts = new List<string> { "Ik speel piano", "Ik ben een nachtmens" } },
            new Person { Id = 24, Name = "Sophie", Age = "21", Bio = "Architectuurstudent", BirthDate = "08/09/2004", FunFacts = new List<string> { "Ik lees graag", "Ik hou van reizen" } },
            new Person { Id = 25, Name = "Lucas", Age = "24", Bio = "Personal trainer", BirthDate = "20/01/2001", FunFacts = new List<string> { "Ik ben sportief", "Ik hou van koken" } },
            new Person { Id = 26, Name = "Nina", Age = "19", Bio = "Grafisch ontwerper", BirthDate = "16/07/2006", FunFacts = new List<string> { "Ik hou van films", "Ik ga graag uit" } },
            new Person { Id = 27, Name = "Max", Age = "23", Bio = "Nog geen bio", BirthDate = "29/12/2002", FunFacts = new List<string> { "Ik voebal graag", "Ik eet graag" } }
        };

        public static Person GetPersonById(int id)
        {
            // Check if it's the logged in user
            if (id == LoggedInUser.Id) return LoggedInUser;

            // Zoek eerst in vrienden
            var friend = Friends.Find(f => f.Id == id);
            if (friend != null) return friend;

            // Dan in vriendschapsverzoeken
            var request = FriendshipRequests.Find(r => r.Id == id);
            if (request != null) return request;

            // Dan in voorgestelde vrienden
            var suggested = SuggestedFriends.Find(s => s.Id == id);
            if (suggested != null) return suggested;

            // Als niemand gevonden, return default persoon
            return new Person { Id = 0, Name = "Onbekend", Age = "0", Bio = "Nog geen bio", BirthDate = "01/01/2000" };
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string Bio { get; set; } = "Nog geen bio";
        public string BirthDate { get; set; } = string.Empty;
        public List<string> FunFacts { get; set; } = new();
    }

    public class Friend : Person
    {
        public bool IsOnline { get; set; }
    }
}