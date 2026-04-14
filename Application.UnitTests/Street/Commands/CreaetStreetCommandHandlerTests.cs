using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using AutoMapper;
using Application.Interfaces;
using Application.Report.Commands.Street;
using Application.Report.DTOs.Street;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tests.Report.Commands.Street
{
    public class StreetHandlerTests
    {
        private readonly Mock<IAppDbContext> _contextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateStreet.Handler _handler;

        public StreetHandlerTests()
        {
            _contextMock = new Mock<IAppDbContext>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateStreet.Handler(_contextMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenStreetIsCreated()
        {
            // --- Arrange ---
            var dto = new StreetCreateDto
            {
                description = "Центральна вулиця"
            };

            var command = new CreateStreet.Command { streetCreateDto = dto };

            var streetDomain = new Domain.Street
            {
                Id = Guid.NewGuid(),
                description = dto.description
            };

            _mapperMock.Setup(m => m.Map<Domain.Street>(It.IsAny<StreetCreateDto>()))
                       .Returns(streetDomain);

            var streetsList = new List<Domain.Street>();

            var streetsData = streetsList.BuildMockDbSet<Domain.Street>();

            _contextMock.Setup(c => c.Streets).Returns(streetsData.Object);

            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

            // --- Act ---
            var result = await _handler.Handle(command, CancellationToken.None);

            // --- Assert ---
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(streetDomain.Id);

            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}