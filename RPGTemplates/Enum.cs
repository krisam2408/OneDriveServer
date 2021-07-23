using RPGTemplates.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTemplates
{
    public enum Games
    {
        ChroniclesOfDarkness
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
        MageTheAwakeing,
        [Game(Games.ChroniclesOfDarkness)]
        PrometheanTheCreated,
        [Game(Games.ChroniclesOfDarkness)]
        VampireTheRequiem,
        [Game(Games.ChroniclesOfDarkness)]
        WerewolfTheForsaken,

    }
}
