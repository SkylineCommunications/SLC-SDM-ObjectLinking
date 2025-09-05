# Registering Solutions and Models with SdmRegistrar

This guide explains how to use the `ObjectLinker` to link 2 object together.

## Prerequisites

- Reference the SDM Object Linking library in your project (e.g., the `Skyline.DataMiner.SDM.ObjectLinking.Protocol` nuget package for use in a Connector)..
- Obtain a DataMiner connection and the `ObjectLinker` instance.

## Linker Entrypoint

The entry point is the `ObjectLinker` class, which you can obtain from your DataMiner connection:

```csharp
// Automation Script
public void Run(IEngine engine)
{
    var linker = engine.GetObjectLinker();
}

// Protocol
public static void Run(SLProtocol protocol)
{
    var linker = protocol.GetObjectLinker();
}

// GQI
public OnInitOutputArgs OnInit(OnInitInputArgs args)
{
	var linker = args.DMS.GetObjectLinker();
}
```

## Creating the link

After you have you're linker you can start creating links between objects:

1. Create descriptors that will uniquely identify your objects.
1. Use the linker to create a link between them.

```csharp
var entity1 = new EntityDescriptor
{
	/* Required */  ID = "resource1",
	/* Required */  DisplayName = "Resource #1",
	/* Optional */  ModelName = "Resource",
	/* Optional */  SolutionName = "resource_studio",
	/* Optional */  ParentID = "resource_pool_a",
	/* Optional */  ParentModelName = "Resource Pool",
};

var entity2 = new EntityDescriptor
{
    /* Required */  ID = "entity2",
    /* Required */  DisplayName = "Entity 2",
    /* Optional */  ModelName = "Model B",
    /* Optional */  SolutionName = "Solution Y",
    /* Optional */  ParentID = "parent2",
    /* Optional */  ParentModelName = "Parent Model B",
};

linker.Create(entity1, entity2);

```