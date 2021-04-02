using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryAnalysis
{
    public static class FileTypeVerifier
    {
        private static FileTypeVerifyResult Unknown = new FileTypeVerifyResult
        {
            Name = "Unknown",
            IsVerified = false
        };

        static FileTypeVerifier()
        {
            Types = new List<FileType>
            {
                new Jpeg(),
                new Pdf()
            }
                .OrderByDescending(x => x.SignatureLength)
                .ToList();
        }

        private static IEnumerable<FileType> Types { get; set; }

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

            // if the filetype is recognized as a JPG/PDF, return JPG/
            return result?.IsVerified == true ? result : Unknown;
        }
    }
}
