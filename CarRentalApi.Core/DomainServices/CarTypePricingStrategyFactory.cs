using CarRentalApi.Core.Enums;
using CarRentalApi.Core.PricingStrategies;

namespace CarRentalApi.Core.DomainServices;

public class CarTypePricingStrategyFactory
{
    private readonly IReadOnlyDictionary<CarTypeEnum, ICarTypePricingStrategy> _strategies;

    public CarTypePricingStrategyFactory(IReadOnlyDictionary<CarTypeEnum, ICarTypePricingStrategy> strategies)
    {
        _strategies = strategies;
    }

    public ICarTypePricingStrategy GetStrategy(CarTypeEnum carType)
    {
        if (_strategies.TryGetValue(carType, out var strategy))
            return strategy;

        throw new NotSupportedException($"No pricing strategy registered for car type {carType}");
    }
}
