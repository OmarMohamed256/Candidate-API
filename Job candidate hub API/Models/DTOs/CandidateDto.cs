namespace Job_candidate_hub_API.Models.DTOs
{
    public class CandidateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeSpan? PreferredCallTime { get; set; }
        public string? LinkedInProfileUrl { get; set; }
        public string? GitHubProfileUrl { get; set; }
        public string? Comment { get; set; }
    }
}
