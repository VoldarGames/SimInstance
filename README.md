![SimInstanceLab](SimInstanceLabIcon.png)
# SimInstance

An small simulation tool for .NET that uses attributes, fluent interface and lambda expressions

Created and written by Dani Garcia Sanchez (https://www.linkedin.com/in/dani-garcia-sanchez-30b684138)

## Introduction
SimInstanceLab is a powerful tool that allows programmers to create lots of instances based on some rules.

    For example:

    ·This instance property must be on a Range or can be null or both
  
    ·This instance property is a foreign key of another type of instance
  
All this rules can be added in a fluent manner with profiles or directly added on your model domain with SimAttributes.

    Once you have create tons of randomly generated instances (based on your rules) you can:

    ·Redirect those instance list to pass your own UnitTests
  
    ·Export to JSON all instances
  
    ·Serialize instances into a file
  
## How to use

In Sim Instance Lab you can choose two ways to generate your instances
  
### Attributes
    
Decorate your model domain entities with SimAttributes

```csharp
    public class DecoratedOneIntClass
    {
        [SimRange(0,10)]
        public int MyInt { get; set; }
    }
```

Create the number of instances you want!
```csharp   
    var generatedInstances = SimInstanceManager.GenerateInstances<DecoratedOneIntClass>(10);
```

### Profiles

First you need to create a profile, just inherit from "AbstractSimRulesProfile<>" and define the rules on default constructor
```csharp
    public class OneIntClassSimRulesProfile : AbstractSimRulesProfile<OneIntClass>
    {
        public OneIntClassSimRulesProfile()
        {
            SimRuleFor(c => c.MyInt, new SimRangeAttribute(0, 100));
        }        
    }
```
Next create the number of instances you want! (You need to add the profile)

```csharp
    SimRulesProfileManager.AddProfile<OneIntClass>(new OneIntClassSimRulesProfile());
    var generatedInstances = SimInstanceManager.GenerateInstancesWithRules<OneIntClass>(numberOfInstances);
```

### Providers

Implement your own provider with this interface

```csharp
public interface ISimProvider
    {
        void Add(Type type, List<object> entities);
        object GetById(Type type, int id);
        bool ContainsKey(Type propertyType);
        List<object> GetAll(Type type);
        int GetCount(Type type);
    }
```

### Define and configure Stages

```csharp
public class AgentsStage : Stage
    {
        public AgentsStage()
        {
            this
                .UseSeed((int) DateTime.Now.Ticks)
                .ForceAction(() =>
                {
                    PrimitiveOrClassHelper.PrimitiveTypes.Add(typeof(ObjectChangeTracker));
                })
                .UseProfile(new AgentSimRulesProfile(), forcePrimaryKey: true)
                .UseProfile(new ClientsSimRulesProfile(), forcePrimaryKey: true)
                .UseProvider(new MyAwesomeProvider())
                .IgnoreAllTypes(typeof(ObjectChangeTracker), typeof(IEnumerable<>));

        }
    }
```

