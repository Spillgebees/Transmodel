# Spillgebees.Transmodel.Generator

Generate C# XML bindings from [NeTEx](https://github.com/TransmodelEcosystem/NeTEx) and [SIRI](https://github.com/TransmodelEcosystem/SIRI) XSD schemas. Requires .NET `10.0`.

The tool downloads schemas from the upstream repositories. Use a release tag, branch, or commit SHA to select the schemas.

## Install

Install the tool globally:

```bash
dotnet tool install -g Spillgebees.Transmodel.Generator
```

## Generate NeTEx models

Generate bindings for a specific NeTEx version tag:

```bash
transmodel-generator generate-netex --version v2.0.0 --output ./Generated --namespace MyApp.NeTEx
```

The root namespace gets these suffixes:

| Sub-namespace | XML namespace | Description |
| --- | --- | --- |
| `MyApp.NeTEx.NeTEx` | `http://www.netex.org.uk/netex` | NeTEx types |
| `MyApp.NeTEx.SIRI` | `http://www.siri.org.uk/siri` | SIRI types bundled with NeTEx |
| `MyApp.NeTEx.GML` | `http://www.opengis.net/gml/3.2` | Geographic markup types |

## Generate SIRI models

Generate bindings for a specific SIRI version tag:

```bash
transmodel-generator generate-siri --version v2.3 --output ./Generated --namespace MyApp.SIRI
```

The root namespace gets these suffixes:

| Sub-namespace | XML namespace | Description |
| --- | --- | --- |
| `MyApp.SIRI.SIRI` | `http://www.siri.org.uk/siri` | Core SIRI types |
| `MyApp.SIRI.IFOPT` | `http://www.ifopt.org.uk/ifopt` | IFOPT types |
| `MyApp.SIRI.ACSB` | `http://www.ifopt.org.uk/acsb` | Accessibility types |
| `MyApp.SIRI.DATEX2` | `http://datex2.eu/schema/2_0RC1/2_0` | DATEX2 types |
| `MyApp.SIRI.WSDL` | `http://wsdl.siri.org.uk` | WSDL/SOAP types |
| `MyApp.SIRI.GML` | `http://www.opengis.net/gml/3.2` | Geographic markup types |
| `MyApp.SIRI.W3` | `http://www.w3.org/XML/1998/namespace` | W3 types |

## Schema caching

The tool reuses cached schemas for version tags and commit SHAs. Branch refs download on every run. To refresh a cached tag, delete its directory from the schema cache before running the command again.

The cache uses the `netex-schemas` and `siri-schemas` directories under .NET's `Environment.SpecialFolder.LocalApplicationData`. On Linux, this is usually `~/.local/share`. On Windows, it is `%LOCALAPPDATA%`.

## CLI reference

```
transmodel-generator generate-netex [options]
transmodel-generator generate-siri  [options]

Options:
  -v, --version <version>      Schema version tag (default: v2.0.0 for NeTEx, v2.3 for SIRI)
  --ref <ref>                  Git ref (branch or commit SHA), mutually exclusive with --version
  -o, --output <output>        Output directory for generated C# files (default: ./Generated)
  -n, --namespace <namespace>  Root C# namespace (default: NeTEx.Models / SIRI.Models)
  --clean                     Delete output directory before generating
  --verbose                   Enable verbose logging

transmodel-generator list-netex-versions   List available NeTEx version tags
transmodel-generator list-siri-versions    List available SIRI version tags
```

## Use a branch or commit

Pass `--ref` instead of `--version`. For example, replace `COMMIT_SHA` with an upstream commit:

```bash
transmodel-generator generate-siri --ref COMMIT_SHA --output ./Generated --namespace MyApp.SIRI
```

`--clean` deletes the entire output directory before generation. Keep handwritten code outside that directory.

## Pre-generated models

For the supported schema versions and usage examples, see the [NeTEx packages](https://github.com/Spillgebees/Transmodel/blob/main/src/netex/Spillgebees.NeTEx.Models/README.md) and [SIRI packages](https://github.com/Spillgebees/Transmodel/blob/main/src/siri/Spillgebees.SIRI.Models/README.md).

## License

[EUPL-1.2](https://github.com/Spillgebees/Transmodel/blob/main/LICENSE). The upstream schemas have their own licensing information in the [NeTEx repository](https://github.com/TransmodelEcosystem/NeTEx) and [SIRI repository](https://github.com/TransmodelEcosystem/SIRI).
