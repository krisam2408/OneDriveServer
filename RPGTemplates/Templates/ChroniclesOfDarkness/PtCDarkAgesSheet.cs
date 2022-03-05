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

        private List<KeyStringValue> m_specialties;
        public List<KeyStringValue> Specialties
        {
            get
            {
                if (m_specialties == null)
                    m_specialties = new();
                return m_specialties;
            }
            set { m_specialties = value; }
        }

        private List<KeyIntValue> m_merits;
        public List<KeyIntValue> Merits
        {
            get
            {
                if (m_merits == null)
                    m_merits = new();
                return m_merits;
            }
            set { m_merits = value; }
        }

        private List<KeyStringValue> m_conditions;
        public List<KeyStringValue> Conditions
        {
            get
            {
                if (m_conditions == null)
                    m_conditions = new();
                return m_conditions;
            }
            set { m_conditions = value; }
        }

        private List<KeyStringValue> m_aspirations;
        public List<KeyStringValue> Aspirations
        {
            get
            {
                if (m_aspirations == null)
                    m_aspirations = new();
                return m_aspirations;
            }
            set { m_aspirations = value; }
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

        private List<char> m_damage;
        public List<char> Damage
        {
            get
            {
                if (m_damage == null)
                    m_damage = new();

                return m_damage;
            }
            set { m_damage = value; }
        }

        public int Willpower
        {
            get
            {
                int val = Resolve + ResolveBonus + Composure + ComposureBonus;
                if (val > 10)
                    return 10;
                return val;
            }
        }
        public int CurrentWillpower { get; set; }

        private int m_azoth;
        public int Azoth 
        {
            get { return m_azoth; }
            set 
            {
                if (value < 1)
                    m_azoth = 1;
                if (value > 10)
                    m_azoth = 10;
                m_azoth = value;
            }
        }
        public int Pyros { get; set; }
        public int MaxPyros
        {
            get
            {
                return Azoth switch
                {
                    2 => 11,
                    3 => 12,
                    4 => 13,
                    5 => 15,
                    6 => 20,
                    7 => 30,
                    8 => 40,
                    9 => 50,
                    10 => 100,
                    _ => 10
                };
            }
        }

        public int Pilgrimage { get; set; }

        private List<KeyStringValue> m_bestowments;
        public List<KeyStringValue> Bestowments
        {
            get
            {
                if(m_bestowments == null)
                    m_bestowments = new();
                return m_bestowments;
            }
            set { m_bestowments = value; }
        }

        private List<TransmutationValue> m_transmutations;
        public List<TransmutationValue> Transmutations
        {
            get
            {
                if(m_transmutations == null)
                    m_transmutations = new();
                return m_transmutations;
            }
            set { m_transmutations = value; }
        }

        private List<KeyStringValue> m_inventory;
        public List<KeyStringValue> Inventory
        {
            get
            {
                if (m_inventory == null)
                    m_inventory = new();
                return m_inventory;
            }
            set { m_inventory = value; }
        }

        public PtCDarkAgesSheet(string frame, string[] styles, string[] scripts) : base(frame, styles, scripts)
        {
            Template = GameTemplates.PrometheanTheCreatedDarkAges;
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
            Template = GameTemplates.PrometheanTheCreatedDarkAges;
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
            Template = GameTemplates.PrometheanTheCreatedDarkAges;
            CanChangeTo = Array.Empty<GameTemplates>();
        }
    }
}
