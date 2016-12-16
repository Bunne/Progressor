using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Progressor {
    
    public class ProgressList {
        public ProgressorProperties Properties { get; set; }
        public Dictionary<string, GitTask> Repositories { get; set; }
        public Dictionary<int, ManualTask> ManTaskList { get; set; }

        /* Establish the Progress List for this session
        */
        public ProgressList(ProgressorProperties prop) {
            Repositories = new Dictionary<string, GitTask>();
            ManTaskList = new Dictionary<int, ManualTask>();
            Properties = prop;
        }

        /* Create GitTasks for all repositories in the Properties file
         * Update their TaskLists
         * Should only be done when necessary
        */
        public bool AddRepositories(){
            foreach (RepoDetails repo in Properties.Repositories) {
                if (!Repositories.ContainsKey(repo.Name)) {
                    try {
                        Repositories.Add(repo.Name,
                                         new GitTask(Properties.Author,
                                                     repo.Name,
                                                     repo.Path));
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            foreach (GitTask t in Repositories.Values) {
                t.ReadUpdates(Properties.UpdateCount);
            }
            return true;
        }


        /* Load the Manual Tasks
         * Do this every time the program is run
        */
        public bool Load(string progressPath) {
            try {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(progressPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                ManTaskList = (Dictionary<int, ManualTask>)formatter.Deserialize(stream);
                stream.Close();
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /* Save the Manual Tasks
         * Do this every time the program is run
        */
        public bool Save(string progressPath) {
            if (ManTaskList.Count > 0) {
                try {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(progressPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    formatter.Serialize(stream, ManTaskList);
                    stream.Close();
                    return true;
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            return false;
        }
    }
}
