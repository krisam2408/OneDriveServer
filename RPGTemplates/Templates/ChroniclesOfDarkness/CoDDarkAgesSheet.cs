using System;
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

        private List<KeyStringValue> specialties;
        public List<KeyStringValue> Specialties 
        {
            get
            {
                if (specialties == null)
                    specialties = new();
                return specialties;
            }
            set { specialties = value; }
        }

        private List<KeyIntValue> merits;
        public List<KeyIntValue> Merits 
        {
            get 
            {
                if (merits == null)
                    merits = new();
                return merits;
            }
            set { merits = value; }
        }

        private List<KeyStringValue> conditions;
        public List<KeyStringValue> Conditions 
        {
            get
            {
                if (conditions == null)
                    conditions = new();
                return conditions;
            }
            set { conditions = value; }
        }

        private List<KeyStringValue> aspirations;
        public List<KeyStringValue> Aspirations 
        {
            get
            {
                if (aspirations == null)
                    aspirations = new();
                return aspirations;
            }
            set { aspirations = value; }
        }

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

        private List<char> damage;
        public List<char> Damage 
        {
            get
            {
                if(damage == null)
                {
                    damage = new();
                    for (int i = 0; i < Health; i++)
                        damage.Add(' ');
                }

                return damage;
            }
            set { damage = value; }
        }

        public int Willpower 
        {
            get
            {
                return Resolve + Composure;
            }
        }
        public int CurrentWillpower { get; set; }

        public int Integrity { get; set; }

        private List<KeyStringValue> inventory;
        public List<KeyStringValue> Inventory 
        {
            get
            {
                if (inventory == null)
                    inventory = new();
                return inventory;
            }
            set { inventory = value; }
        }

        public CoDDarkAgesSheet(string frame, string[] styles, string[] scripts) : base(frame, styles, scripts)
        {
            CanChangeTo = new Type[] { };

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
