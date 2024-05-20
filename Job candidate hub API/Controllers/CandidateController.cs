using Job_candidate_hub_API.Models.DTOs;
using Job_candidate_hub_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Job_candidate_hub_API.Controllers
{
    public class CandidateController : BaseApiController
    {
        private readonly ICandidateService _candidateService;
        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] CandidateDto candidateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _candidateService.CreateUpdateCandidateAsync(candidateDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
