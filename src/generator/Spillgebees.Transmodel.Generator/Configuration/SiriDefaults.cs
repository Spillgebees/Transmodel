namespace Spillgebees.Transmodel.Generator.Configuration;

public static class SiriDefaults
{
    public const string DefaultVersion = "v2.2";
    public const string DefaultNamespace = "SIRI.Models";

    public const string SiriXmlNamespace = "http://www.siri.org.uk/siri";
    public const string IfoptXmlNamespace = "http://www.ifopt.org.uk/ifopt";
    public const string AcsbXmlNamespace = "http://www.ifopt.org.uk/acsb";
    public const string Datex2XmlNamespace = "http://datex2.eu/schema/2_0RC1/2_0";
    public const string WsdlXmlNamespace = "http://wsdl.siri.org.uk";

    public const string SiriSubNamespace = "SIRI";
    public const string IfoptSubNamespace = "IFOPT";
    public const string AcsbSubNamespace = "ACSB";
    public const string Datex2SubNamespace = "DATEX2";
    public const string WsdlSubNamespace = "WSDL";

    public const string GitHubArchiveUrlTemplate =
        "https://github.com/SIRI-CEN/SIRI/archive/refs/tags/{0}.tar.gz";
    public const string GitHubArchiveRefUrlTemplate =
        "https://github.com/SIRI-CEN/SIRI/archive/{0}.tar.gz";
    public const string GitHubApiTagsUrl =
        "https://api.github.com/repos/SIRI-CEN/SIRI/tags";

    public const string MainSchemaFileName = "siri.xsd";

    public const string CacheDirectoryName = "siri-schemas";
}
