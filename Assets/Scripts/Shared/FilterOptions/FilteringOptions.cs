using UnityEngine;

namespace Assets.Scripts.Shared.FilterOptions
{
    public abstract class FilteringOptions
    {

    }

    public class SpireFilteringOptions : FilteringOptions
    {
        public string Prefix { get; set; }
    }
}