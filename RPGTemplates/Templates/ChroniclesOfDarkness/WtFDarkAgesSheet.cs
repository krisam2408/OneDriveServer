using System;
using SheetDrama.Abstracts;
using SheetDrama.DataTransfer;
using System.Collections.Generic;

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
        private int StrengthBonus
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
        private int DexterityBonus
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
        private int StaminaBonus
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
        private int ManipulationBonus
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

        private int SizeBonus
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
        
        public int Intelligence { get; set; }
        public int Wits { get; set; }
        public int Resolve { get; set; }

        public int Presence { get; set; }
        private int manipulation;
        public int Manipulation { get { return manipulation + ManipulationBonus; } set { manipulation = value; } }
        public int Composure { get; set; }

        private int strength;
        public int Strength { get { return strength + StrengthBonus; } set { strength = value; } }
        private int dexterity;
        public int Dexterity { get { return dexterity + DexterityBonus; } set { dexterity = value; } }
        private int stamina;
        public int Stamina { get { return stamina + StaminaBonus; } set { stamina = value; } }

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

        private int size;
        public int Size { get { return size + SizeBonus; } set { size = value; } }
        public int Speed
        {
            get
            {
                return Forms switch 
                { 
                    WerewolfForms.Hishu or WerewolfForms.Dalu or WerewolfForms.Gauru => Strength + Dexterity + 5,
                    WerewolfForms.Urshul => Strength + Dexterity + 8,
                    WerewolfForms.Urhan => Strength + Dexterity + 7,
                    _ => throw new ArgumentOutOfRangeException()
                };
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
                return Forms switch
                {
                    WerewolfForms.Dalu or WerewolfForms.Hishu => Dexterity + Composure,
                    WerewolfForms.Urshul or WerewolfForms.Urhan => Dexterity + Composure + 2,
                    WerewolfForms.Gauru => Dexterity + Composure + 1,
                    _ => throw new ArgumentOutOfRangeException()
                };
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
                return Resolve + Composure;
            }
        }
        public int CurrentWillpower { get; set; }

        public int PrimalUrge { get; set; }
        public int Essence { get; set; }

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
            CanChangeTo = Array.Empty<GameTemplates>();
            UsesBonuses = true;

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
            CanChangeTo = Array.Empty<GameTemplates>();
            UsesBonuses = true;

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
            CanChangeTo = Array.Empty<GameTemplates>();
            UsesBonuses = true;
        }

    }
}