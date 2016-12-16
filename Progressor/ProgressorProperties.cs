using System.Collections.Generic;

namespace Progressor {

    public class ProgressorProperties {
        public string Author { get; set; } = "";
        public string OutputMarkdown { get; set; } = "";
        public Dictionary<string, string> MarkdownHeader { get; set; } = new Dictionary<string, string>();
        public string MarkdownTopText { get; set; } = "";
        public int UpdateCount { get; set; } = 5;
        public List<RepoDetails> Repositories { get; set; } = new List<RepoDetails>();
    }

    public class RepoDetails {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
    }
}
