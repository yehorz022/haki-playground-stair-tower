using Assets.Scripts.Services.Converters;
using Assets.Scripts.Services.Instanciation;
using UnityEngine;

namespace Assets.Scripts.Services.Factories
{
    public class PillarFactory
    {
        private readonly IMilimitersToUnityUnitConverter unitConverter;
        private readonly IObjectCacheManager ocm;

        public PillarFactory(IMilimitersToUnityUnitConverter unitConverter, IObjectCacheManager ocm)
        {
            this.unitConverter = unitConverter;
            this.ocm = ocm;
        }

        public void Produce(Transform parent, MonoBehaviour prefab, int lenght, int width)
        {
            float z = unitConverter.Convert(lenght);
            float x = unitConverter.Convert(width);

            for (int loop1 = 0; loop1 < 2; loop1++)
            {
                for (int loop2 = 0; loop2 < 2; loop2++)
                {
                    MonoBehaviour res = ocm.CreateInstance(prefab, parent, Vector3.zero, Quaternion.identity);

                    res.transform.Translate(x * loop1, 0, z * loop2);
                }
            }
        }

    }
}