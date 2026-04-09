namespace Domain.Entites
{
    public class DataBatch
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSizeInBytes { get; set; }
        public BatchStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public ICollection<FinancialRecord> Records { get; set; }
        public BatchSummary? Summary { get; set; }
    }
}