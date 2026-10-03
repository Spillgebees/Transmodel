using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using AwesomeAssertions;
using Siri221 = Spillgebees.SIRI.Models.V2_2_1.SIRI;
using Siri23 = Spillgebees.SIRI.Models.V2_3.SIRI;

namespace Spillgebees.SIRI.Models.Tests.Serialization;

public class ReleaseRegressionTests
{
    [Test]
    public void Should_keep_the_siri_namespace_for_new_releases()
    {
        // arrange
        var types = new[] { typeof(Siri221.Siri), typeof(Siri23.Siri) };

        // act
        var namespaces = types.Select(type => type.GetCustomAttributes(typeof(XmlTypeAttribute), false)
            .Cast<XmlTypeAttribute>().Single().Namespace);

        // assert
        namespaces.Should().OnlyContain(value => value == "http://www.siri.org.uk/siri");
    }
    [Test]
    public void Should_round_trip_corrected_driver_scope_in_v2_2_1()
    {
        // arrange
        const string xml = """
            <DriverMessageStructure xmlns="http://www.siri.org.uk/siri">
              <MessageIdentifier>message:1</MessageIdentifier>
              <DriverScope><EmployeeRef>driver:1</EmployeeRef></DriverScope>
            </DriverMessageStructure>
            """;
        var serializer = new XmlSerializer(typeof(Siri221.DriverMessageStructure), "http://www.siri.org.uk/siri");

        // act
        using var reader = new StringReader(xml);
        var message = (Siri221.DriverMessageStructure)serializer.Deserialize(reader)!;
        using var writer = new StringWriter();
        serializer.Serialize(writer, message);

        // assert
        message.DriverScope.EmployeeRef.Should().Be("driver:1");
        writer.ToString().Should().Contain("<DriverScope>").And.NotContain("DiverScope");
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public void Should_round_trip_dead_run_flag_in_v2_3(bool isDeadRun)
    {
        // arrange
        var original = new Siri23.TargetedVehicleJourneyStructure
        {
            LineRef = new Siri23.LineRefStructure { Value = "line:1" },
            DirectionRef = new Siri23.DirectionRefStructure { Value = "outbound" },
            IsDeadRun = isDeadRun,
        };
        var serializer = new XmlSerializer(typeof(Siri23.TargetedVehicleJourneyStructure));

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        using var reader = new StringReader(writer.ToString());
        var result = (Siri23.TargetedVehicleJourneyStructure)serializer.Deserialize(reader)!;

        // assert
        result.IsDeadRun.Should().Be(isDeadRun);
        XDocument.Parse(writer.ToString()).Descendants()
            .Single(element => element.Name.LocalName == "IsDeadRun").Value
            .Should().Be(XmlConvert.ToString(isDeadRun));
    }

    [Test]
    public void Should_serialize_action_keywords_as_one_schema_valid_token_list()
    {
        // arrange
        var original = new Siri23.ParameterisedActionStructure { Keywords = ["delay", "rail"] };
        var serializer = new XmlSerializer(typeof(Siri23.ParameterisedActionStructure));
        const string schema = """
            <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
                       targetNamespace="http://www.siri.org.uk/siri" elementFormDefault="qualified">
              <xs:element name="Keywords" type="xs:NMTOKENS" />
            </xs:schema>
            """;
        var schemas = new XmlSchemaSet();
        using var schemaReader = new StringReader(schema);
        schemas.Add("http://www.siri.org.uk/siri", XmlReader.Create(schemaReader));

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        var keywords = XDocument.Parse(writer.ToString()).Descendants()
            .Where(element => element.Name.LocalName == "Keywords").ToList();

        // assert
        keywords.Should().ContainSingle();
        keywords[0].Value.Should().Be("delay rail");
        var validationErrors = new List<string>();
        new XDocument(new XElement(keywords[0])).Validate(schemas, (_, error) => validationErrors.Add(error.Message));
        validationErrors.Should().BeEmpty();
        using var reader = new StringReader(writer.ToString());
        var result = (Siri23.ParameterisedActionStructure)serializer.Deserialize(reader)!;
        result.Keywords.Should().Equal("delay", "rail");
    }
    [Test]
    public void Should_round_trip_passenger_information_periods_in_v2_3()
    {
        // arrange
        var start = new DateTimeOffset(2026, 10, 3, 8, 0, 0, TimeSpan.Zero);
        var original = new Siri23.PassengerInformationActionStructure
        {
            ActionRef = new Siri23.EntryQualifierStructure { Value = "action:1" },
            RecordedAtTime = start,
            Period =
            [
                new Siri23.HalfOpenTimestampOutputRangeStructure { StartTime = start, EndTime = start.AddHours(2) },
                new Siri23.HalfOpenTimestampOutputRangeStructure { StartTime = start.AddDays(1) },
            ],
        };
        var serializer = new XmlSerializer(typeof(Siri23.PassengerInformationActionStructure));

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        using var reader = new StringReader(writer.ToString());
        var result = (Siri23.PassengerInformationActionStructure)serializer.Deserialize(reader)!;

        // assert
        result.Period.Should().HaveCount(2);
        result.Period![0].StartTime.Should().Be(start);
        result.Period[0].EndTime.Should().Be(start.AddHours(2));
        result.Period[1].StartTime.Should().Be(start.AddDays(1));
        result.Period[1].EndTime.Should().BeNull();
    }
}
