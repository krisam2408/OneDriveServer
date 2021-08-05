using Newtonsoft.Json;

namespace SheetDrama.Abstracts
{
    public abstract class ISheet
    {
        public string SheetId { get; set; }
        public string CharacterName { get; set; }
        public string PlayerName { get; set; }
        
        public string SheetFrame { get; init; }
        public string[] SheetStyles { get; init; }
        public string[] SheetScripts { get; init; }

        protected ISheet(string frame, string[] styles, string[] scripts)
        {
            SheetFrame = frame;
            SheetStyles = styles;
            SheetScripts = scripts;
        }

        public string JsonSheet()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
