using Job_candidate_hub_API.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Job_candidate_hub_API.Repositories
{
    public class CachedCandidateRepositoryDecorator : ICandidateRepository
    {
        private readonly ICandidateRepository _decoratedCandidateRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(3);
        public CachedCandidateRepositoryDecorator(ICandidateRepository decoratedCandidateRepository, IMemoryCache memoryCache)
        {
            _decoratedCandidateRepository = decoratedCandidateRepository;
            _memoryCache = memoryCache;
        }
        public async Task AddCandidateAsync(Candidate candidate)
        {
            await _decoratedCandidateRepository.AddCandidateAsync(candidate);
            CacheCandidateAsync(candidate);
        }

        public async Task<Candidate> GetCandidateByEmailAsync(string email)
        {
            string key = email;
            return await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(3);
                    return await _decoratedCandidateRepository.GetCandidateByEmailAsync(email);
                });
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _decoratedCandidateRepository.SaveAllAsync();
        }

        public void UpdateCandidate(Candidate candidate)
        {
            _decoratedCandidateRepository.UpdateCandidate(candidate);
            CacheCandidateAsync(candidate);
        }
        private void CacheCandidateAsync(Candidate candidate)
        {
            _memoryCache.Set(candidate.Email, candidate, new MemoryCacheEntryOptions
            {
                SlidingExpiration = _cacheExpiration
            });
        }

    }
}
