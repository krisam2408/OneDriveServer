using System;
using System.Collections.Generic;
using System.Linq;

namespace SheetDrama.Extensions
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
        public static GameTemplates[] GameTemplates(this Games game)
        {
            List<GameTemplates> output = new();

            foreach(GameTemplates gt in Enum.GetValues(typeof(GameTemplates)))
            {
                GameAttribute gameAttribute = (GameAttribute)gt.GetType()
                .GetMember(gt.ToString())
                .First()
                .GetCustomAttributes(typeof(GameAttribute), false)
                .First();

                if (gameAttribute.Game == game)
                    output.Add(gt);
            }

            return output.ToArray();
        }

        public static string GetTemplate(this GameTemplates template)
        {
            string output = string.Empty;
            string[] outOfTemplate = new string[] { "DarkAges" };
            string input = null;

            string text = template.ToString();
            foreach(string str in outOfTemplate)
                if(text.Contains(str))
                {
                    text = text.Replace(str, string.Empty);
                    input = str;
                }

            foreach (char c in text)
                if (c.ToString() == c.ToString().ToUpper())
                    output += c.ToString().ToUpper();

            if (!string.IsNullOrWhiteSpace(input))
                output += input;

            output += "Sheet";

            return output;
        }

        public static string WriteEnum(this Enum template)
        {
            string output = string.Empty;

            int i = 0;
            foreach(char c in template.ToString())
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
