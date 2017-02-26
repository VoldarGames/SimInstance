using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimInstance
{
    public abstract class AbstractSimRulesProfile<T> where T : new()
    {
        public List<SimRule<T>> SimRules = new List<SimRule<T>>();
        public AbstractSimRulesProfile<T> RuleFor(Expression<Func<T, object>> func, SimAttribute simAttribute)
        {
            MemberExpression operand;

            var body =  func.Body as UnaryExpression;
            if (body != null)
            {
                operand = body.Operand as MemberExpression;
            }
            else
            {
                operand = func.Body as MemberExpression;
            }
            if (operand == null) throw new NullReferenceException("RuleFor Operand Expression null.");
           
            SimRules.Add(new SimRule<T>(operand.Member.Name, func.ReturnType, simAttribute));
            return this;
        }
    }
}