using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Main.FileIO
{
    public class FileManager
    {
        public FileManager()
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var casesDirectory = System.IO.Directory.GetParent(currentDirectory).Parent.Parent.GetDirectories("Main\\Cases\\Chao\\Set_64_234").First();

            var filePath = casesDirectory.FullName;
            const string filter = "*.txt";
            var files = Directory.GetFiles(filePath, filter);

            var fileFullName = files.FirstOrDefault();
            if (string.IsNullOrEmpty(fileFullName))
            {
                Console.WriteLine("Not 'in' file found in path: '{0}'", filePath);
                Console.Read();
                throw new Exception(String.Format("Not 'in' file found in path: '{0}'", filePath));
            }
            
            FileName = fileFullName.Substring(fileFullName.LastIndexOf("\\") + 1, fileFullName.LastIndexOf(".txt") - fileFullName.LastIndexOf("\\") - 1);
            FilePath = filePath;
        }

        public FileManager(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }

        public string FilePath { get; set; }

        public string FileName { get; set; }


        private string Input { get { return Path.Combine(FilePath, FileName); } }

        private string Output { get { return Path.Combine(FilePath, FileName + ".out"); } }


        public List<List<string>> ReadFile()
        {
            var ret = new List<List<string>>();
            string line;

            var file = new System.IO.StreamReader(Input);
            while ((line = file.ReadLine()) != null)
                ret.Add(line.Contains("\t") ? new List<string>(line.Split('\t')) : new List<string>(line.Split(' ')));

            file.Close();
            return ret;
        }

        public void WriteFile(List<string> output)
        {
            var file = new System.IO.StreamWriter(Output);

            foreach (var line in output)
                file.WriteLine(line);

            file.Close();
        }
    }
}
