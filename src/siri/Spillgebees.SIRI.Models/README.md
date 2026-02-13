# Spillgebees.SIRI.Models

Pre-generated C# XML bindings for [SIRI](https://github.com/SIRI-CEN/SIRI) (Service Interface for Real-time Information) XSD schemas.

Each version package provides strongly-typed classes for a specific [SIRI-CEN/SIRI release tag](https://github.com/SIRI-CEN/SIRI/tags). Install only the version you need, or use the meta-package to get all versions at once.

## Available versions

| SIRI version | Package                          |                                                                            NuGet                                                                             |
|:------------:|----------------------------------|:------------------------------------------------------------------------------------------------------------------------------------------------------------:|
|     v2.1     | `Spillgebees.SIRI.Models.V2_1`   | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_1?logo=nuget&label=)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_1)   |
|     v2.2     | `Spillgebees.SIRI.Models.V2_2`   | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_2?logo=nuget&label=)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_2)   |
| All versions | `Spillgebees.SIRI.Models`        |      [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models?logo=nuget&label=)](https://www.nuget.org/packages/Spillgebees.SIRI.Models)        |

## Getting started

```bash
# Install a single version
dotnet add package Spillgebees.SIRI.Models.V2_2

# Or install all versions at once
dotnet add package Spillgebees.SIRI.Models
```

## Usage

Each version package contains seven sub-namespaces:

| Sub-namespace | XML Namespace                          | Description                   |
|---------------|----------------------------------------|-------------------------------|
| `.SIRI`       | `http://www.siri.org.uk/siri`          | Core SIRI types               |
| `.IFOPT`      | `http://www.ifopt.org.uk/ifopt`        | IFOPT types                   |
| `.ACSB`       | `http://www.ifopt.org.uk/acsb`         | Accessibility types           |
| `.DATEX2`     | `http://datex2.eu/schema/2_0RC1/2_0`   | DATEX2 types                  |
| `.WSDL`       | `http://wsdl.siri.org.uk`              | WSDL/SOAP types               |
| `.GML`        | `http://www.opengis.net/gml/3.2`       | GML types (geographic markup) |
| `.W3`         | `http://www.w3.org/XML/1998/namespace` | W3 types                      |

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
using var stream = new MemoryStream();
using var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
{
    Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
    Indent = true,
});
serializer.Serialize(xmlWriter, siri);

// <?xml version="1.0" encoding="utf-8"?>
// <Siri xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ...>
//   <ServiceDelivery>
//     <ResponseTimestamp>2026-02-13T12:00:00+00:00</ResponseTimestamp>
//   </ServiceDelivery>
// </Siri>
```

## Custom generation

If you need full control over namespaces or want to target a different SIRI version, use the [`Spillgebees.Transmodel.Generator`](https://www.nuget.org/packages/Spillgebees.Transmodel.Generator) CLI tool to generate models from the XSD schemas directly.

## License

[EUPL-1.2](https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12). The SIRI schemas are published by CEN on the [SIRI-CEN/SIRI](https://github.com/SIRI-CEN/SIRI) repository without an explicit open-source license.
