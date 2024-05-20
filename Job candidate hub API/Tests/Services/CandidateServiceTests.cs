using Job_candidate_hub_API.Models.DTOs;
using Job_candidate_hub_API.Repositories;
using Job_candidate_hub_API.Services;
using NUnit.Framework;
using Moq;
using Job_candidate_hub_API.Models;
using NUnit.Framework.Legacy;

namespace Job_candidate_hub_API.Tests.Services
{
    [TestFixture]
    public class CandidateServiceTests
    {
        private Mock<ICandidateRepository> _mockCandidateRepository;
        private ICandidateService _candidateService;
        private CandidateDto _expectedCandidateDto;
        private Candidate _existingCandidateEntity;
        private int _nextTestId = 1;

        [SetUp]
        public void Setup()
        {
            _mockCandidateRepository = new Mock<ICandidateRepository>();
            _candidateService = new CandidateService(_mockCandidateRepository.Object);
        }

        [Test]
        public async Task GetCandidateByEmailAsync_CandidateExists_ReturnsCandidateDto()
        {
            // Arrange
            _expectedCandidateDto = new CandidateDto
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
            };

            _existingCandidateEntity = new Candidate
            {
                Id = _nextTestId++,
                Email = _expectedCandidateDto.Email,
                FirstName = _expectedCandidateDto.FirstName,
                LastName = _expectedCandidateDto.LastName,
                PhoneNumber = _expectedCandidateDto.PhoneNumber,
            };
            _mockCandidateRepository.Setup(repo => repo.GetCandidateByEmailAsync(It.IsAny<string>()))
                                   .ReturnsAsync(_existingCandidateEntity);

            // Act
            var result = await _candidateService.GetCandidateByEmailAsync(_existingCandidateEntity.Email);

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(_expectedCandidateDto.Email, result.Email);
        }
        [Test]
        public async Task GetCandidateByEmailAsync_CandidateDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockCandidateRepository.Setup(repo => repo.GetCandidateByEmailAsync(It.IsAny<string>()))
                                  .ReturnsAsync((Candidate)null);

            // Act
            var result = await _candidateService.GetCandidateByEmailAsync("nonexistent-email@example.com");

            // Assert
            ClassicAssert.IsNull(result);
        }
        [Test]
        public void CreateUpdateCandidateAsync_InvalidEmail_ThrowsArgumentException()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "invalid-email",
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _candidateService.CreateUpdateCandidateAsync(candidateDto));
        }
        [Test]
        public void CreateUpdateCandidateAsync_EmptyCandidateDto_ThrowsArgumentException()
        {
            // Arrange
            var candidateDto = new CandidateDto();
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _candidateService.CreateUpdateCandidateAsync(candidateDto));
            Assert.That(ex.Message, Does.StartWith("Candidate DTO cannot be null or empty"));
        }
        [Test]
        public void CreateUpdateCandidateAsync_EmptyEmail_ThrowsArgumentException()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
            };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _candidateService.CreateUpdateCandidateAsync(candidateDto));
            Assert.That(ex.Message, Does.StartWith("Candidate DTO cannot be null or empty"));
        }
        [Test]
        public async Task CreateUpdateCandidateAsync_UpdatesExistingCandidate_WhenCandidateExists()
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

            var existingCandidate = new Candidate { Email = candidateDto.Email };

            _mockCandidateRepository
                .Setup(repo => repo.GetCandidateByEmailAsync(candidateDto.Email))
                .ReturnsAsync(existingCandidate);
            _mockCandidateRepository.Setup(repo => repo.UpdateCandidate(It.IsAny<Candidate>()));
            _mockCandidateRepository.Setup(repo => repo.SaveAllAsync()).ReturnsAsync(true);

            // Act
            var result = await _candidateService.CreateUpdateCandidateAsync(candidateDto);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(candidateDto.Email, result.Email);
            ClassicAssert.AreEqual(candidateDto.FirstName, result.FirstName);
            ClassicAssert.AreEqual(candidateDto.LastName, result.LastName);
            ClassicAssert.AreEqual(candidateDto.PhoneNumber, result.PhoneNumber);
            ClassicAssert.AreEqual(candidateDto.PreferredCallTime, result.PreferredCallTime);
            ClassicAssert.AreEqual(candidateDto.LinkedInProfileUrl, result.LinkedInProfileUrl);
            ClassicAssert.AreEqual(candidateDto.GitHubProfileUrl, result.GitHubProfileUrl);
            ClassicAssert.AreEqual(candidateDto.Comment, result.Comment);

            _mockCandidateRepository
                .Verify(repo => repo.UpdateCandidate(It.Is<Candidate>(c => c.Email == candidateDto.Email)), Times.Once);
        }
        [Test]
        public async Task CreateUpdateCandidateAsync_AddNewCandidate_WhenCandidateDoesNotExists()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                Email = "newcandidate@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                PreferredCallTime = new TimeSpan(14, 0, 0),
                LinkedInProfileUrl = "https://linkedin.com/in/johndoe",
                GitHubProfileUrl = "https://github.com/johndoe",
                Comment = "New candidate."
            };
            _mockCandidateRepository
                .Setup(repo => repo.GetCandidateByEmailAsync(candidateDto.Email)).ReturnsAsync((Candidate)null);

            _mockCandidateRepository.Setup(repo => repo.AddCandidateAsync(It.IsAny<Candidate>()));
            _mockCandidateRepository.Setup(repo => repo.SaveAllAsync()).ReturnsAsync(true);

            // Act
            var result = await _candidateService.CreateUpdateCandidateAsync(candidateDto);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(candidateDto.Email, result.Email);
            ClassicAssert.AreEqual(candidateDto.FirstName, result.FirstName);
            ClassicAssert.AreEqual(candidateDto.LastName, result.LastName);
            ClassicAssert.AreEqual(candidateDto.PhoneNumber, result.PhoneNumber);
            ClassicAssert.AreEqual(candidateDto.PreferredCallTime, result.PreferredCallTime);
            ClassicAssert.AreEqual(candidateDto.LinkedInProfileUrl, result.LinkedInProfileUrl);
            ClassicAssert.AreEqual(candidateDto.GitHubProfileUrl, result.GitHubProfileUrl);
            ClassicAssert.AreEqual(candidateDto.Comment, result.Comment);

            _mockCandidateRepository
                .Verify(repo => repo.AddCandidateAsync(It.Is<Candidate>(c => c.Email == candidateDto.Email)), Times.Once);
        }
    }
}
