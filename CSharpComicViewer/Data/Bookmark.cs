using Newtonsoft.Json;

namespace CSharpComicViewer.Data
{
    public class Bookmark
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        public string[] FilePaths { get; set; }

        public int Page { get; set; }
    }
}