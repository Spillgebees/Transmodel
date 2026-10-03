# Spillgebees.Transmodel

[![Build & test](https://github.com/Spillgebees/Transmodel/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/Spillgebees/Transmodel/actions/workflows/build-and-test.yml)
[![NeTEx NuGet](https://img.shields.io/nuget/vpre/Spillgebees.NeTEx.Models?label=NeTEx)](https://www.nuget.org/packages/Spillgebees.NeTEx.Models)
[![SIRI NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models?label=SIRI)](https://www.nuget.org/packages/Spillgebees.SIRI.Models)
[![License](https://img.shields.io/github/license/Spillgebees/Transmodel)](LICENSE)

C# XML bindings for [NeTEx](https://github.com/TransmodelEcosystem/NeTEx) timetable data and [SIRI](https://github.com/TransmodelEcosystem/SIRI) real-time transport data, generated from their XSD schemas. The packages target .NET `10.0` and use `XmlSerializer`.

The project uses AI-assisted development and has not had a full manual review. It is pre-production software.

## Install models

Choose the schema version your data provider uses. For example:

```bash
dotnet add package Spillgebees.NeTEx.Models.V1_3_1
dotnet add package Spillgebees.SIRI.Models.V2_2
```

The version in a package name identifies the upstream schema, not the NuGet release. Each schema version has its own C# namespace, so you can reference several versions in one application. The meta-packages include all versions listed below.

### NeTEx

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

See the [NeTEx usage example and namespaces](src/netex/Spillgebees.NeTEx.Models/README.md#serialize-a-publication).

### SIRI

| SIRI version | Package | NuGet |
| --- | --- | --- |
| `v2.1` | `Spillgebees.SIRI.Models.V2_1` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_1?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_1) |
| `v2.2` | `Spillgebees.SIRI.Models.V2_2` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_2?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_2) |
| `v2.2.1` | `Spillgebees.SIRI.Models.V2_2_1` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_2_1?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_2_1) |
| `v2.3` | `Spillgebees.SIRI.Models.V2_3` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models.V2_3?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models.V2_3) |
| All versions | `Spillgebees.SIRI.Models` | [![NuGet](https://img.shields.io/nuget/vpre/Spillgebees.SIRI.Models?label=nuget)](https://www.nuget.org/packages/Spillgebees.SIRI.Models) |

See the [SIRI usage example and namespaces](src/siri/Spillgebees.SIRI.Models/README.md#serialize-a-service-delivery).

Read [generated model behaviour](docs/model-behaviour.md) for choice diagnostics, default values, and fixed values.

## Generate your own models

Use the generator for a custom namespace or an upstream tag, branch, or commit that has no model package.

```bash
dotnet tool install -g Spillgebees.Transmodel.Generator
transmodel-generator generate-siri --version v2.2 --output ./Generated --namespace MyApp.SIRI
```

The [generator README](src/generator/Spillgebees.Transmodel.Generator/README.md) covers both standards, command options, and schema caching.

## Build from source

Use the .NET SDK selected by `global.json`. From the repository root, run:

```bash
dotnet build Spillgebees.Transmodel.slnx --configuration Release
dotnet test --solution Spillgebees.Transmodel.slnx --configuration Release
```

The first build downloads schemas from GitHub and generates the model classes. Later builds reuse the schema cache. Generated files are ignored by Git.

To regenerate the models, run `dotnet clean Spillgebees.Transmodel.slnx`, then build again. Fix generation problems in the generator or its `XmlSchemaClassGenerator` fork, since the next generation replaces files under `Generated/`.

## License

This project uses [EUPL-1.2](LICENSE). The upstream schemas have their own licensing information in the [NeTEx repository](https://github.com/TransmodelEcosystem/NeTEx) and [SIRI repository](https://github.com/TransmodelEcosystem/SIRI).
