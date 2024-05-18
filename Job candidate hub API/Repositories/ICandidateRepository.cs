using Job_candidate_hub_API.Models;

namespace Job_candidate_hub_API.Repositories
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetCandidateByEmailAsync(string email);
        Task AddCandidateAsync(Candidate candidate);
        void UpdateCandidateAsync(Candidate candidate);
        Task<bool> SaveAllAsync();
    }
}
