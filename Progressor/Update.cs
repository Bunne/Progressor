using System;

namespace Progressor {
    [Serializable]
    public class Update : IComparable {
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public string UpdateId { get; set; }

        public Update(string txt,
                      string id,
                      string author,
                      DateTime date) {
            Text = txt;
            UpdateId = id;
            Author = author;
            Created = date;
        }

        /* Override for each Update type
        */
        public virtual string ToMarkdown() {
            return "";
        }

        /* Make objects Sortable and Comparable
        */
        public int CompareTo(object obj) {
            if (obj == null) return 1;

            var up = obj as Update;
            if (up != null) {
                return Created.CompareTo(up.Created);
            }
            throw new ArgumentException("Object is not an Update");
        }

        public override bool Equals(object obj) {
            var up = obj as Update;
            if (up != null) {
                return UpdateId.Equals(up.UpdateId);
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return UpdateId.GetHashCode();
        }
    }


    /* The primary difference between the updates is how each is printed
     * to console and how each is rendered in Markdown
    */
    [Serializable]
    public class GitUpdate : Update {
        public GitUpdate(string txt,
                         string commitId,
                         string author,
                         DateTime date)
            : base(txt, commitId, author, date) { }

        public override string ToString() {
            return string.Format("{0} : {1}\n{2}", Author, Created, Text);
        }

        public override string ToMarkdown() {
            return string.Format("- **Commit** {0}<br/>\n" +
                                 "**Author:**&nbsp; {1}<br/>\n" +
                                 "**Date:**&nbsp;&nbsp;&nbsp;&nbsp; {2}<br/>\n" +
                                 "{3}\n",
                                 UpdateId,
                                 Author,
                                 Created,
                                 Text);
        }
    }

    [Serializable]
    public class ManualUpdate : Update {
        public ManualUpdate(string txt,
                            string id,
                            string author,
                            DateTime date)
            : base(txt, id, author, date) { }

        public override string ToString() {
            return string.Format("{0} : {1}\n{2}", Author, Created, Text);
        }

        public override string ToMarkdown() {
            return string.Format("* **{0} - {1}**<br/>\n" +
                                 "{2}\n",
                                 Author,
                                 Created,
                                 Text);
        }
    }
}
