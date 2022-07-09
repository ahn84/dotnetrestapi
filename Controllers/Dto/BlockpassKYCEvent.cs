using System.Text.Json.Serialization;

namespace DotNetRestApi.Controllers.Dto
{
    public class BlockpassKYCEvent
    {
        public string? guid { get; set; }
        public string? status { get; set; }
        public string? clientId { get; set; }
        [JsonPropertyName("event")]
        public string? _event { get; set; }
        public string? recordId { get; set; }
        public string? refId { get; set; }
        public int submitCount { get; set; }
        public string? blockPassID { get; set; }
        public bool? isArchived { get; set; }
        public DateTime? inreviewDate { get; set; }
        public DateTime? waitingDate { get; set; }
        public DateTime? approvedDate { get; set; }
        public bool? isPing { get; set; }
        public string? env { get; set; }
        public string? webhookId { get; set; }
    }
}