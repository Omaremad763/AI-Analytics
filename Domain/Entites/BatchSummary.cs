namespace Domain.Entites
{
    public class BatchSummary
    {
        public Guid Id { get; set; }
        public Guid DataBatchId { get; set; }
        public string AiInsightText { get; set; }
        public DateTime GeneratedAt { get; set; }
        public DataBatch DataBatch { get; set; }
    }
}