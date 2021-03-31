using System;
using System.IO;
using System.Collections;

namespace DirectoryAnalysis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Takes the directory
            Console.WriteLine("Type the directory that contains the files you want analyzed.");
            String directory = Convert.ToString(Console.ReadLine());

            // Takes the path for the output file
            Console.WriteLine("Type the path for the output file.");
            String outputFilePath = Convert.ToString(Console.ReadLine());

            // Ask if we should also look into the subdirectories
            Console.WriteLine("Should we look into the subdirectories? [Y/N]");
            String ifSubdirectoriesInput = Convert.ToString(Console.ReadLine());
            Boolean ifSubdirectories = ifSubdirectoriesInput.ToLower().Equals("y");

            if (Directory.Exists(directory))
            {
                ProcessDirectory(directory, ifSubdirectories);
            }
        }

        public static void ProcessDirectory(string directory, Boolean ifSubdirectories)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            if (ifSubdirectories)
            {
                string[] subdirectoryEntries = Directory.GetDirectories(directory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory, ifSubdirectories);
            }
        }

        public static void ProcessFile(string file)
        {
            // Console.WriteLine("Processed file '{0}'.", file);
            var path = Path.Combine("./assets", asset);
            var result = FileTypeVerifier.What(path);
            Console.WriteLine($"{asset} is a {result.Name} ({result.Description}).");
        }
    }
}
