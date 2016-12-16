using System;
using System.Collections.Generic;

namespace Progressor {
    [Serializable]
    public class Task {
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<Update> Updates { get; set; }

        /* Set this list's properties
        */
        public Task(string Auth, string desc) {
            Author = Auth;
            Description = desc;
            LastUpdated = DateTime.Now;
            Updates = new List<Update> { };
        }

        /* Add an update to this list
        */
        public bool AddUpdate(Update u) {
            if (LastUpdated < u.Created) {
                LastUpdated = u.Created;
            }
            if (Updates.Count == 0 ||
                !u.UpdateId.Equals(Updates[0].UpdateId)) {
                Updates.Add(u);
                Sort();
                return true;
            }
            return false;
        }

        /* Sort then order Recent to Oldest
        */
        public void Sort() {
            Updates.Sort();
            Updates.Reverse();
        }

        /* Override ToString
        */
        public override string ToString() {
            return Description;
        }

        public string ToMarkdown() {
            return string.Format("\n\n### {0}\n\n",Description);
        }
    }
}
