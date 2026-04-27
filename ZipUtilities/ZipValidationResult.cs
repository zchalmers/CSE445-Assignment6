namespace ZipUtilities
{
    public class ZipValidationResult
    {
        public string OriginalInput { get; set; }

        public string NormalizedZip { get; set; }

        public bool IsValid { get; set; }

        public string Message { get; set; }
    }
}
