using Newtonsoft.Json;
using SheetDrama.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SheetDrama
{
    public static class SheetFactory
    {
        public static string[] GetTemplatesNames()
        {
            IEnumerable<Type> templates = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.Contains("SheetDrama.Templates."));

            return templates.Select(t => t.Name)
                .ToArray();
        }

        public static ISheet GetBasicSheet(string template)
        {
            IEnumerable<Type> templates = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.Contains("SheetDrama.Templates."));

            Type targetTemplate = templates.Where(t => t.Name.ToUpper() == template.ToUpper())
                .FirstOrDefault();

            if (targetTemplate == null)
                throw new ArgumentOutOfRangeException(nameof(targetTemplate), "Template doesn't exist.");

            Type[] sheetParameters = new Type[] { };
            ConstructorInfo constructor = targetTemplate.GetConstructor(sheetParameters);
            ISheet sheet = (ISheet)constructor.Invoke(null);
            return sheet;
        }

        public static ISheet GetSheet(string template, string frame, string[] styles, string[] scripts)
        {
            IEnumerable<Type> templates = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.Contains("SheetDrama.Templates."));

            Type targetTemplate = templates.Where(t => t.Name.ToUpper() == template.ToUpper())
                .FirstOrDefault();

            if (targetTemplate == null)
                throw new ArgumentOutOfRangeException(nameof(targetTemplate), "Template doesn't exist.");

            Type[] sheetParameters = new Type[] { typeof(string), typeof(string[]), typeof(string[]) };
            ConstructorInfo constructor = targetTemplate.GetConstructor(sheetParameters);
            object[] parameters = new object[] { frame, styles, scripts };
            ISheet sheet = (ISheet)constructor.Invoke(parameters);
            return sheet;
        }

        public static ISheet GetSheet(string sheetTemplate, string jsonString)
        {
            IEnumerable<Type> templates = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.Contains("SheetDrama.Templates."));

            Type targetTemplate = templates.Where(t => t.Name.ToUpper() == sheetTemplate.ToUpper())
                .FirstOrDefault();

            if (targetTemplate == null)
                throw new ArgumentOutOfRangeException(nameof(targetTemplate), "Template doesn't exist.");

            ISheet sheet = (ISheet)JsonConvert.DeserializeObject(jsonString, targetTemplate);

            return sheet;
        }

        public static ISheet ChangeSheetTemplate(string newSheetTemplate, ISheet oldSheet, string frame, string[] styles, string[] scripts)
        {
            IEnumerable<Type> templates = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.Contains("SheetDrama.Templates."));

            Type targetTemplate = templates.Where(t => t.Name.ToUpper() == newSheetTemplate.ToUpper())
                .FirstOrDefault();

            if (targetTemplate == null)
                throw new ArgumentOutOfRangeException(nameof(targetTemplate), "Template doesn't exist.");

            Type[] sheetParameters = new Type[] { oldSheet.GetType(), typeof(string), typeof(string[]), typeof(string[]) };
            ConstructorInfo constructor = targetTemplate.GetConstructor(sheetParameters);
            object[] parameters = new object[] { oldSheet, frame, styles, scripts };
            ISheet sheet = (ISheet)constructor.Invoke(parameters);
            return sheet;
        }
    }
}
