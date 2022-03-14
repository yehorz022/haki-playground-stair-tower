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
    [Service(typeof(IDeckFactory))]
    public class Deckfactory : FactoryBase, IDeckFactory
    {
        private readonly IConverter<MilliMeter, Native> unitConverter;

        public Deckfactory(IObjectCacheManager objectCacheManager, IConverter<MilliMeter, Native> unitConverter, IComponentHolder componentHolder) : base(componentHolder, objectCacheManager)
        {
            this.unitConverter = unitConverter;
        }

        public void Produce(Transform parent, HakiComponent prefab, int length, int width, int elementWidth, int level)
        {
            int number = width / elementWidth;
            float offset = unitConverter.Convert(width % elementWidth) / 2f;
            float halfEleWidth = unitConverter.Convert(elementWidth / 2);
            float elementWithUnity = unitConverter.Convert(elementWidth);

            float y = unitConverter.Convert(Constants.SpireInitialOffset + level * Constants.SpireSpacingBetweenPocketGroups)
                      + unitConverter.Convert(Constants.DeckVerticalOffset);


            Quaternion rotation = Quaternion.identity;
            for (int i = 0; i < number; i++)
            {
                HakiComponent hc = Produce(prefab, parent.transform);
                hc.transform.localPosition = new Vector3(0, y, offset + (halfEleWidth + elementWithUnity * i));
                hc.transform.rotation = rotation;
            }
        }
    }
}