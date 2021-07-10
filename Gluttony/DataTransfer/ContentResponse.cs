using Newtonsoft.Json;

namespace Gluttony.DataTransfer
{
    public class ContentResponse:Response
    {
        public string Content { get; private set; }
        public string ContentType { get; private set; }
        public bool ContentExists { get { return !string.IsNullOrWhiteSpace(Content); } }

        public void SetContent<T>(T content)
        {
            Content = JsonConvert.SerializeObject(content);
            ContentType = content.GetType().FullName;
        }

        public T GetContent<T>()
        {
            return JsonConvert.DeserializeObject<T>(Content);
        }
    }
}
