using System.Xml.Linq;
using System.Xml.Serialization;
using AwesomeAssertions;
using Netex132 = Spillgebees.NeTEx.Models.V1_3_2.NeTEx;

namespace Spillgebees.NeTEx.Models.Tests.Serialization.Netex;

public class ReleaseRegressionTests
{
    [Test]
    public void Should_keep_the_netex_namespace_for_v1_3_2()
    {
        // arrange
        var type = typeof(Netex132.StopPlace);

        // act
        var xmlType = type.GetCustomAttributes(typeof(XmlTypeAttribute), false)
            .Cast<XmlTypeAttribute>().Single();

        // assert
        xmlType.Namespace.Should().Be("http://www.netex.org.uk/netex");
    }
    [Test]
    public void Should_round_trip_type_of_frame_reference_attributes()
    {
        // arrange
        var original = new Netex132.ClassInFrameStructure
        {
            TypeOfFrameRef = new Netex132.TypeOfFrameRefStructure { Ref = "frame-type:1", Version = "1" },
        };
        var serializer = new XmlSerializer(typeof(Netex132.ClassInFrameStructure));

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        using var reader = new StringReader(writer.ToString());
        var result = (Netex132.ClassInFrameStructure)serializer.Deserialize(reader)!;

        // assert
        result.TypeOfFrameRef.Should().NotBeNull();
        result.TypeOfFrameRef.Ref.Should().Be("frame-type:1");
        result.TypeOfFrameRef.Version.Should().Be("1");
        XDocument.Parse(writer.ToString()).Descendants()
            .Single(element => element.Name.LocalName == "TypeOfFrameRef")
            .Attribute("ref")!.Value.Should().Be("frame-type:1");
    }

    [Test]
    public void Should_round_trip_route_instruction_references()
    {
        // arrange
        var original = new Netex132.RouteInstructionsRelStructure
        {
            RouteInstructionRef =
            [
                new Netex132.RouteInstructionRefStructure { Ref = "instruction:1", Version = "1" },
                new Netex132.RouteInstructionRefStructure { Ref = "instruction:2", Version = "1" },
            ],
        };
        var serializer = new XmlSerializer(typeof(Netex132.RouteInstructionsRelStructure));

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        using var reader = new StringReader(writer.ToString());
        var result = (Netex132.RouteInstructionsRelStructure)serializer.Deserialize(reader)!;

        // assert
        result.RouteInstructionRef.Should().HaveCount(2);
        result.RouteInstructionRef!.Select(reference => reference.Ref)
            .Should().Equal("instruction:1", "instruction:2");
        result.RouteInstruction.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_deserialize_concrete_luggage_locker_equipment()
    {
        // arrange
        const string xml = """
            <LuggageLockerEquipment xmlns="http://www.netex.org.uk/netex" id="locker:1" version="1" />
            """;
        var serializer = new XmlSerializer(typeof(Netex132.LuggageLockerEquipment));

        // act
        using var reader = new StringReader(xml);
        var result = (Netex132.LuggageLockerEquipment)serializer.Deserialize(reader)!;

        // assert
        result.Id.Should().Be("locker:1");
        result.Version.Should().Be("1");
        typeof(Netex132.LuggageLockerEquipment).IsAbstract.Should().BeFalse();
    }
}
