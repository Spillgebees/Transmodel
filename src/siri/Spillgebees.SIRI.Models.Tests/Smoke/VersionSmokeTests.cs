using System.Xml.Serialization;
using AwesomeAssertions;
using V2_1_SIRI = Spillgebees.SIRI.Models.V2_1.SIRI;
using V2_1_IFOPT = Spillgebees.SIRI.Models.V2_1.IFOPT;
using V2_1_ACSB = Spillgebees.SIRI.Models.V2_1.ACSB;
using V2_1_DATEX2 = Spillgebees.SIRI.Models.V2_1.DATEX2;
using V2_1_WSDL = Spillgebees.SIRI.Models.V2_1.WSDL;
using V2_1_GML = Spillgebees.SIRI.Models.V2_1.GML;
using V2_2_SIRI = Spillgebees.SIRI.Models.V2_2.SIRI;
using V2_2_IFOPT = Spillgebees.SIRI.Models.V2_2.IFOPT;
using V2_2_ACSB = Spillgebees.SIRI.Models.V2_2.ACSB;
using V2_2_DATEX2 = Spillgebees.SIRI.Models.V2_2.DATEX2;
using V2_2_WSDL = Spillgebees.SIRI.Models.V2_2.WSDL;
using V2_2_GML = Spillgebees.SIRI.Models.V2_2.GML;

namespace Spillgebees.SIRI.Models.Tests.Smoke;

/// <summary>
/// Smoke tests verifying that core types exist and have correct XML namespace metadata
/// for each supported SIRI version (tags from the SIRI-CEN/SIRI GitHub repository).
/// </summary>
public class VersionSmokeTests
{
    [Test]
    public void Should_have_core_siri_types_for_v2_1()
    {
        typeof(V2_1_SIRI.Siri).IsClass.Should().BeTrue();
        typeof(V2_1_SIRI.ServiceDelivery).IsClass.Should().BeTrue();
        typeof(V2_1_SIRI.ServiceRequest).IsClass.Should().BeTrue();
        typeof(V2_1_SIRI.NaturalLanguageStringStructure).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_1_SIRI.Siri))
            .Should().Be("http://www.siri.org.uk/siri");
    }

    [Test]
    public void Should_have_ifopt_and_acsb_types_for_v2_1()
    {
        typeof(V2_1_IFOPT.StopPlaceRefStructure).IsClass.Should().BeTrue();
        typeof(V2_1_ACSB.AccessibilityAssessmentStructure).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_1_IFOPT.StopPlaceRefStructure))
            .Should().Be("http://www.ifopt.org.uk/ifopt");
        GetXmlTypeNamespace(typeof(V2_1_ACSB.AccessibilityAssessmentStructure))
            .Should().Be("http://www.ifopt.org.uk/acsb");
    }

    [Test]
    public void Should_have_datex2_and_gml_types_for_v2_1()
    {
        typeof(V2_1_DATEX2.TrafficStatusValue).IsClass.Should().BeTrue();
        typeof(V2_1_GML.DirectPositionType).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_1_DATEX2.TrafficStatusValue))
            .Should().Be("http://datex2.eu/schema/2_0RC1/2_0");
        GetXmlTypeNamespace(typeof(V2_1_GML.DirectPositionType))
            .Should().Be("http://www.opengis.net/gml/3.2");
    }

    [Test]
    public void Should_have_wsdl_types_for_v2_1()
    {
        typeof(V2_1_WSDL.GetSiriServiceRequestStructure).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_1_WSDL.GetSiriServiceRequestStructure))
            .Should().Be("http://wsdl.siri.org.uk");
    }

    [Test]
    public void Should_have_core_siri_types_for_v2_2()
    {
        typeof(V2_2_SIRI.Siri).IsClass.Should().BeTrue();
        typeof(V2_2_SIRI.ServiceDelivery).IsClass.Should().BeTrue();
        typeof(V2_2_SIRI.ServiceRequest).IsClass.Should().BeTrue();
        typeof(V2_2_SIRI.NaturalLanguageStringStructure).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_2_SIRI.Siri))
            .Should().Be("http://www.siri.org.uk/siri");
    }

    [Test]
    public void Should_have_ifopt_and_acsb_types_for_v2_2()
    {
        typeof(V2_2_IFOPT.StopPlaceRefStructure).IsClass.Should().BeTrue();
        typeof(V2_2_ACSB.AccessibilityAssessmentStructure).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_2_IFOPT.StopPlaceRefStructure))
            .Should().Be("http://www.ifopt.org.uk/ifopt");
        GetXmlTypeNamespace(typeof(V2_2_ACSB.AccessibilityAssessmentStructure))
            .Should().Be("http://www.ifopt.org.uk/acsb");
    }

    [Test]
    public void Should_have_datex2_and_gml_types_for_v2_2()
    {
        typeof(V2_2_DATEX2.TrafficStatusValue).IsClass.Should().BeTrue();
        typeof(V2_2_GML.DirectPositionType).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_2_DATEX2.TrafficStatusValue))
            .Should().Be("http://datex2.eu/schema/2_0RC1/2_0");
        GetXmlTypeNamespace(typeof(V2_2_GML.DirectPositionType))
            .Should().Be("http://www.opengis.net/gml/3.2");
    }

    [Test]
    public void Should_have_wsdl_types_for_v2_2()
    {
        typeof(V2_2_WSDL.GetSiriServiceRequestStructure).IsClass.Should().BeTrue();

        GetXmlTypeNamespace(typeof(V2_2_WSDL.GetSiriServiceRequestStructure))
            .Should().Be("http://wsdl.siri.org.uk");
    }

    private static string? GetXmlTypeNamespace(Type type) =>
        type.GetCustomAttributes(typeof(XmlTypeAttribute), false)
            .Cast<XmlTypeAttribute>()
            .FirstOrDefault()
            ?.Namespace;
}
