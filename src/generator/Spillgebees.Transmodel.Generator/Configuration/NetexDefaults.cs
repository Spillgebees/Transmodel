namespace Spillgebees.Transmodel.Generator.Configuration;

public static class NetexDefaults
{
    public const string DefaultVersion = "v1.3.1";
    public const string DefaultNamespace = "NeTEx.Models";

    public const string NetexXmlNamespace = "http://www.netex.org.uk/netex";
    public const string SiriXmlNamespace = "http://www.siri.org.uk/siri";

    public const string NetexSubNamespace = "NeTEx";
    public const string SiriSubNamespace = "SIRI";

    public const string GitHubArchiveUrlTemplate =
        "https://github.com/TransmodelEcosystem/NeTEx/archive/refs/tags/{0}.tar.gz";
    public const string GitHubArchiveRefUrlTemplate =
        "https://github.com/TransmodelEcosystem/NeTEx/archive/{0}.tar.gz";
    public const string GitHubApiTagsUrl =
        "https://api.github.com/repos/TransmodelEcosystem/NeTEx/tags";

    public const string PublicationSchemaFileName = "NeTEx_publication.xsd";
    public const string SiriSchemaFileName = "NeTEx_siri.xsd";

    public const string CacheDirectoryName = "netex-schemas";
}
