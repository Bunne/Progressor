using System;
using LibGit2Sharp;

namespace Progressor {

    /* Special Task for a Git Repo
    */
    public class GitTask : Task {
        public Repository Repository { get; set; }

        /* Establish Task with Repo
        */
        public GitTask(string author,
                       string description,
                       string repository)
            : base(author, description) {
            Repository = new Repository(repository);
        }

        /* Read through the Commit logs and add the required count
        */
        public bool ReadUpdates(int count) {
            int read = 0;
            foreach (Commit c in Repository.Commits) {
                if (read < count) {
                    AddUpdate(new GitUpdate(
                        c.MessageShort,
                        c.Id.ToString(),
                        c.Author.Name,
                        c.Author.When.DateTime));
                    read++;
                } else {
                    break;
                }
            }
            return true;
        }
    }


    /* Special task for Manually entered lists
    */
    [Serializable]
    public class ManualTask : Task {
        DateTime Created { get; set; }
        DateTime? Completed { get; set; } = null;

        /* Establish basic task
        */
        public ManualTask(string Auth,
                          string desc)
            : base(Auth, desc) {
            Created = DateTime.Now;
        }

        /* Update the Task
        */
        public bool UpdateTask(string author,
                               string txt) {
            string hash = (author + DateTime.Now + txt).GetHashCode().ToString();
            return AddUpdate(new ManualUpdate(txt, hash, author, DateTime.Now));
        }
    }
}
