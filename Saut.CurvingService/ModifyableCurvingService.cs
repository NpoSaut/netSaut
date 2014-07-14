using System.Linq;
using Saut.Interfaces;

namespace Saut.CurvingService
{
    public class ModifyableCurvingService : ICurvingService
    {
        private readonly ICurveAggregator _aggregator;
        private readonly ICurveComposer _composer;
        private readonly IModifiersService _modifiersService;
        private readonly ICurveProvider[] _providers;

        public ModifyableCurvingService(ICurveProvider[] Providers, IModifiersService ModifiersService, ICurveAggregator Aggregator, ICurveComposer Composer)
        {
            _providers = Providers;
            _modifiersService = ModifiersService;
            _aggregator = Aggregator;
            _composer = Composer;
        }

        public ICurve ProcessCurve()
        {
            return
                _composer.Compose(
                    _aggregator.Aggregate(_providers)
                               .Select(layer => _modifiersService.ModifyLayerCurve(layer.Kind,
                                                                                   _composer.Compose(layer.Curves))));
        }
    }
}
