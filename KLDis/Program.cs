using System;
using System.IO;
using System.Security.Cryptography;

namespace DirectoryAnalysis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Takes the directory
            Console.WriteLine("Type the directory that contains the files you want analyzed:");
            String directory = Convert.ToString(Console.ReadLine());

            while (!Directory.Exists(directory)) {
                Console.WriteLine("Invalid directory. Try again.");
                Console.WriteLine("Type the directory that contains the files you want analyzed:");
                directory = Convert.ToString(Console.ReadLine());
            }

            // Takes the path for the output file
            Console.WriteLine("Type the CSV FILE path for the output file:");
            String outputFile = Convert.ToString(Console.ReadLine());

            // Ask if we should also look into the subdirectories
            Console.WriteLine("Should we look into the subdirectories? [Type Y for yes, otherwise enter any key]:");
            Boolean ifSubdirectories = Convert.ToString(Console.ReadLine()).ToLower().Equals("y");

            Console.WriteLine("Processing the directory...");
            ProcessDirectory(directory, ifSubdirectories, outputFile);

            Console.WriteLine("You should now be able to see your results at " + outputFile);
        }

        public static void ProcessDirectory(string directory, Boolean ifSubdirectories, string outputFile)
        {
            // Processes the files in the immediate directory
            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, outputFile);

            // Recurses the subdirectories
            if (ifSubdirectories)
            {
                string[] subdirectoryEntries = Directory.GetDirectories(directory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory, ifSubdirectories, outputFile);
            }
        }

        public static void ProcessFile(string file, string outputFile)
        {
            var result = FileTypeVerifier.What(file);
            
            // Parses the files only if they are a PDF or a JPEG file
            if (result.Name.Equals("PDF") || result.Name.Equals("JPEG"))
            {
                var md5hash = CalculateMD5(file);
                AddToCSV(outputFile, file, result.Name, md5hash);
            }
        }

        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static void AddToCSV(string outputFile, string fullPath, string fType, string md5)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@outputFile, true))
                {
                    file.WriteLine(fullPath + "," + fType + "," + md5);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error: ", e);
            }
        }
    }
}
