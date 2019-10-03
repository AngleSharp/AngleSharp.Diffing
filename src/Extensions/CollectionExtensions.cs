using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.AngleSharp.Diffing.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (source is null) throw new ArgumentNullException(nameof(source));

            foreach (var item in source)
            {
                target.Add(item);
            }
        }
    }
}
