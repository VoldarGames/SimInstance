using System;

namespace SimInstance
{
    public class SimRangeAttribute : SimAttribute
    {
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        /// <summary>
        /// [min,max]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        
        public SimRangeAttribute(int min, int max)
        {
            MinRange = min;
            MaxRange = max;
        }

        public override Type[] GetParameterTypes()
        {
            return new[] {typeof(int), typeof(int)};
        }

        public override object[] GetParameterValues()
        {
            return new object[] {MinRange, MaxRange};
        }
    }
}