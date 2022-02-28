using System;
using SheetDrama.Abstracts;
using SheetDrama.DataTransfer;
using System.Collections.Generic;
using System.Linq;

namespace SheetDrama.Templates.ChroniclesOfDarkness
{
    public class WtFDarkAgesSheet:ISheet
    {
        public string Age { get; set; }
        public string Concept { get; set; }
        public string Blood { get; set; }
        public string Bone { get; set; }
        public string Auspice { get; set; }
        public string Tribe { get; set; }
        public string Lodge { get; set; }

        public WerewolfForms Forms { get; set; }
        
        public int Intelligence { get; set; }
        public int IntelligenceBonus { get; set; }
        public int Wits { get; set; }
        public int WitsBonus { get; set; }
        public int Resolve { get; set; }
        public int ResolveBonus { get; set; }

        public int Strength { get; set; }
        public int StrengthBonus { get; set; }
        public int StrengthAutoBonus
        {
            get
            {
                return Forms switch
                {
                    WerewolfForms.Hishu or WerewolfForms.Urhan => 0,
                    WerewolfForms.Dalu => 1,
                    WerewolfForms.Gauru => 3,
                    WerewolfForms.Urshul => 2,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public int Dexterity { get; set; }
        public int DexterityBonus { get; set; }
        public int DexterityAutoBonus
        {
            get
            {
                return Forms switch
                {
                    WerewolfForms.Hishu or WerewolfForms.Dalu => 0,
                    WerewolfForms.Gauru => 1,
                    WerewolfForms.Urshul or WerewolfForms.Urhan => 2,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public int Stamina { get; set; }
        public int StaminaBonus { get; set; }
        public int StaminaAutoBonus
        {
            get
            {
                return Forms switch
                {
                    WerewolfForms.Hishu => 0,
                    WerewolfForms.Dalu or WerewolfForms.Urhan => 1,
                    WerewolfForms.Gauru or WerewolfForms.Urshul => 2,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public int Presence { get; set; }
        public int PresenceBonus { get; set; }
        public int Manipulation { get; set; }
        public int ManipulationBonus { get; set; }
        public int ManipulationAutoBonus
        {
            get
            {
                return Forms switch
                {
                    WerewolfForms.Hishu or WerewolfForms.Gauru => 0,
                    WerewolfForms.Dalu or WerewolfForms.Urshul or WerewolfForms.Urhan => -1,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
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

        private int size;
        public int Size { get { return size + SizeAutoBonus; } set { size = value; } }
        public int SizeAutoBonus
        {
            get
            {
                return Forms switch
                {
                    WerewolfForms.Hishu => 0,
                    WerewolfForms.Dalu or WerewolfForms.Urshul => 1,
                    WerewolfForms.Gauru => 2,
                    WerewolfForms.Urhan => -1,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public int Speed
        {
            get
            {
                return Forms switch 
                { 
                    WerewolfForms.Hishu or WerewolfForms.Dalu or WerewolfForms.Gauru => Strength + StrengthBonus + StrengthAutoBonus + Dexterity + DexterityBonus + DexterityAutoBonus + 5,
                    WerewolfForms.Urshul or WerewolfForms.Urhan => Strength + StrengthBonus + StrengthAutoBonus + Dexterity + DexterityBonus + DexterityAutoBonus + 8,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public int PerceptionBonus
        {
            get
            {
                return Forms switch
                {
                    WerewolfForms.Hishu => 1,
                    WerewolfForms.Dalu => 2,
                    WerewolfForms.Gauru or WerewolfForms.Urshul => 3,
                    WerewolfForms.Urhan => 4,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public int Defense
        {
            get
            {
                int[] attributes = { Wits + WitsBonus, Dexterity + DexterityBonus + DexterityAutoBonus };
                int low = attributes.Min();
                low += Athletics + AthleticsBonus;
                return low;
            }
        }
        public int Armor { get; set; }
        public int IniativeMod
        {
            get
            {
                return Dexterity + DexterityBonus + DexterityAutoBonus + Composure + ComposureBonus;
            }
        }
        public int Beats { get; set; }
        public int Experience { get; set; }

        public int Health
        {
            get
            {
                return Stamina + StaminaBonus + StaminaAutoBonus + Size;
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
                int val = Resolve + ResolveBonus + Composure + ComposureBonus;
                if (val > 10)
                    return 10;
                return val;
            }
        }
        public int CurrentWillpower { get; set; }

        public int PrimalUrge { get; set; }
        public int Essence { get; set; }
        public int MaxEssence
        {
            get
            {
                return PrimalUrge switch
                {
                    1 => 10,
                    2 => 11,
                    3 => 12,
                    4 => 13,
                    5 => 15,
                    6 => 20,
                    7 => 25,
                    8 => 30,
                    9 => 50,
                    _ => 75
                };
            }
        }

        public int Harmony { get; set; }
        public string FleshTouchstone { get; set; }
        public string SpiritTouchstone { get; set; }

        public int Purity { get; set; }
        public int Glory { get; set; }
        public int Honor { get; set; }
        public int Wisdom { get; set; }
        public int Cunning { get; set; }

        private List<KeyStringValue> passiveKuruthTriggers;
        public List<KeyStringValue> PassiveKuruthTriggers
        {
            get
            {
                if (passiveKuruthTriggers == null)
                    passiveKuruthTriggers = new();
                return passiveKuruthTriggers;
            }
            set { passiveKuruthTriggers = value; }
        }
        private List<KeyStringValue> commonKuruthTriggers;
        public List<KeyStringValue> CommonKuruthTriggers
        {
            get
            {
                if (commonKuruthTriggers == null)
                    commonKuruthTriggers = new();
                return commonKuruthTriggers;
            }
            set { commonKuruthTriggers = value; }
        }
        private List<KeyStringValue> specificKuruthTriggers;
        public List<KeyStringValue> SpecificKuruthTriggers
        {
            get
            {
                if (specificKuruthTriggers == null)
                    specificKuruthTriggers = new();
                return specificKuruthTriggers;
            }
            set { specificKuruthTriggers = value; }
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

        private List<KeyIntValue> moonGifts;
        public List<KeyIntValue> MoonGifts
        {
            get
            {
                if(moonGifts == null)
                    moonGifts = new();
                return moonGifts;
            }
            set { moonGifts = value; }
        }
        private List<KeyStringValue> shadowGifts;
        public List<KeyStringValue> ShadowGifts
        {
            get
            {
                if (shadowGifts == null)
                    shadowGifts = new();
                return shadowGifts;
            }
            set { shadowGifts = value; }
        }
        private List<KeyStringValue> wolfGifts;
        public List<KeyStringValue> WolfGifts
        {
            get
            {
                if (wolfGifts == null)
                    wolfGifts = new();
                return wolfGifts;
            }
            set { wolfGifts = value; }
        }
        private List<KeyStringValue> rites;
        public List<KeyStringValue> Rites
        {
            get
            {
                if (rites == null)
                    rites = new();
                return rites;
            }
            set { rites = value; }
        }

        public WtFDarkAgesSheet(string frame, string[] styles, string[] scripts) : base(frame, styles, scripts)
        {
            Template = GameTemplates.WerewolfTheForsakenDarkAges;
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

            Harmony = 7;

            Forms = WerewolfForms.Hishu;
            PrimalUrge = 1;
            Essence = 7;
        }

        public WtFDarkAgesSheet(CoDDarkAgesSheet sheet, string frame, string[] styles, string[] scripts) :base(frame, styles, scripts)
        {
            Template = GameTemplates.WerewolfTheForsakenDarkAges;
            CanChangeTo = Array.Empty<GameTemplates>();

            SheetId = sheet.SheetId;
            CharacterName = sheet.CharacterName;
            PlayerName = sheet.PlayerName;
            
            Age = sheet.Age;
            Concept = sheet.Concept;

            Forms = WerewolfForms.Hishu;

            Intelligence = sheet.Intelligence;
            Wits = sheet.Wits;
            Resolve = sheet.Resolve;

            Strength = sheet.Strength;
            Dexterity= sheet.Dexterity;
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
            Harmony = 9;
            Aspirations = sheet.Aspirations;
            Conditions = sheet.Conditions;
            Beats = sheet.Beats;
            Experience = sheet.Experience;
            Inventory = sheet.Inventory;
            Specialties = sheet.Specialties;

            Damage = sheet.Damage;
            Armor = sheet.Armor;
            Size = sheet.Size;

            PrimalUrge = 1;
            Essence = 7;
        }

        public WtFDarkAgesSheet() : base()
        {
            Template = GameTemplates.WerewolfTheForsakenDarkAges;
            CanChangeTo = Array.Empty<GameTemplates>();
        }

    }
}