using System.Xml.Linq;
using System.Xml.Serialization;
using AwesomeAssertions;
using Spillgebees.SIRI.Models.V2_2.SIRI;

namespace Spillgebees.SIRI.Models.Tests.Serialization;

public class DefaultPresenceTests
{
    [Test]
    [Arguments(null)]
    [Arguments(true)]
    [Arguments(false)]
    public void Should_preserve_assignment_of_defaulted_monitored_value(bool? assignedValue)
    {
        // arrange
        var original = new MonitoredVehicleJourneyStructure();
        if (assignedValue.HasValue)
        {
            original.Monitored = assignedValue.Value;
        }

        // act
        var xml = Serialize(original);
        var monitored = xml.Root!.Element(XName.Get("Monitored", "http://www.siri.org.uk/siri"));

        // assert
        original.Monitored.Should().Be(assignedValue ?? true);
        if (assignedValue.HasValue)
        {
            monitored.Should().NotBeNull();
            monitored!.Value.Should().Be(assignedValue.Value ? "true" : "false");
        }
        else
        {
            monitored.Should().BeNull();
        }
    }

    [Test]
    [Arguments("")]
    [Arguments("<Monitored xmlns=\"http://www.siri.org.uk/siri\">true</Monitored>")]
    [Arguments("<Monitored xmlns=\"http://www.siri.org.uk/siri\">false</Monitored>")]
    public void Should_preserve_default_presence_after_deserialization(string element)
    {
        // arrange
        var serializer = new XmlSerializer(typeof(MonitoredVehicleJourneyStructure));
        using var reader = new StringReader($"<MonitoredVehicleJourneyStructure>{element}</MonitoredVehicleJourneyStructure>");

        // act
        var model = (MonitoredVehicleJourneyStructure)serializer.Deserialize(reader)!;
        var xml = Serialize(model);

        // assert
        var monitored = xml.Root!.Element(XName.Get("Monitored", "http://www.siri.org.uk/siri"));
        (monitored is not null).Should().Be(element.Length != 0);
        if (monitored is not null)
        {
            monitored.Value.Should().Be(model.Monitored ? "true" : "false");
        }
    }

    private static XDocument Serialize(MonitoredVehicleJourneyStructure model)
    {
        var serializer = new XmlSerializer(typeof(MonitoredVehicleJourneyStructure));
        using var writer = new StringWriter();
        serializer.Serialize(writer, model);
        return XDocument.Parse(writer.ToString());
    }
}
