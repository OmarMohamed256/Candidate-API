using Job_candidate_hub_API.Mapper;
using Job_candidate_hub_API.Models.DTOs;
using Job_candidate_hub_API.Repositories;
using Job_candidate_hub_API.Utilities;

namespace Job_candidate_hub_API.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }
        public async Task<CandidateDto> CreateUpdateCandidateAsync(CandidateDto candidateDto)
        {
            if (candidateDto == null || string.IsNullOrWhiteSpace(candidateDto.Email))
                throw new ArgumentException("Candidate DTO cannot be null or empty", nameof(candidateDto));
            
            if (!RegexUtilities.IsValidEmail(candidateDto.Email)) 
                throw new ArgumentException("Invalid email format", nameof(candidateDto.Email));

            var existingCandidate = await GetCandidateByEmailAsync(candidateDto.Email);

            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidateDto.FirstName;
                existingCandidate.LastName = candidateDto.LastName;
                existingCandidate.PhoneNumber = candidateDto.PhoneNumber;
                existingCandidate.PreferredCallTime = candidateDto.PreferredCallTime;
                existingCandidate.LinkedInProfileUrl = candidateDto.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidateDto.GitHubProfileUrl;
                existingCandidate.Comment = candidateDto.Comment;
                
                _candidateRepository.UpdateCandidate(CandidateMapper.ToEntity(existingCandidate));
            }
            else
                await _candidateRepository.AddCandidateAsync(CandidateMapper.ToEntity(candidateDto));

            var result = await _candidateRepository.SaveAllAsync();
            if (result) return candidateDto;
            throw new ArgumentException("Failed to add/update candidate");
        }

        public async Task<CandidateDto> GetCandidateByEmailAsync(string email)
        {
            var candidate = await _candidateRepository.GetCandidateByEmailAsync(email);
            if(candidate != null)
               return CandidateMapper.ToDto(candidate);
            return null;
        }
    }
}
