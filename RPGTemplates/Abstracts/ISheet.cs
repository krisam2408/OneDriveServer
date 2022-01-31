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
        // -> Add Character Image
        public DateTime LastModified { get; set; }
        
        public string SheetFrame { get; set; }
        public string[] SheetStyles { get; set; }
        public string[] SheetScripts { get; set; }

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
