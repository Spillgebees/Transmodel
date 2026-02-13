using System.Xml.Serialization;
using AwesomeAssertions;
using Spillgebees.SIRI.Models.V2_2.IFOPT;

namespace Spillgebees.SIRI.Models.Tests.Serialization;

public class IfoptSerializationTests
{
    [Test]
    public void Should_serialize_and_deserialize_stop_place_ref_structure()
    {
        // arrange
        var serializer = new XmlSerializer(typeof(StopPlaceRefStructure));
        var original = new StopPlaceRefStructure
        {
            Value = "NSR:StopPlace:1234",
        };

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        var xml = writer.ToString();

        using var reader = new StringReader(xml);
        var deserialized = serializer.Deserialize(reader) as StopPlaceRefStructure;

        // assert
        deserialized.Should().NotBeNull();
        deserialized.Value.Should().Be("NSR:StopPlace:1234");
    }

    [Test]
    public void Should_have_correct_xml_type_namespace_on_ifopt_types()
    {
        // act
        var xmlTypeAttr = typeof(StopPlaceRefStructure)
            .GetCustomAttributes(typeof(XmlTypeAttribute), false)
            .Cast<XmlTypeAttribute>()
            .FirstOrDefault();

        // assert
        xmlTypeAttr.Should().NotBeNull();
        xmlTypeAttr.Namespace.Should().Be("http://www.ifopt.org.uk/ifopt");
    }
}
