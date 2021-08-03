﻿using Newtonsoft.Json;

namespace RetiraTracker.Core
{
    public class ListItem
    {
        public string Key { get; private set; }
        public string Display { get; private set; }

        public string Content { get; private set; }
        public string ContentType { get; private set; }

        public bool DefaultSelected { get; private set; }

        public ListItem(string key, string display, bool selected = false)
        {
            Key = key;
            Display = display;
            DefaultSelected = selected;
        }

        public void SetContent<T>(T infoContent)
        {
            ContentType = infoContent.GetType().FullName;
            Content = infoContent.ToString();
            if (ContentType != typeof(string).FullName)
                Content = JsonConvert.SerializeObject(infoContent);
        }

        public T GetContent<T>()
        {
            return JsonConvert.DeserializeObject<T>(Content);
        }

        public override string ToString() { return Display; }
    }
}
