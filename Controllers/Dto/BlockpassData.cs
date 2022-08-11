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
    public class BlockpassProfileResponse
    {
        public string? status { get; set; }
        public BlockpassProfileData? data { get; set; }
    }
    public class IdentityAttribute
    {
        public string? type { get; set; }
        public string? value { get; set; }
    }
    public class BlockpassIdentity
    {
        public IdentityAttribute? address { get; set; }
        public IdentityAttribute? dob { get; set; }
        public IdentityAttribute? email { get; set; }
        public IdentityAttribute? family_name { get; set; }
        public IdentityAttribute? given_name { get; set; }
        public IdentityAttribute? phone { get; set; }
        public IdentityAttribute? selfie_national_id { get; set; }
        public IdentityAttribute? proof_of_address { get; set; }
        public IdentityAttribute? selfie { get; set; }
        public IdentityAttribute? passport { get; set; }
    }
    public class BlockpassProfileData
    {
        public string? refId { get; set; }
        public bool? isArchived { get; set; }
        public string? blockPassID { get; set; }
        public DateTime? inreviewDate { get; set; }
        public DateTime? waitingDate { get; set; }
        //customFields
        public BlockpassIdentity? identities { get; set; }
    }
}