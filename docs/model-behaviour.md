# Generated model behaviour

The models use `XmlSerializer`. The bundled analyzer checks XML choice groups when you compile consumer code. It does not run when you serialize an object.

## Construct choice groups

Select an alternative for each required choice group in the object initializer. You can omit optional properties and assign them later.

`XCGA001` reports assignments to conflicting alternatives. `XCGA002` reports a missing required alternative and lists the properties you can choose. A sequence inside a choice can require several properties from the same alternative.

For example, SIRI coordinates use the same choice alternative for longitude and latitude:

```csharp
using Spillgebees.SIRI.Models.V2_2.SIRI;

var location = new LocationStructure
{
    Longitude = 6,
    Latitude = 49,
};
```

The analyzer checks exclusivity at each assignment. To switch between nullable reference alternatives, clear the old property before assigning the new one. A nillable property can serialize `null` as `xsi:nil`, so setting it to `null` does not necessarily remove that alternative from XML.

If your construction pattern must fill required choices after the initializer, suppress `XCGA002` at that location. Suppression accepts responsibility for completing the object before serialization.

## Preserve default-value presence

An untouched optional scalar property returns its XSD default but stays out of XML. Assigning a value marks it as present, even when that value equals the default.

```csharp
var journey = new MonitoredVehicleJourneyStructure();
journey.Monitored = true;
```

The tracking state is private. There is no public `MonitoredSpecified` property. Deserialization preserves presence through the setter. Copying property values into another object counts as explicit assignment.

Object-valued defaults retain their existing serialization behaviour. For example, NeTEx `Note` serializes its default object even when the `Note` setter was not called. This preserves edits through `Note.Value` or `Note.Lang`, which scalar assignment tracking cannot observe.

This getter behaviour is a model convenience. In XSD, an absent defaulted element and an empty defaulted element have different meanings. The [XML Schema primer](https://www.w3.org/TR/xmlschema-0/#OccurrenceConstraints) explains element and attribute defaults.

## Include fixed values

Fixed-value properties are getter-only. Required fixed elements and attributes serialize automatically.

For optional fixed elements, call the generated inclusion method:

```csharp
var filtering = new SituationExchangeServiceCapabilitiesStructureTopicFiltering
{
    DefaultPreviewInterval = TimeSpan.FromHours(1),
};
filtering.IncludeFilterByLocationRef();
```

The serializer uses a separate public property hidden from IntelliSense. That property accepts the fixed value and rejects other values when reading XML. Application code should use the getter and inclusion method.

XML token lists also use hidden serialization properties. For example, SIRI `v2.3` exposes `Keywords` as `List<string>` and writes it as one whitespace-separated XML element.

## Validate XML at the boundary

The analyzer checks selected construction patterns. It cannot prove that every object produces schema-valid XML. Empty collections, mutations through helpers, and relationships between separate conditional branches can exceed its analysis.

Repeated choices allow different alternatives on separate repetitions. Some more complex repeated sequences cannot preserve their original interleaving in separate generated collections. Validate the XML against the applicable upstream XSD when that distinction matters to your exchange.
