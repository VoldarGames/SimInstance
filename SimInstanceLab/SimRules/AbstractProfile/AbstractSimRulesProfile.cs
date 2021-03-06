using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimRules.AbstractProfile
{
    public abstract class AbstractSimRulesProfile<T> : IAbstractSimRulesProfile where T : new()
    {
        public List<ISimRule> SimRules { get; set; } = new List<ISimRule>();


        public void SimRuleFor<TReturnType>(Expression<Func<T, TReturnType>> func, SimAttribute simAttribute)
        {
            MemberExpression operand;

            var body = func.Body as UnaryExpression;
            if (body != null)
            {
                operand = body.Operand as MemberExpression;
            }
            else
            {
                operand = func.Body as MemberExpression;
            }
            if (operand == null) throw new NullReferenceException("SimRuleFor Operand Expression null.");

            SimRules.Add(new SimRule<T>(operand.Member.Name, func.ReturnType, simAttribute));

        }



    }
}