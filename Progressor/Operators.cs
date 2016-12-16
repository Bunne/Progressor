//#define DEBUG 
using System;
using System.Collections.Generic;
using System.IO;
using ManyConsole;
#pragma warning disable // For catch statements

namespace Progressor {


    /* CREATE a new task
    */
    public class CreateTaskCommand : ConsoleCommand{
        public string TaskName { get; set; }

        public CreateTaskCommand() {
            IsCommand("create", "Create a task.");

            HasRequiredOption("d|desc=", "The task description", t => TaskName = t);
        }

        public override int Run(string[] remainingArguments) {
            try {
                Setup setup = new Setup();
                setup.progList.ManTaskList.Add(
                    setup.progList.ManTaskList.Count + 1,
                    new ManualTask(setup.progList.Properties.Author, TaskName)
                );
                setup.progList.Save(setup.getProgressPath());
                return 0;
            } catch (Exception e) {
                #if (DEBUG)
                Console.Write(e.Message);
                Console.WriteLine(e.StackTrace);
                #endif
                return 2;
            }
        }
    }


    /* DELETE a task
    */
    public class DeleteTaskCommand : ConsoleCommand {
        public int TaskNum { get; set; }

        public DeleteTaskCommand() {
            IsCommand("delete", "Delete a task.");

            HasRequiredOption("t|task=", "The task to delete", t => TaskNum = Convert.ToInt32(t.Trim()));
        }

        public override int Run(string[] remainingArguments) {
            try {
                Setup setup = new Setup();

                Dictionary<int, ManualTask> ManTaskListNew = new Dictionary<int, ManualTask>();

                setup.progList.ManTaskList.Remove(TaskNum);

                int newIndex = 1;
                foreach (ManualTask mt in setup.progList.ManTaskList.Values) {
                    ManTaskListNew.Add(newIndex, mt);
                    newIndex++;
                }
                setup.progList.ManTaskList = ManTaskListNew;

                setup.progList.Save(setup.getProgressPath());
                return 0;
            } catch (Exception e) {
                #if (DEBUG)
                Console.Write(e.Message);
                Console.WriteLine(e.StackTrace);
                #endif
                return 2;
            }
        }
    }


    /* UPDATE a task
    */
    public class UpdateTaskCommand : ConsoleCommand {
        public int TaskNum { get; set; }
        public string TaskUpdate { get; set; }

        public UpdateTaskCommand() {
            IsCommand("update", "Update a task.");

            HasRequiredOption("t|task=", "The task number", t => TaskNum = Convert.ToInt32(t.Trim()));
            HasRequiredOption("u|update=", "The update text", u => TaskUpdate = u);
        }

        public override int Run(string[] remainingArguments) {
            try {
                Setup setup = new Setup();
                setup.progList.ManTaskList[TaskNum].UpdateTask(setup.progList.Properties.Author, TaskUpdate);
                setup.progList.Save(setup.getProgressPath());
                return 0;
            } catch (Exception e) {
                #if (DEBUG)
                Console.Write(e.Message);
                Console.WriteLine(e.StackTrace);
                #endif
                return 2;
            }
        }
    }


    /* LIST UPDATES for a specific task
    */
    public class ListUpdatesCommand : ConsoleCommand {
        public int TaskNum { get; set; }

        public ListUpdatesCommand() {
            IsCommand("lupdates", "List existing task updates.");

            HasRequiredOption("t|task=", "Which task to list.",
                              t => TaskNum = Convert.ToInt32(t.Trim()));
        }

        public override int Run(string[] remainingArguments) {
            try {
                Setup setup = new Setup();
                if (TaskNum > 0) {
                    foreach (Update mu in setup.progList.ManTaskList[TaskNum].Updates) {
                        Console.WriteLine(mu);
                    }
                } else {
                    Console.WriteLine("Must indicate an existing task.");
                    return 2;
                }
                return 0;
            } catch (Exception e) {
                #if (DEBUG)
                Console.Write(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(TaskNum);
                #endif
                return 2;
            }
        }
    }


    /* LIST all tasks
    */
    public class ListTasksCommand : ConsoleCommand {
        public int TaskNum { get; set; }

        public ListTasksCommand() {
            IsCommand("ltasks", "List existing tasks.");
        }

        public override int Run(string[] remainingArguments) {
            try {
                Setup setup = new Setup();
                foreach (var t in setup.progList.ManTaskList) {
                    Console.WriteLine(t.Key + ": " + t.Value);
                }
                return 0;
            } catch (Exception e) {
                #if (DEBUG)
                Console.Write(e.Message);
                Console.WriteLine(e.StackTrace);
                #endif
                return 2;
            }
        }
    }


    /* GENERATE MARKDOWN
    */
    public class OutputMarkdownCommand : ConsoleCommand {
        public string markdownOut { get; set; } = "";

        public OutputMarkdownCommand() {
            IsCommand("markdown", "Generate Markdown.");
        }

        public override int Run(string[] remainingArguments) {
            try {
                Setup setup = new Setup();
                if (setup.progList.Properties.OutputMarkdown.Length == 0) {
                    Console.WriteLine("Property MarkdownOutput cannot be empty.");
                    return 2;
                }
                setup.progList.AddRepositories();

                // HEADER
                markdownOut += "---\n";
                foreach (KeyValuePair<string, string> k in setup.progList.Properties.MarkdownHeader) {
                    markdownOut += string.Format("{0}: {1}\n", k.Key, k.Value);
                }
                markdownOut += "---\n";

                // OPTIONAL HEADER TEXT
                if (setup.progList.Properties.MarkdownTopText.Length > 0) {
                    markdownOut += string.Format("\n{0}\n", setup.progList.Properties.MarkdownTopText);
                }
                // GITHUB
                if (setup.progList.Repositories.Count > 0) {
                    markdownOut += "\n\n## Github";
               
                    foreach (GitTask gt in setup.progList.Repositories.Values) {
                        markdownOut += gt.ToMarkdown();

                        foreach (GitUpdate gu in gt.Updates) {
                            markdownOut += gu.ToMarkdown();
                        }
                    }
                }

                // MANUAL TASKS
                if (setup.progList.ManTaskList.Count > 0) {
                    markdownOut += "\n\n## Personal";
                    foreach (ManualTask mt in setup.progList.ManTaskList.Values) {
                        if (mt.Updates.Count > 0) {
                            markdownOut += mt.ToMarkdown();
                            int read = 0;
                            foreach (ManualUpdate mu in mt.Updates) {
                                if (read < setup.progList.Properties.UpdateCount) {
                                    markdownOut += mu.ToMarkdown();
                                    read++;
                                } else {
                                    break;
                                }
                            }
                        }
                    }
                }

                // WRITE 
                try {
                    File.WriteAllText(
                        setup.progList.Properties.OutputMarkdown,
                        markdownOut
                    );
                    Console.WriteLine("Markdown file created at: " + setup.progList.Properties.OutputMarkdown);
                } catch (Exception e) {
                }
                return 0;
            } catch (Exception e) {
                #if (DEBUG)
                Console.Write(e.Message);
                Console.WriteLine(e.StackTrace);
                #endif
                return 2;
            }
        }
    }
}
