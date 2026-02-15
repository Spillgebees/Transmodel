# AGENTS.md

Guidelines for AI agents operating in this repository.

## Project overview

C# XML bindings for [SIRI](https://www.siri-cen.eu/) and [NeTEx](https://netex-cen.eu/) public transport schemas. A CLI generator (`Spillgebees.Transmodel.Generator`) downloads official XSD schemas and produces C# classes via a fork of `XmlSchemaClassGenerator`. Version-specific model projects (e.g. `Spillgebees.NeTEx.Models.V1_3_1`) are thin wrappers whose `Generated/` contents are created at build time.

## Build / test / lint

```bash
# Build (from repo root)
dotnet build Spillgebees.Transmodel.slnx

# Run all tests
dotnet test --solution Spillgebees.Transmodel.slnx

# Run a single test by name
dotnet test --solution Spillgebees.Transmodel.slnx --filter "FullyQualifiedName~Should_serialize_and_deserialize_stop_place"

# Run tests in one project
dotnet test src/netex/Spillgebees.NeTEx.Models.Tests

# Clean (removes Generated/ dirs; next build regenerates them)
dotnet clean Spillgebees.Transmodel.slnx
```

There is no separate lint command. `TreatWarningsAsErrors=True` is set globally in `src/General.targets`, so building IS linting. The `.editorconfig` at `src/.editorconfig` configures all Roslyn analyzers.

## Critical rules

1. **Never edit files under `Generated/` directories.** They are machine-generated, `.gitignore`d, and recreated every build. All fixes must go in the xscgen fork or the generator.
2. **Never manually set package versions in `.csproj` files.** Use `src/Directory.Packages.props` (central package management).
3. **Never add other target frameworks.** The project targets `net10.0` only.
4. **`.targets` files are shared MSBuild imports.** Changes to `General.targets`, `SIRI.Models.targets`, or `NeTEx.Models.targets` affect every project that imports them.
5. **The `packages/` directory contains vendored local `.nupkg` files** for a pre-release xscgen fork. These are intentionally committed.

## Project structure

```
Spillgebees.Transmodel.slnx         Solution file
global.json                         .NET 10 SDK pinning
nuget.config                        NuGet sources (nuget.org + local packages/)
packages/                           Vendored local nupkgs (xscgen fork)
src/
  .editorconfig                     Code style / analyzer rules
  Directory.Packages.props          Central package version management
  General.targets                   Shared: TFM, nullable, TreatWarningsAsErrors
  generator/
    Spillgebees.Transmodel.Generator/  CLI tool (System.CommandLine)
  siri/
    SIRI.Models.targets             NuGet metadata + build-time generation
    Spillgebees.SIRI.Models/        Meta-package (all SIRI versions)
    Spillgebees.SIRI.Models.V2_1/   SIRI v2.1 bindings
    Spillgebees.SIRI.Models.V2_2/   SIRI v2.2 bindings
    Spillgebees.SIRI.Models.Tests/  Tests
  netex/
    NeTEx.Models.targets            NuGet metadata + build-time generation
    Spillgebees.NeTEx.Models/       Meta-package (all NeTEx versions)
    Spillgebees.NeTEx.Models.V1_*/  Version-specific bindings (5 versions)
    Spillgebees.NeTEx.Models.Tests/ Tests
```

## Code style

Enforced by `src/.editorconfig` and `TreatWarningsAsErrors`. Key rules:

### Formatting
- **No region markers or decorative comment dividers.** Do not use comments like `// -- section name -----` or `#region`/`#endregion`. If a file needs sectioning, it should likely be split into separate files instead.
- **4 spaces** for C#; **2 spaces** for XML/csproj/json/yaml
- **Allman braces** (opening brace on new line for all constructs)
- **Always use braces**, even for single-line `if`/`for`/etc.
- **File-scoped namespaces** (`namespace Foo;` not `namespace Foo { }`)
- No multiple consecutive blank lines

### Types and keywords
- **Use `var` everywhere** (built-in types, apparent types, elsewhere)
- **Use predefined type keywords** (`int` not `Int32`, `string` not `String`)
- **Nullable reference types** are enabled globally
- Prefer expression-bodied members, object/collection initializers, pattern matching, null propagation

### Naming conventions
| Symbol | Convention | Example |
|---|---|---|
| Constants | PascalCase | `MaxRetries` |
| Private/internal fields | `_camelCase` | `_groupId` |
| Methods, properties | PascalCase | `GenerateCode()` |
| Local variables | camelCase | `rootNamespace` |

- No `this.` qualifier
- Modifier order: `public, private, protected, internal, static, extern, new, virtual, abstract, sealed, override, readonly, unsafe, volatile, async`

### Imports
- System namespaces first (`dotnet_sort_system_directives_first = true`)
- Placed outside the namespace declaration
- Implicit usings are enabled (`System`, `System.Collections.Generic`, `System.Linq`, etc. are available without explicit `using`)

## Test conventions

### Framework
- **TUnit** (not xUnit/NUnit/MSTest) with `[Test]` attribute
- **AwesomeAssertions** for fluent assertions (`.Should().Be(...)`, `.Should().NotBeNull()`)
- Test runner: `Microsoft.Testing.Platform` (configured in `global.json`)

### Naming
Tests use `Should_describe_expected_behavior` in snake_case:
```csharp
[Test]
public void Should_serialize_and_deserialize_stop_place_with_stop_place_type()
```

### Structure
Use Arrange/Act/Assert with comments:
```csharp
[Test]
public void Should_round_trip_multilingual_string()
{
    // arrange
    var serializer = new XmlSerializer(typeof(MultilingualString));
    var original = new MultilingualString { Value = "hello", Lang = "en" };

    // act
    using var writer = new StringWriter();
    serializer.Serialize(writer, original);
    var xml = writer.ToString();

    using var reader = new StringReader(xml);
    var deserialized = (MultilingualString?)serializer.Deserialize(reader);

    // assert
    deserialized.Should().NotBeNull();
    deserialized!.Value.Should().Be("hello");
}
```

### Organization
- `Smoke/` -- Type existence and XML namespace verification
- `Serialization/` -- XmlSerializer round-trip tests
- `Deserialization/` -- XML parsing tests (with `TestData/` fixtures)
- Root level -- Cross-cutting concerns (choice groups, nullability, required modifiers)

Test classes are plain `public class` with no base class or constructor injection. Test projects reference multiple versioned model assemblies.

## Tooling

- **SDK**: .NET 10.0 (`global.json` pins `10.0.102`, rolls forward within feature band)
- **Versioning**: MinVer (automatic from Git tags, no manual versions in csproj)
- **Reproducible builds**: `DotNet.ReproducibleBuilds` package
- **License**: EUPL-1.2
- **CI**: GitHub Actions (`.github/workflows/`) -- build, test, pack, publish to nuget.org on release
