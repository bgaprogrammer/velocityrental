using CarRentalApi.Core.PricingStrategies;
using CarRentalApi.Core.DomainServices;
using CarRentalApi.Core.Repositories;
using CarRentalApi.Core.Enums;
using Moq;

namespace CarRentalApi.Core.Tests.DomainServices;

/// <summary>
/// Base class for RentalAppService tests. 
/// Provides common mocks and service setup. :)
/// </summary>
public abstract class RentalAppServiceTestBase
{
    public readonly Mock<IRentalRepository> RentalRepoMock;
    public readonly Mock<ICarRepository> CarRepoMock;
    public readonly Mock<ICustomerRepository> CustomerRepoMock;
    public readonly Mock<ICarTypePricingRepository> PricingRepoMock;
    public readonly CarTypePricingStrategyFactory PricingStrategyFactory;
    public readonly RentalAppService Service;

    protected RentalAppServiceTestBase()
    {
        // Arrange all mocks as variables
        RentalRepoMock = new Mock<IRentalRepository>();
        CarRepoMock = new Mock<ICarRepository>();
        CustomerRepoMock = new Mock<ICustomerRepository>();
        PricingRepoMock = new Mock<ICarTypePricingRepository>();

        // Register all strategies
        var strategies = new Dictionary<CarTypeEnum, ICarTypePricingStrategy>
        {
            [CarTypeEnum.Premium] = new PremiumCarPricingStrategy(),
            [CarTypeEnum.SUV] = new SuvCarPricingStrategy(PricingRepoMock.Object),
            [CarTypeEnum.Small] = new SmallCarPricingStrategy()
        };

        // Create the factory with the strategies
        PricingStrategyFactory = new CarTypePricingStrategyFactory(strategies);

        // Create the service with all the mocks and the factory
        Service = new RentalAppService(
            RentalRepoMock.Object,
            CarRepoMock.Object,
            CustomerRepoMock.Object,
            PricingRepoMock.Object,
            PricingStrategyFactory);
    }
}
