using SheetDrama.Abstracts;
using SheetDrama.DataTransfer;
using System.Collections.Generic;

namespace SheetDrama.Templates.ChroniclesOfDarkness
{
    public class CoDDarkAgesSheet : ISheet
    {
        public string Concept { get; set; }
        public string Virtue { get; set; }
        public string Vice { get; set; }

        public int Inteligence { get; set; }
        public int Wits { get; set; }
        public int Resolve { get; set; }

        public int Presence { get; set; }
        public int Manipulation { get; set; }
        public int Composure { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Stamina { get; set; }

        public int Academics { get; set; }
        public int Enigmas { get; set; }
        public int Crafts { get; set; }
        public int Investigation { get; set; }
        public int Medicine { get; set; }
        public int Occult { get; set; }
        public int Politics { get; set; }
        public int Science { get; set; }

        public int AnimalKen { get; set; }
        public int Empathy { get; set; }
        public int Expression { get; set; }
        public int Intimidation { get; set; }
        public int Persuasion { get; set; }
        public int Socialize { get; set; }
        public int Streetwise { get; set; }
        public int Subterfuge { get; set; }

        public int Athletics { get; set; }
        public int Brawl { get; set; }
        public int Ride { get; set; }
        public int Archery { get; set; }
        public int Larceny { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }
        public int Weaponry { get; set; }

        public List<KeyStringValue> Specialties { get; set; }
        public List<KeyIntValue> Merits { get; set; }
        public List<string> Conditions { get; set; }
        public List<string> Aspirations { get; set; }

        public int Size { get; set; }
        public int Speed { get; set; }
        public int Defense
        {
            get
            {
                int low;
                if (Wits < Dexterity) low = Wits;
                else low = Dexterity;
                low += Athletics;
                return low;
            }
        }
        public int Armor { get; set; }
        public int IniativeMod
        {
            get
            {
                return Dexterity + Composure;
            }
        }
        public int Beats { get; set; }
        public int Experience { get; set; }

        public int Health { get; set; }
        public string Damage { get; set; }

        public int Willpower { get; set; }
        public int CurrentWillpower { get; set; }

        public int Integrity { get; set; }

        public List<string> Weapons { get; set; }
        public List<string> Equipment { get; set; }
        public List<string> Inventory { get; set; }

        public CoDDarkAgesSheet(string frame, string[] styles, string[] scripts) : base(frame, styles, scripts)
        {
            Inteligence = 1;
            Wits = 1;
            Resolve = 1;

            Strength = 1;
            Dexterity = 1;
            Stamina = 1;

            Presence = 1;
            Manipulation = 1;
            Composure = 1;

            Specialties = new();
            Merits = new();
            Conditions = new();
            Aspirations = new();

            Integrity = 7;

            Weapons = new();
            Equipment = new();
            Inventory = new();
        }
    }
}
