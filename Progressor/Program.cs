using System;
using System.IO;
using Newtonsoft.Json;
using ManyConsole;
#pragma warning disable // For catch statements

namespace Progressor {
    
    class MainClass {
        public static void Main(string[] args) {
            // Process Operation
            try {
                var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(MainClass));
                ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
            } catch (Exception e) {
                Console.WriteLine("Unable to process command...");
            }
        }
    }


    class Setup {

        /* Establish some directory paths
        */
        static readonly string FOLDER_PATH = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "Progressor");

        static readonly string PROPERTIES_PATH = Path.Combine(
            FOLDER_PATH,
            "User.preferences"
        );

        static readonly string PROGRESS_PATH = Path.Combine(
            FOLDER_PATH,
            "Progress.bin"
        );

        /* Set up the Properties and Lists for access
        */
        public ProgressList progList { get; set; }

        public Setup() {
            PropCheck();

            // Load Properties, Create List
            progList = new ProgressList(JsonConvert.DeserializeObject<ProgressorProperties>
                                                 (File.ReadAllText(PROPERTIES_PATH)));

            // Load List
            if (File.Exists(PROGRESS_PATH)) {
                progList.Load(PROGRESS_PATH);
            }
        }

        public string getProgressPath() {
            return PROGRESS_PATH;
        }
        /* Check for Properties folder/file
        * Create if not found
        */
        public static void PropCheck() {
            if (!Directory.Exists(FOLDER_PATH)) {
                Directory.CreateDirectory(FOLDER_PATH);
            }
            if (!File.Exists(PROPERTIES_PATH)) {
                File.WriteAllText(
                    PROPERTIES_PATH,
                    "{\n    \"Author\" : \"\",\n    " +
                    "\"OutputMarkdown\": \"\",\n    " +
                    "\"MarkdownHeader\":\n    {\n    },\n    " +
                    "\"MarkdownTopText\": \"\",\n    " +
                    "\"UpdateCount\": 5,\n    " +
                    "\"Repositories\":" +
                    "\n    [" +
                    "\n        // {" +
                    "\n        //     \"Name\" : \"\"," +
                    "\n        //     \"Path\" : \"\"" +
                    "\n        // }" +
                    "\n    ]" +
                    "\n}"
                );
                Console.WriteLine("Properties file created at: " + PROPERTIES_PATH);
            }
        }
    }
}
