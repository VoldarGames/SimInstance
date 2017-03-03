using System;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimAttributes
{
    public class SimRegexAttribute : SimAttribute
    {
        public string RegularExpression { get; set; }
        /// <summary>
        /// This attribute will generate a random string based on the regularExpression.
        /// </summary>
        /// <param name="regularExpression"></param>
        public SimRegexAttribute(string regularExpression)
        {
            RegularExpression = regularExpression;
        }
        public override Type[] GetParameterTypes()
        {
            return new[] {typeof(string)};
        }

        public override object[] GetParameterValues()
        {
            return new object[] {RegularExpression};
        }
    }
}