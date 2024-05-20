using Job_candidate_hub_API.Errors;
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
                throw new BadRequestException("Candidate DTO cannot be null or empty");

            if (!RegexUtilities.IsValidEmail(candidateDto.Email))
                throw new BadRequestException("Invalid email format");

            var existingCandidate = await _candidateRepository.GetCandidateByEmailAsync(candidateDto.Email);

            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidateDto.FirstName;
                existingCandidate.LastName = candidateDto.LastName;
                existingCandidate.PhoneNumber = candidateDto.PhoneNumber;
                existingCandidate.PreferredCallTime = candidateDto.PreferredCallTime;
                existingCandidate.LinkedInProfileUrl = candidateDto.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidateDto.GitHubProfileUrl;
                existingCandidate.Comment = candidateDto.Comment;

                _candidateRepository.UpdateCandidate(existingCandidate);
            }
            else
                await _candidateRepository.AddCandidateAsync(CandidateMapper.ToEntity(candidateDto));

            var result = await _candidateRepository.SaveAllAsync();
            if (result) return candidateDto;
            throw new BadRequestException("Failed to add/update candidate");
        }
    }
}
