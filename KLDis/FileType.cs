using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryAnalysis
{
    public abstract class FileType
    {
        protected string Description { get; set; }
        protected string Name { get; set; }

        private List<string> Extensions { get; }
            = new List<string>();

        private List<byte[]> Signatures { get; }
            = new List<byte[]>();

        public int SignatureLength => Signatures.Max(m => m.Length);

        protected FileType AddSignatures(params byte[][] bytes)
        {
            Signatures.AddRange(bytes);
            return this;
        }

        protected FileType AddExtensions(params string[] extensions)
        {
            Extensions.AddRange(extensions);
            return this;
        }

        public FileTypeVerifyResult Verify(Stream stream)
        {
            stream.Position = 0;
            var reader = new BinaryReader(stream);
            var headerBytes = reader.ReadBytes(SignatureLength);

            return new FileTypeVerifyResult
            {
                Name = Name,
                Description = Description,
                IsVerified = Signatures.Any(signature =>
                    headerBytes.Take(signature.Length)
                        .SequenceEqual(signature)
                )
            };
        }
    }

    public class FileTypeVerifyResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVerified { get; set; }
    }

    public sealed class Jpeg : FileType
    {
        public Jpeg()
        {
            Name = "JPEG";
            Description = "JPEG IMAGE";
            AddExtensions("jpeg", "jpg");
            AddSignatures(
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xFE }
            );
        }
    }

    public sealed class Pdf : FileType
    {
        public Pdf()
        {
            Name = "PDF";
            Description = "PDF Document";
            AddExtensions("pdf");
            AddSignatures(
                new byte[] { 0x25, 0x50, 0x44, 0x46 }
            );
        }
    }
}
