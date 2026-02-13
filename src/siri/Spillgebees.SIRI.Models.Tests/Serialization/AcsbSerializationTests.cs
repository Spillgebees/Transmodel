using System.Xml.Serialization;
using AwesomeAssertions;
using Spillgebees.SIRI.Models.V2_2.ACSB;

namespace Spillgebees.SIRI.Models.Tests.Serialization;

public class AcsbSerializationTests
{
    [Test]
    public void Should_serialize_and_deserialize_suitability()
    {
        // arrange
        var serializer = new XmlSerializer(typeof(SuitabilityStructure));
        var original = new SuitabilityStructure
        {
            Suitable = SuitabilityEnumeration.Suitable,
            UserNeed = new UserNeedStructure
            {
                MobilityNeed = MobilityEnumeration.Wheelchair,
            },
        };

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        var xml = writer.ToString();

        using var reader = new StringReader(xml);
        var deserialized = serializer.Deserialize(reader) as SuitabilityStructure;

        // assert
        deserialized.Should().NotBeNull();
        deserialized.Suitable.Should().Be(SuitabilityEnumeration.Suitable);
        deserialized.UserNeed.MobilityNeed.Should().Be(MobilityEnumeration.Wheelchair);
    }

    [Test]
    public void Should_have_correct_xml_type_namespace_on_acsb_types()
    {
        // act
        var xmlTypeAttr = typeof(AccessibilityAssessmentStructure)
            .GetCustomAttributes(typeof(XmlTypeAttribute), false)
            .Cast<XmlTypeAttribute>()
            .FirstOrDefault();

        // assert
        xmlTypeAttr.Should().NotBeNull();
        xmlTypeAttr.Namespace.Should().Be("http://www.ifopt.org.uk/acsb");
    }
}
