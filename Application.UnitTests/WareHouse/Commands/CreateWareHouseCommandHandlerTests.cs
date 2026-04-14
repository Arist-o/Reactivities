using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using AutoMapper;
using Application.Interfaces;
using Application.Report.Commands.WareHouse;
using Application.Report.DTOs.WareHouse;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.WareHouse.Commands
{
    public class WareHouseHandlerTests
    {
        private readonly Mock<IAppDbContext> _contextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateWareHouse.Handler _handler;

        public WareHouseHandlerTests()
        {
            _contextMock = new Mock<IAppDbContext>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateWareHouse.Handler(_contextMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenWareHouseIsCreated()
        {
            // --- Arrange ---
            var dto = new WareHouseCreateDto
            {
                description = "Головний склад",
                number = 101
            };

            var command = new CreateWareHouse.Command { wareHouseCreateDto = dto };

            var wareHouseDomain = new Domain.WareHouse
            {
                Id = Guid.NewGuid(),
                description = dto.description, 
                number = dto.number
            };

            _mapperMock.Setup(m => m.Map<Domain.WareHouse>(It.IsAny<WareHouseCreateDto>()))
                       .Returns(wareHouseDomain);

            var wareHouseList = new List<Domain.WareHouse>();
            var wareHouseData = wareHouseList.BuildMockDbSet();

            _contextMock.Setup(c => c.WareHouses).Returns(wareHouseData.Object);

            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

            // --- Act ---
            var result = await _handler.Handle(command, CancellationToken.None);

            // --- Assert ---
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(wareHouseDomain.Id);

            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}