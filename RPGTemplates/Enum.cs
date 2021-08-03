using SheetDrama.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheetDrama
{
    public enum Games
    {
        ChroniclesOfDarkness, ChroniclesOfDarknessDarkAges
    }

    public enum GameTemplates
    {
        [Game(Games.ChroniclesOfDarkness)]
        ChroniclesOfDarkness,
        [Game(Games.ChroniclesOfDarkness)]
        BeastThePrimordial,
        [Game(Games.ChroniclesOfDarkness)]
        ChangelingTheLost,
        [Game(Games.ChroniclesOfDarkness)]
        GeistTheSinEaters,
        [Game(Games.ChroniclesOfDarkness)]
        MageTheAwakening,
        [Game(Games.ChroniclesOfDarkness)]
        PrometheanTheCreated,
        [Game(Games.ChroniclesOfDarkness)]
        VampireTheRequiem,
        [Game(Games.ChroniclesOfDarkness)]
        WerewolfTheForsaken,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        ChroniclesOfDarknessDarkAges,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        BeastThePrimordialDarkAges,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        ChangelingTheLostDarkAges,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        GeistTheSinEatersDarkAges,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        MageTheAwakeningDarkAges,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        PrometheanTheCreatedDarkAges,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        VampireTheRequiemDarkAges,
        [Game(Games.ChroniclesOfDarknessDarkAges)]
        WerewolfTheForsakenDarkAges,

    }
}
