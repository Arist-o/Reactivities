using Application.Report.Commands.WareHouse;
using Application.Report.DTOs.WareHouse;
using AutoMapper;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Testcontainers.MsSql;
using Xunit;

namespace Application.Tests.Integration.Report.Commands
{
    // IAsyncLifetime дозволяє запускати і зупиняти контейнер до/після всіх тестів у класі
    public class CreateWareHouseIntegrationTests : IAsyncLifetime
    {
        // Налаштовуємо контейнер SQL Server
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        private AppDbContext _context = null!;
        private readonly Mock<IMapper> _mapperMock = new();

        public async Task InitializeAsync()
        {
            // 1. Запускаємо контейнер
            await _msSqlContainer.StartAsync();

            // 2. Налаштовуємо DbContext на підключення до контейнера
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .Options;

            _context = new AppDbContext(options);

            // 3. Створюємо схему БД (альтернатива міграціям для тестів)
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            // Після виконання тестів видаляємо базу і зупиняємо контейнер
            await _context.DisposeAsync();
            await _msSqlContainer.DisposeAsync();
        }

        [Fact]
        public async Task Handle_Should_CreateWareHouse_InRealDatabase()
        {
            // --- Arrange ---
            // Оскільки WareHouse має CityId (зовнішній ключ), нам треба спочатку створити місто
            var city = new City { Id = Guid.NewGuid(), description = "Київ" };
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            var dto = new WareHouseCreateDto
            {
                description = "Головний склад",
                number = 101
            };

            var command = new CreateWareHouse.Command { wareHouseCreateDto = dto };

            var wareHouseDomain = new WareHouse
            {
                Id = Guid.NewGuid(),
                description = dto.description,
                number = dto.number,
                CityId = city.Id // Прив'язуємо до існуючого міста
            };

            _mapperMock.Setup(m => m.Map<WareHouse>(It.IsAny<WareHouseCreateDto>()))
                       .Returns(wareHouseDomain);

            var handler = new CreateWareHouse.Handler(_context, _mapperMock.Object);

            // --- Act ---
            var result = await handler.Handle(command, CancellationToken.None);

            // --- Assert ---
            result.IsSuccess.Should().BeTrue();

            // Перевіряємо, чи дійсно запис з'явився у реальній БД
            var savedWareHouse = await _context.WareHouses.FirstOrDefaultAsync(w => w.Id == result.Value);

            savedWareHouse.Should().NotBeNull();
            savedWareHouse!.description.Should().Be("Головний склад");
            savedWareHouse.number.Should().Be(101);
            savedWareHouse.CityId.Should().Be(city.Id);
        }
    }
}