# Spillgebees.NeTEx.Models

C# XML bindings for [NeTEx](https://github.com/TransmodelEcosystem/NeTEx), generated from the upstream XSD schemas. Requires .NET `10.0`.

## Install

Choose the schema version your data provider uses:

```bash
dotnet add package Spillgebees.NeTEx.Models.V1_3_1
```

To reference all supported versions, install `Spillgebees.NeTEx.Models` instead. Each version has a separate C# namespace.

## Available versions

The version in each package name identifies the schema, not the NuGet release.

| NeTEx version | Package | NuGet |
| --- | --- | --- |
| `v1.2` | `Spillgebees.NeTEx.Models.V1_2` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models.V1_2?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models.V1_2) |
| `v1.2.2` | `Spillgebees.NeTEx.Models.V1_2_2` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models.V1_2_2?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models.V1_2_2) |
| `v1.2.3` | `Spillgebees.NeTEx.Models.V1_2_3` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models.V1_2_3?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models.V1_2_3) |
| `v1.3.0` | `Spillgebees.NeTEx.Models.V1_3_0` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models.V1_3_0?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models.V1_3_0) |
| `v1.3.1` | `Spillgebees.NeTEx.Models.V1_3_1` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models.V1_3_1?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models.V1_3_1) |
| `v1.3.2` | `Spillgebees.NeTEx.Models.V1_3_2` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models.V1_3_2?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models.V1_3_2) |
| `v2.0.0` | `Spillgebees.NeTEx.Models.V2_0_0` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models.V2_0_0?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models.V2_0_0) |
| All versions | `Spillgebees.NeTEx.Models` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models?label=nuget)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models) |

## Serialize a publication

This example writes the delivery header to a UTF-8 XML file. Add the payload required by your data provider before sending it.

```csharp
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Spillgebees.NeTEx.Models.V1_3_1.NeTEx;

var delivery = new PublicationDeliveryStructure
{
    PublicationTimestamp = DateTimeOffset.UtcNow,
    ParticipantRef = "my-data-provider",
};

var serializer = new XmlSerializer(typeof(PublicationDeliveryStructure));
using var stream = File.Create("publication.xml");
using var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
{
    Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
    Indent = true,
});
serializer.Serialize(xmlWriter, delivery);
```

## Namespaces

The package name is the root namespace. For example, `Spillgebees.NeTEx.Models.V1_3_1.NeTEx` contains the `PublicationDeliveryStructure` type used above.

| Sub-namespace | XML namespace | Description |
| --- | --- | --- |
| `.NeTEx` | `http://www.netex.org.uk/netex` | NeTEx types |
| `.SIRI` | `http://www.siri.org.uk/siri` | SIRI types bundled with NeTEx |
| `.GML` | `http://www.opengis.net/gml/3.2` | Geographic markup types |

See [generated model behaviour](https://github.com/Spillgebees/Transmodel/blob/main/docs/model-behaviour.md) for choice diagnostics, default-value presence, and fixed values.

## Custom generation

Use [`Spillgebees.Transmodel.Generator`](https://github.com/Spillgebees/Transmodel/blob/main/src/generator/Spillgebees.Transmodel.Generator/README.md) for a custom namespace or a different schema tag, branch, or commit.

## License

[EUPL-1.2](https://github.com/Spillgebees/Transmodel/blob/main/LICENSE). See the [NeTEx repository](https://github.com/TransmodelEcosystem/NeTEx) for upstream schema licensing information.
