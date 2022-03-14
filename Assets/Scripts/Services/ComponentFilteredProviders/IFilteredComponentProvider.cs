using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.FilterOptions;
using UnityEngine;

namespace Assets.Scripts.Services.ComponentFilteredProviders
{
    public interface IFilteredComponentProvider<in T> where T : FilteringOptions
    {
        IEnumerable<HakiComponent> Filter(T data);
    }


    [Service(typeof(IFilteredComponentProvider<SpireFilteringOptions>))]
    public class SpireComponentProvider : IFilteredComponentProvider<SpireFilteringOptions>
    {
        private readonly IComponentHolder holder;

        public SpireComponentProvider(IComponentHolder holder)
        {
            this.holder = holder;
        }

        public IEnumerable<HakiComponent> Filter(SpireFilteringOptions data)
        {
            return holder.Enumerate().Where(component => FilterIndividual(component, data));
        }

        private static bool FilterIndividual (Object component,SpireFilteringOptions filteringOptions)
        {
            return component.name.StartsWith(filteringOptions.Prefix);
        }
    }
}