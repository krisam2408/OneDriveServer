using SheetDrama.Abstracts;
using SheetDrama.DataTransfer;
using System.Collections.Generic;

namespace SheetDrama.Templates.ChroniclesOfDarkness
{
    public class CoDDarkAgesSheet : ISheet
    {
        public string Age { get; set; }
        public string Concept { get; set; }
        public string Virtue { get; set; }
        public string Vice { get; set; }

        public int Intelligence { get; set; }
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

        public KeyStringValue[] Specialties { get; set; }
        public KeyIntValue[] Merits { get; set; }
        public string[] Conditions { get; set; }
        public string[] Aspirations { get; set; }

        public int Size { get; set; }
        public int Speed 
        { 
            get
            {
                return Strength + Dexterity + 5;
            }
        }
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

        public int Health 
        { 
            get
            {
                return Stamina + Size;
            }
        }
        public string Damage { get; set; }

        public int Willpower 
        {
            get
            {
                return Resolve + Composure;
            }
        }
        public int CurrentWillpower { get; set; }

        public int Integrity { get; set; }

        public string[] Weapons { get; set; }
        public string[] Equipment { get; set; }
        public string[] Inventory { get; set; }

        public CoDDarkAgesSheet(string frame, string[] styles, string[] scripts) : base(frame, styles, scripts)
        {
            Intelligence = 1;
            Wits = 1;
            Resolve = 1;

            Strength = 1;
            Dexterity = 1;
            Stamina = 1;

            Presence = 1;
            Manipulation = 1;
            Composure = 1;

            Integrity = 7;
        }
    }
}
