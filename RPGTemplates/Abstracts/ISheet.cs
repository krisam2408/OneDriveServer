using Newtonsoft.Json;
using System;

namespace SheetDrama.Abstracts
{
    public abstract class ISheet
    {
        public GameTemplates[] CanChangeTo { get; init; }
        public bool CanChange 
        { 
            get
            {
                if (CanChangeTo != null && CanChangeTo.Length > 0)
                    return true;
                return false;
            }
        }

        public string SheetId { get; set; }
        public string CharacterName { get; set; }
        public string PlayerName { get; set; }
        public DateTime LastModified { get; set; }
        public bool UsesBonuses { get; protected set; }
        
        public string SheetFrame { get; init; }
        public string[] SheetStyles { get; init; }
        public string[] SheetScripts { get; init; }

        protected ISheet(string frame, string[] styles, string[] scripts)
        {
            SheetFrame = frame;
            SheetStyles = styles;
            SheetScripts = scripts;
        }

        protected ISheet() { }

        public string JsonSheet()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
