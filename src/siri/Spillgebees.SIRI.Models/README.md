# Spillgebees.SIRI.Models

C# XML bindings for [SIRI](https://github.com/TransmodelEcosystem/SIRI), generated from the upstream XSD schemas. Requires .NET `10.0`.

## Install

Choose the schema version your data provider uses:

```bash
dotnet add package Spillgebees.SIRI.Models.V2_2
```

To reference all supported versions, install `Spillgebees.SIRI.Models` instead. Each version has a separate C# namespace.

## Available versions

The version in each package name identifies the schema, not the NuGet release.

| SIRI version | Package | NuGet |
| --- | --- | --- |
| `v2.1` | `Spillgebees.SIRI.Models.V2_1` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_1?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_1) |
| `v2.2` | `Spillgebees.SIRI.Models.V2_2` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_2?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_2) |
| `v2.2.1` | `Spillgebees.SIRI.Models.V2_2_1` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_2_1?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_2_1) |
| `v2.3` | `Spillgebees.SIRI.Models.V2_3` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_3?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_3) |
| All versions | `Spillgebees.SIRI.Models` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models) |

## Serialize a service delivery

This example writes the delivery header to a UTF-8 XML file. Add the payload required by your data provider before sending it.

```csharp
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Spillgebees.SIRI.Models.V2_2.SIRI;

var siri = new Siri
{
    ServiceDelivery = new ServiceDelivery
    {
        ResponseTimestamp = DateTimeOffset.UtcNow,
    },
};

var serializer = new XmlSerializer(typeof(Siri));
using var stream = File.Create("siri.xml");
using var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
{
    Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
    Indent = true,
});
serializer.Serialize(xmlWriter, siri);
```

## Namespaces

The package name is the root namespace. For example, `Spillgebees.SIRI.Models.V2_2.SIRI` contains the `Siri` type used above.

| Sub-namespace | XML namespace | Description |
| --- | --- | --- |
| `.SIRI` | `http://www.siri.org.uk/siri` | Core SIRI types |
| `.IFOPT` | `http://www.ifopt.org.uk/ifopt` | IFOPT types |
| `.ACSB` | `http://www.ifopt.org.uk/acsb` | Accessibility types |
| `.DATEX2` | `http://datex2.eu/schema/2_0RC1/2_0` | DATEX2 types |
| `.WSDL` | `http://wsdl.siri.org.uk` | WSDL/SOAP types |
| `.GML` | `http://www.opengis.net/gml/3.2` | Geographic markup types |
| `.W3` | `http://www.w3.org/XML/1998/namespace` | W3 types |

See [generated model behaviour](https://github.com/Spillgebees/Transmodel/blob/main/docs/model-behaviour.md) for choice diagnostics, default-value presence, and fixed values.

## Custom generation

Use [`Spillgebees.Transmodel.Generator`](https://github.com/Spillgebees/Transmodel/blob/main/src/generator/Spillgebees.Transmodel.Generator/README.md) for a custom namespace or a different schema tag, branch, or commit.

## License

[EUPL-1.2](https://github.com/Spillgebees/Transmodel/blob/main/LICENSE). See the [SIRI repository](https://github.com/TransmodelEcosystem/SIRI) for upstream schema licensing information.
