using System;

namespace Secs
{
    public struct Filter
    {
        public readonly Type[] Include;
        public readonly Type[] Exclude;

        private Filter(Type[] include, Type[] exclude)
        {
            Include = include;
            Exclude = exclude;
        }

        public static Filter Create(Type[] include = null, Type[] exclude = null)
        {
            return new Filter(include, exclude);
        }
    }
}