# Retrieve linked objects

This guide explains how to read the list of linked objects for your given object, using the `ObjectLinker`.

## Prerequisites

- Reference the SDM Object Linking library in your project.
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

## Retrieving linked entity descriptors

You can retrieve linked entities by the ID of your entity, DisplayName or any form of custom filters:

```csharp
// Automation Script, read by SDM Object
public void Run(IEngine engine)
{
    var linker = engine.GetObjectLinker();
    var ticketingHelper = new TicketingApiHelper(engine);
    var ticket = ticketingHelper.Tickets.Read(TicketExposers.ID.Equal("Ticket 00001"));
    var linkedEntities = linker.GetLinkedEntities(ticket);

    foreach(var linkedEntity in linkedEntities)
    {
        Console.WriteLine($"ID: {linkedEntity.ID}, DisplayName: {linkedEntity.DisplayName}, ModelName: {linkedEntity.ModelName}, SolutionName: {linkedEntity.SolutionName}");
    }
}

// Protocol, read by Id
public static void Run(SLProtocol protocol)
{
    var linker = protocol.GetObjectLinker();
    var linkedEntities = linker.GetLinkedEntities("my-identifier");

    foreach(var linkedEntity in linkedEntities)
    {
        Console.WriteLine($"ID: {linkedEntity.ID}, DisplayName: {linkedEntity.DisplayName}, ModelName: {linkedEntity.ModelName}, SolutionName: {linkedEntity.SolutionName}");
    }
}

// GQI, read using custom filter
public OnInitOutputArgs OnInit(OnInitInputArgs args)
{
	var linker = args.DMS.GetObjectLinker();
    var solutions = linker.Links.Read(
        LinkExposers.EntityDescriptors.SolutionName.Equal("solution_a")
    );

    foreach(var linkedEntity in linkedEntities)
    {
        args.Logger.Information($"ID: {linkedEntity.ID}, DisplayName: {linkedEntity.DisplayName}, ModelName: {linkedEntity.ModelName}, SolutionName: {linkedEntity.SolutionName}");
    }
}
```