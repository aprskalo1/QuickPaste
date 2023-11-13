namespace QuickPaste.DTO_s
{
    public class FileStorageDTO
    {
        public int Id { get; set; }

        public DateTime? TimeUploaded { get; set; }

        public string Filename { get; set; } = null!;

        public int? PastedCodeId { get; set; }
    }
}
