using System;
using System.Linq;
using System.Reflection;
using SimInstanceLab.Managers;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimRules.PrimaryKeyMap;
using SimInstanceLab.SimRules.RuleAppliers.BaseClass;

namespace SimInstanceLab.SimRules.RuleAppliers
{
    public class SimForeignKeyRuleApplier<T> : SimRuleApplier<T>
    {
        public override void ApplyRule(PropertyInfo property, ref T newEntity, SimAttribute simAttribute)
        {
            var typeInSimForeignKeyAttribute = simAttribute.GetParameterValues().FirstOrDefault() as Type;
            var allObjectsOfTypeInForeignKey = SimContainer.Container.GetAll(typeInSimForeignKeyAttribute);
            var theTypeChosen = allObjectsOfTypeInForeignKey[GetRandomRange(0, allObjectsOfTypeInForeignKey.Count - 1)];

            //PRIMARY KEY TYPES AND PROPERTY STRINGS! IN DICTIONARY !!!!!
            //GENERALO CUANDO ESTES GESTIONANDO EL PROFILE!
            //APPLIER DE PK !!!

            var primaryKeyPropertyName = SimPrimaryKeyMap.GetPrimaryKeyPropertyName(typeInSimForeignKeyAttribute);

            var primaryKeyValue = theTypeChosen.GetType().GetProperty(primaryKeyPropertyName).GetValue(theTypeChosen);

            property.SetValue(newEntity, primaryKeyValue);
        }
        private static int GetRandomRange(int min, int max)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(min, max);
        }
    }
}