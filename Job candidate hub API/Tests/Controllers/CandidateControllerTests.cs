using Job_candidate_hub_API.Controllers;
using Job_candidate_hub_API.Models.DTOs;
using Job_candidate_hub_API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Job_candidate_hub_API.Tests.Controllers
{
    [TestFixture]
    public class CandidateControllerTests
    {
        private CandidateController _candidateController;
        private Mock<ICandidateService> _mockCandidateService;

        [SetUp]
        public void Setup()
        {
            _mockCandidateService = new Mock<ICandidateService>();
            _candidateController = new CandidateController(_mockCandidateService.Object);
        }

        [Test]
        public async Task AddOrUpdateCandidate_WithInvalidDto_ReturnsBadRequestWithModelState()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                FirstName = "John",
            };
            _candidateController.ModelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = await _candidateController.AddOrUpdateCandidate(candidateDto);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);

        }

        [Test]
        public async Task AddOrUpdateCandidate_WithValidDto_ReturnsOk()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                Email = "test@example.com",
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "1234567890",
                PreferredCallTime = new TimeSpan(14, 0, 0),
                LinkedInProfileUrl = "https://linkedin.com/in/test",
                GitHubProfileUrl = "https://github.com/test",
                Comment = "This is a comment."
            };
            var expectedCandidate = new CandidateDto 
            {
                Email = "test@example.com",
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                PhoneNumber = "1234567890",
                PreferredCallTime = new TimeSpan(14, 0, 0),
                LinkedInProfileUrl = "https://linkedin.com/in/test",
                GitHubProfileUrl = "https://github.com/test",
                Comment = "This is a comment."
            };
            _mockCandidateService.Setup(x => x.CreateUpdateCandidateAsync(candidateDto)).ReturnsAsync(expectedCandidate);

            // Act
            var result = await _candidateController.AddOrUpdateCandidate(candidateDto);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualCandidate = okResult.Value as CandidateDto;
            ClassicAssert.AreEqual(expectedCandidate, actualCandidate);
        }
        [Test]
        public async Task AddOrUpdateCandidate_NullDto_ReturnsBadRequest()
        {
            // Arrange
            CandidateDto candidateDto = null;
            _mockCandidateService.Setup(x => x.CreateUpdateCandidateAsync(It.IsAny<CandidateDto>()))
                .ThrowsAsync(new ArgumentException("Simulated error"));

            // Act
            var result = await _candidateController.AddOrUpdateCandidate(candidateDto);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
