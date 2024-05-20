using Job_candidate_hub_API.Models.DTOs;
using Job_candidate_hub_API.Models;

namespace Job_candidate_hub_API.Mapper
{
    public static class CandidateMapper
    {
        public static CandidateDto ToDto(Candidate candidate)
        {
            return new CandidateDto
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                PhoneNumber = candidate.PhoneNumber,
                Email = candidate.Email,
                PreferredCallTime = candidate.PreferredCallTime,
                LinkedInProfileUrl = candidate.LinkedInProfileUrl,
                GitHubProfileUrl = candidate.GitHubProfileUrl,
                Comment = candidate.Comment
            };
        }

        public static Candidate ToEntity(CandidateDto candidateDto)
        {
            return new Candidate
            {
                FirstName = candidateDto.FirstName,
                LastName = candidateDto.LastName,
                PhoneNumber = candidateDto.PhoneNumber,
                Email = candidateDto.Email,
                PreferredCallTime = candidateDto.PreferredCallTime,
                LinkedInProfileUrl = candidateDto.LinkedInProfileUrl,
                GitHubProfileUrl = candidateDto.GitHubProfileUrl,
                Comment = candidateDto.Comment
            };
        }
    }
}
