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
            
            var body = ((System.Linq.Expressions.UnaryExpression) func.Body);
            var operand = body.Operand as MemberExpression;
            if (operand == null) throw new NullReferenceException("RuleFor Operand Expression null.");
           
            SimRules.Add(new SimRule<T>(operand.Member.Name, func.ReturnType, simAttribute));
            return this;
        }
    }
}