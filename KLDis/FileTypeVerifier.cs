using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryAnalysis
{
    public static class FileTypeVerifier
    {
        private static IEnumerable<FileType> Types { get; set; }

        private static FileTypeVerifyResult Unknown = new FileTypeVerifyResult
        {
            Name = "Unknown",
            IsVerified = false
        };

        public static FileTypeVerifyResult What(string path)
        {
            // read the file from the path
            using var file = File.OpenRead(path);
            FileTypeVerifyResult result = null;

            foreach (var fileType in Types)
            {
                // check for the filetype of the file
                result = fileType.Verify(file);
                if (result.IsVerified)
                    break;
            }

            // if the filetype is recognized as a JPG/PDF, return JPG/PDF
            // otherwise return unknown
            return result?.IsVerified == true ? result : Unknown;
        }
    }
}
