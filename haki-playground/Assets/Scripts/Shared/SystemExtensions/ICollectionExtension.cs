using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shared.SystemExtensions
{
    public static class ICollectionExtension
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static bool IsNotEmpty<T>(this ICollection<T> collection)
        {
            return collection.IsEmpty() == false;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Any();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsEmpty() == false;
        }
    }
}