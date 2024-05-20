using Job_candidate_hub_API.Models.DTOs;

namespace Job_candidate_hub_API.Services
{
    public interface ICandidateService
    {
        Task<CandidateDto> CreateUpdateCandidateAsync(CandidateDto candidateDto);
    }
}
