using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTemplates.Extensions
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GameAttribute:Attribute
    {
        public Games Game { get; init; }

        public GameAttribute(Games game)
        {
            Game = game;
        }
    }

    public static class GameTemplatesExtensions
    {
        public static GameTemplates[] GameTemplates(this Enum game)
        {
            List<GameTemplates> output = new();

            foreach(GameTemplates gt in Enum.GetValues(typeof(GameTemplates)))
            {
                GameAttribute gameAttribute = (GameAttribute)gt.GetType()
                .GetMember(gt.ToString())
                .First()
                .GetCustomAttributes(typeof(GameAttribute), false)
                .First();

                if (gameAttribute.Game == (Games)game)
                    output.Add(gt);
            }

            return output.ToArray();
        }

        public static string WriteEnum(this Enum e)
        {
            string output = string.Empty;

            int i = 0;
            foreach(char c in e.ToString())
            {
                if (i > 0 && c.ToString() == c.ToString().ToUpper())
                    output += " ";
                output += c;
                i++;
            }

            return output;
        }
    }
}
