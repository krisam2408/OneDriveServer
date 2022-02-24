using System;
using SheetDrama.Abstracts;
using SheetDrama.DataTransfer;
using System.Collections.Generic;
using System.Linq;

namespace SheetDrama.Templates.ChroniclesOfDarkness
{
    public class PtCDarkAgesSheet : ISheet
    {
        public string Age { get; set; }
        public string Concept { get; set; }
        public string Elpis { get; set; }
        public string Torment { get; set; }
        public string Lineage { get; set; }
        public string Refinement { get; set; }
        public string Role { get; set; }

        public int Intelligence { get; set; }
        public int IntelligenceBonus { get; set; }
        public int Wits { get; set; }
        public int WitsBonus { get; set; }
        public int Resolve { get; set; }
        public int ResolveBonus { get; set; }

        public int Strength { get; set; }
        public int StrengthBonus { get; set; }
        public int Dexterity { get; set; }
        public int DexterityBonus { get; set; }
        public int Stamina { get; set; }
        public int StaminaBonus { get; set; }

        public int Presence { get; set; }
        public int PresenceBonus { get; set; }
        public int Manipulation { get; set; }
        public int ManipulationBonus { get; set; }
        public int Composure { get; set; }
        public int ComposureBonus { get; set; }

        public int Academics { get; set; }
        public int AcademicsBonus { get; set; }
        public int Enigmas { get; set; }
        public int EnigmasBonus { get; set; }
        public int Crafts { get; set; }
        public int CraftsBonus { get; set; }
        public int Investigation { get; set; }
        public int InvestigationBonus { get; set; }
        public int Medicine { get; set; }
        public int MedicineBonus { get; set; }
        public int Occult { get; set; }
        public int OccultBonus { get; set; }
        public int Politics { get; set; }
        public int PoliticsBonus { get; set; }
        public int Science { get; set; }
        public int ScienceBonus { get; set; }

        public int AnimalKen { get; set; }
        public int AnimalKenBonus { get; set; }
        public int Empathy { get; set; }
        public int EmpathyBonus { get; set; }
        public int Expression { get; set; }
        public int ExpressionBonus { get; set; }
        public int Intimidation { get; set; }
        public int IntimidationBonus { get; set; }
        public int Persuasion { get; set; }
        public int PersuasionBonus { get; set; }
        public int Socialize { get; set; }
        public int SocializeBonus { get; set; }
        public int Streetwise { get; set; }
        public int StreetwiseBonus { get; set; }
        public int Subterfuge { get; set; }
        public int SubterfugeBonus { get; set; }

        public int Athletics { get; set; }
        public int AthleticsBonus { get; set; }
        public int Brawl { get; set; }
        public int BrawlBonus { get; set; }
        public int Ride { get; set; }
        public int RideBonus { get; set; }
        public int Archery { get; set; }
        public int ArcheryBonus { get; set; }
        public int Larceny { get; set; }
        public int LarcenyBonus { get; set; }
        public int Stealth { get; set; }
        public int StealthBonus { get; set; }
        public int Survival { get; set; }
        public int SurvivalBonus { get; set; }
        public int Weaponry { get; set; }
        public int WeaponryBonus { get; set; }

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
                return Strength+StrengthBonus + Dexterity+DexterityBonus + 5;
            }
        }
        public int Defense
        {
            get
            {
                int[] attributes = { Wits + WitsBonus, Dexterity + DexterityBonus };
                int low = attributes.Min();
                low += Athletics+AthleticsBonus;
                return low;
            }
        }
        public int Armor { get; set; }
        public int IniativeMod
        {
            get
            {
                return Dexterity + DexterityBonus + Composure + ComposureBonus;
            }
        }
        public int Beats { get; set; }
        public int Experience { get; set; }
        public int VitriolBeats { get; set; }
        public int VitriolExperience { get; set; }

        public int Health
        {
            get
            {
                return Stamina+StaminaBonus + Size;
            }
        }

        private List<char> damage;
        public List<char> Damage
        {
            get
            {
                if (damage == null)
                    damage = new();

                return damage;
            }
            set { damage = value; }
        }

        public int Willpower
        {
            get
            {
                return Resolve+ResolveBonus + Composure+ComposureBonus;
            }
        }
        public int CurrentWillpower { get; set; }

        public int Azoth { get; set; }
        public int Pyros { get; set; }
        public int MaxPyros
        {
            get
            {
                return Azoth switch
                {
                    1 => 10,
                    2 => 11,
                    3 => 12,
                    4 => 13,
                    5 => 15,
                    6 => 20,
                    7 => 30,
                    8 => 40,
                    9 => 50,
                    _ => 100
                };
            }
        }

        public int Pilgrimage { get; set; }

        private List<KeyStringValue> bestowments;
        public List<KeyStringValue> Bestowments
        {
            get
            {
                if(bestowments == null)
                    bestowments = new();
                return bestowments;
            }
            set { bestowments = value; }
        }

        private List<TransmutationValue> transmutations;
        public List<TransmutationValue> Transmutations
        {
            get
            {
                if(transmutations == null)
                    transmutations = new();
                return transmutations;
            }
            set { transmutations = value; }
        }

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

        public PtCDarkAgesSheet(string frame, string[] styles, string[] scripts) : base(frame, styles, scripts)
        {
            CanChangeTo = Array.Empty<GameTemplates>();

            Intelligence = 1;
            Wits = 1;
            Resolve = 1;

            Strength = 1;
            Dexterity = 1;
            Stamina = 1;

            Presence = 1;
            Manipulation = 1;
            Composure = 1;

            Pilgrimage = 1;

            Azoth = 1;
            Pyros = 10;
        }

        public PtCDarkAgesSheet(CoDDarkAgesSheet sheet, string frame, string[] styles, string[] scripts):base(frame, styles, scripts)
        {
            CanChangeTo = Array.Empty<GameTemplates>();

            SheetId = sheet.SheetId;
            CharacterName = sheet.CharacterName;
            PlayerName = sheet.PlayerName;

            Age = sheet.Age;
            Concept = sheet.Concept;

            Intelligence = sheet.Intelligence;
            Wits = sheet.Wits;
            Resolve = sheet.Resolve;

            Strength = sheet.Strength;
            Dexterity = sheet.Dexterity;
            Stamina = sheet.Stamina;

            Presence = sheet.Presence;
            Manipulation = sheet.Manipulation;
            Composure = sheet.Composure;

            Academics = sheet.Academics;
            Enigmas = sheet.Enigmas;
            Crafts = sheet.Crafts;
            Investigation = sheet.Investigation;
            Medicine = sheet.Medicine;
            Occult = sheet.Occult;
            Politics = sheet.Politics;
            Science = sheet.Science;

            Athletics = sheet.Athletics;
            Brawl = sheet.Brawl;
            Ride = sheet.Ride;
            Archery = sheet.Archery;
            Larceny = sheet.Larceny;
            Stealth = sheet.Stealth;
            Survival = sheet.Survival;
            Weaponry = sheet.Weaponry;

            AnimalKen = sheet.AnimalKen;
            Empathy = sheet.Empathy;
            Expression = sheet.Expression;
            Intimidation = sheet.Intimidation;
            Persuasion = sheet.Persuasion;
            Socialize = sheet.Socialize;
            Streetwise = sheet.Streetwise;
            Subterfuge = sheet.Subterfuge;

            Merits = sheet.Merits;
            CurrentWillpower = sheet.CurrentWillpower;
            Pilgrimage = 1;

            Aspirations = sheet.Aspirations;
            Conditions = sheet.Conditions;
            Beats = sheet.Beats;
            Experience = sheet.Experience;
            Inventory = sheet.Inventory;
            Specialties = sheet.Specialties;

            Damage = sheet.Damage;
            Armor = sheet.Armor;
            Size = sheet.Size;

            Azoth = 1;
            Pyros = 10;
        }

        public PtCDarkAgesSheet() : base()
        {
            CanChangeTo = Array.Empty<GameTemplates>();
        }
    }
}
