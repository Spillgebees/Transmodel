using Spillgebees.NeTEx.Generator.Configuration;
using XmlSchemaClassGenerator;
using XscGenerator = XmlSchemaClassGenerator.Generator;

namespace Spillgebees.NeTEx.Generator.Services;

public static class CodeGenerator
{
    public static void Generate(
        string xsdDirectory,
        string outputDirectory,
        string rootNamespace,
        bool verbose = false)
    {
        var netexNamespace = $"{rootNamespace}.{GeneratorDefaults.NetexSubNamespace}";
        var siriNamespace = $"{rootNamespace}.{GeneratorDefaults.SiriSubNamespace}";
        var gmlNamespace = $"{rootNamespace}.{GeneratorDefaults.GmlSubNamespace}";

        var namespaceProvider = new NamespaceProvider
        {
            GenerateNamespace = key =>
                CodeUtilities.GenerateNamespace(key.XmlSchemaNamespace, rootNamespace),
        };

        namespaceProvider[new NamespaceKey(GeneratorDefaults.NetexXmlNamespace)] = netexNamespace;
        namespaceProvider[new NamespaceKey(GeneratorDefaults.SiriXmlNamespace)] = siriNamespace;
        namespaceProvider[new NamespaceKey(GeneratorDefaults.GmlXmlNamespace)] = gmlNamespace;

        var generator = new XscGenerator
        {
            OutputFolder = Path.GetFullPath(outputDirectory),
            NamespaceProvider = namespaceProvider,

            // .NET Core / Nullable
            NetCoreSpecificCode = true,
            EnableNullableReferenceAttributes = true,
            GenerateNullables = true,

            // Code style
            CompactTypeNames = true,
            SeparateClasses = true,
            SeparateNamespaceHierarchy = true,
            UseDateOnly = true,

            // Collections
            CollectionSettersMode = CollectionSettersMode.Init,
            CollectionType = typeof(List<>),

            // Minimal attribute noise
            GenerateDebuggerStepThroughAttribute = false,
            GenerateDescriptionAttribute = false,
            GenerateCommandLineArgumentsComment = false,
            CreateGeneratedCodeAttributeVersion = false,

            // Structure
            EmitOrder = true,
            GenerateInterfaces = true,

            // Logging
            Log = verbose ? msg => Console.WriteLine($"  [xscgen] {msg}") : null,
        };

        var publicationSchema = Path.Combine(xsdDirectory, GeneratorDefaults.PublicationSchemaFileName);
        var siriSchema = Path.Combine(xsdDirectory, GeneratorDefaults.SiriSchemaFileName);

        var schemas = new List<string>();

        if (File.Exists(publicationSchema))
        {
            schemas.Add(publicationSchema);
        }
        else
        {
            throw new FileNotFoundException(
                $"Publication schema not found: {publicationSchema}");
        }

        if (File.Exists(siriSchema))
        {
            schemas.Add(siriSchema);
        }
        else if (verbose)
        {
            Console.WriteLine($"  [warn] SIRI schema not found at {siriSchema}, skipping SIRI types.");
        }

        generator.Generate(schemas);
    }
}
