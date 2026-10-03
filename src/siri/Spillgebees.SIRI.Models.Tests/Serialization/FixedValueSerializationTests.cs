using System.Xml.Linq;
using System.Xml.Serialization;
using AwesomeAssertions;
using Spillgebees.SIRI.Models.V2_2.SIRI;

namespace Spillgebees.SIRI.Models.Tests.Serialization;

public class FixedValueSerializationTests
{
    [Test]
    public void Should_serialize_required_fixed_discovery_version()
    {
        // arrange
        var original = new ConnectionLinksDiscoveryRequestStructure
        {
            RequestTimestamp = DateTimeOffset.UnixEpoch,
            RequestorRef = new ParticipantRefStructure { Value = "test:requestor" },
        };
        var serializer = new XmlSerializer(original.GetType());

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        var xml = XDocument.Parse(writer.ToString());

        // assert
        var version = xml.Root!.Attribute("version");
        version.Should().NotBeNull();
        version!.Value.Should().Be("2.1");
        typeof(ConnectionLinksDiscoveryRequestStructure).GetProperty(nameof(original.Version))!.CanWrite.Should().BeFalse();
    }

    [Test]
    public void Should_include_optional_fixed_capability_only_when_requested()
    {
        // arrange
        var original = new SituationExchangeServiceCapabilitiesStructureTopicFiltering
        {
            DefaultPreviewInterval = TimeSpan.FromHours(1),
        };
        var serializer = new XmlSerializer(original.GetType());
        using var untouchedWriter = new StringWriter();
        serializer.Serialize(untouchedWriter, original);

        // act
        original.IncludeFilterByLocationRef();
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        var xml = XDocument.Parse(writer.ToString());
        using var reader = new StringReader(writer.ToString());
        var result = (SituationExchangeServiceCapabilitiesStructureTopicFiltering)serializer.Deserialize(reader)!;
        using var roundTripWriter = new StringWriter();
        serializer.Serialize(roundTripWriter, result);

        // assert
        XDocument.Parse(untouchedWriter.ToString()).Descendants()
            .Should().NotContain(element => element.Name.LocalName == "FilterByLocationRef");
        xml.Descendants().Single(element => element.Name.LocalName == "FilterByLocationRef").Value.Should().Be("true");
        XDocument.Parse(roundTripWriter.ToString()).Descendants()
            .Single(element => element.Name.LocalName == "FilterByLocationRef").Value.Should().Be("true");
    }

    [Test]
    public void Should_serialize_required_fixed_vehicle_monitoring_capability()
    {
        // arrange
        var original = new VehicleMonitoringServiceCapabilitiesStructureTopicFiltering
        {
            DefaultPreviewInterval = TimeSpan.FromHours(1),
        };
        var serializer = new XmlSerializer(original.GetType());

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        var xml = XDocument.Parse(writer.ToString());

        // assert
        var capability = xml.Root!.Element(XName.Get("FilterByVehicleMonitoringRef", "http://www.siri.org.uk/siri"));
        capability.Should().NotBeNull();
        capability!.Value.Should().Be("true");
    }
}
