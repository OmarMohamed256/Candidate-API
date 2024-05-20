using Job_candidate_hub_API.Data;
using Job_candidate_hub_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_candidate_hub_API.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCandidateAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
        }

        public async Task<Candidate> GetCandidateByEmailAsync(string email)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);
        }
        public void UpdateCandidate(Candidate candidate)
        {
            var trackedCandidate = _context.Candidates.Local.FirstOrDefault(c => c.Email == candidate.Email);
            if (trackedCandidate == null)
            {
                _context.Candidates.Attach(candidate);
                _context.Entry(candidate).State = EntityState.Modified;
            }
            else _context.Entry(trackedCandidate).CurrentValues.SetValues(candidate);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
