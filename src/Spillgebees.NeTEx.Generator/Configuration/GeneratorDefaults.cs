namespace Spillgebees.NeTEx.Generator.Configuration;

public static class GeneratorDefaults
{
    public const string DefaultVersion = "v1.3.1";
    public const string DefaultOutputDirectory = "./NeTEx";
    public const string DefaultNamespace = "NeTEx";

    public const string NetexXmlNamespace = "http://www.netex.org.uk/netex";
    public const string SiriXmlNamespace = "http://www.siri.org.uk/siri";
    public const string GmlXmlNamespace = "http://www.opengis.net/gml/3.2";

    public const string NetexSubNamespace = "Netex";
    public const string SiriSubNamespace = "Siri";
    public const string GmlSubNamespace = "Gml";

    public const string GitHubArchiveUrlTemplate =
        "https://github.com/NeTEx-CEN/NeTEx/archive/refs/tags/{0}.tar.gz";

    public const string GitHubArchiveRefUrlTemplate =
        "https://github.com/NeTEx-CEN/NeTEx/archive/{0}.tar.gz";

    public const string GitHubApiTagsUrl =
        "https://api.github.com/repos/NeTEx-CEN/NeTEx/tags";

    public const string PublicationSchemaFileName = "NeTEx_publication_noConstraint.xsd";
    public const string SiriSchemaFileName = "NeTEx_siri.xsd";
}
