using SheetDrama;
using SheetDrama.Extensions;
using System;

namespace RetiraTracker.Extensions
{
    public static class SheetDramaExtension
    {
        public static string[] GetGameCommands(this Games game)
        {
            switch(game)
            {
                case Games.ChroniclesOfDarkness:
                case Games.ChroniclesOfDarknessDarkAges:
                    return new string[] { "CoDTemplateCommand" };
                default:
                    return Array.Empty<string>();
            }
        }

        public static string GetTemplatePage(this GameTemplates template)
        {
            string output = template.GetTemplate();
            output = output.Replace("Sheet", "Page.xaml");
            return output;
        }
    }
}
