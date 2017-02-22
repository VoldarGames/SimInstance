using System;
using System.Reflection;

namespace SimInstance
{
    public static class SimRangeRule<T>
    {
        public static void ApplySimRangeRule(PropertyInfo property, ref T newEntity)
        {
            var rangeAttribute = property.GetCustomAttribute(typeof(SimRangeAttribute));
            var minRangeValue = rangeAttribute.GetType().GetProperty("MinRange").GetValue(rangeAttribute);
            var maxRangeValue = rangeAttribute.GetType().GetProperty("MaxRange").GetValue(rangeAttribute);
            property.SetValue(newEntity, GetRandomRange(minRangeValue, maxRangeValue));
        }

        public static int GetRandomRange(object min, object max)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next((int)min, (int)max);
        }
    }
}