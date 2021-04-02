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
            Console.WriteLine("Type the directory that contains the files you want analyzed.");
            String directory = Convert.ToString(Console.ReadLine());

            // Takes the path for the output file
            Console.WriteLine("Type the path for the output file.");
            String outputFile = Convert.ToString(Console.ReadLine());

            // Ask if we should also look into the subdirectories
            Console.WriteLine("Should we look into the subdirectories? [Y/N]");
            String ifSubdirectoriesInput = Convert.ToString(Console.ReadLine());
            Boolean ifSubdirectories = ifSubdirectoriesInput.ToLower().Equals("y");

            if (Directory.Exists(directory))
            {
                ProcessDirectory(directory, ifSubdirectories, outputFile);
            }
        }

        public static void ProcessDirectory(string directory, Boolean ifSubdirectories, string outputFile)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, outputFile);

            // Recurse into subdirectories of this directory.
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
            var md5hash = CalculateMD5(file);
            if (result.Name.Equals("PDF") || result.Name.Equals("JPEG"))
            {
                // Console.WriteLine($"{file} is a {result.Name} ({result.Description}).");
                AddToCSV(outputFile, file, result.Name, md5hash);
            }
        }

        /* public static void WriteToCSV(ToCSV outputFile, string fullPath, string fType, string md5) {
            CsvRow row = new CsvRow();
            Console.WriteLine(String.Format("{0}, {1}, {2}", fullPath, fType, md5));
            row.Add(String.Format("{0}, {1}, {2}", fullPath, fType, md5));
            outputFile.WriteRow(row);
        } */

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
