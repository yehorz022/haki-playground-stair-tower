using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Converters;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Metrics.Metric_Units;
using UnityEngine;

namespace Assets.Scripts.Services.Factories
{
    [Service(typeof(IPillarFactory))]
    public class PillarFactory : FactoryBase, IPillarFactory
    {
        private readonly IConverter<MilliMeter, Native> unitConverter;

        public PillarFactory(IConverter<MilliMeter, Native> unitConverter, IObjectCacheManager objectCacheManager, IComponentHolder componentHolder) : base(componentHolder, objectCacheManager)
        {
            this.unitConverter = unitConverter;
        }

        public void Produce(Transform parent, HakiComponent prefab, int length, int width)
        {
            float z = unitConverter.Convert(length);
            float x = unitConverter.Convert(width);

            for (int loop1 = 0; loop1 < 2; loop1++)
            {
                for (int loop2 = 0; loop2 < 2; loop2++)
                {
                    HakiComponent res = Produce(prefab, parent.transform);
                    res.transform.localPosition = new Vector3(x * loop1, 0, z * loop2);
                    res.transform.rotation = Quaternion.identity;
                }
            }
        }
    }
}