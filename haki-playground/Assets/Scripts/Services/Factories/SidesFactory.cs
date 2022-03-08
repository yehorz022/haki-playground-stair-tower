using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Converters;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.Metrics.Metric_Units;
using UnityEngine;

namespace Assets.Scripts.Services.Factories
{
    [Service(typeof(ISidesFactory))]
    public class SidesFactory : FactoryBase, ISidesFactory
    {

        private readonly IConverter<MilliMeter, Native> unitConverter;

        public SidesFactory(IObjectCacheManager objectCacheManager, IConverter<MilliMeter, Native> unitConverter, IComponentHolder componentHolder) : base(componentHolder, objectCacheManager)
        {
            this.unitConverter = unitConverter;
        }


        public void Produce(Transform parent, HakiComponent prefabAlongZ, HakiComponent prefabAlongX, int length, int width, int level)
        {
            float y = unitConverter.Convert(Constants.SpireInitialOffset + level * Constants.SpireSpacingBetweenPocketGroups);
            float x = unitConverter.Convert(width /*+ Constants.SpirePocketOffset * 2*/);
            float z = unitConverter.Convert(length /*+ Constants.SpirePocketOffset * 2*/);

            MonoBehaviour res1 = Produce(prefabAlongX, parent.transform);

            res1.transform.localPosition = new Vector3(0, y, 0);
            res1.transform.rotation = Quaternion.identity;

            MonoBehaviour res2 = Produce(prefabAlongX, parent.transform);

            res2.transform.localPosition = new Vector3(0, y, z);
            res2.transform.rotation = Quaternion.identity;


            Quaternion rotation = Quaternion.AngleAxis(-90, Vector3.up);
            MonoBehaviour res3 = Produce(prefabAlongZ, parent.transform);

            res3.transform.localPosition = new Vector3(0, y, 0);
            res3.transform.rotation = rotation;

            MonoBehaviour res4 = Produce(prefabAlongZ, parent.transform);

            res4.transform.localPosition = new Vector3(x, y, 0);
            res4.transform.rotation = rotation;
        }
    }
}