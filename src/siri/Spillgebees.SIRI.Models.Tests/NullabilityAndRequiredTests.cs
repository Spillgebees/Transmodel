using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using AwesomeAssertions;
using Spillgebees.SIRI.Models.V2_2.ACSB;
using Spillgebees.SIRI.Models.V2_2.SIRI;

namespace Spillgebees.SIRI.Models.Tests;

/// <summary>
/// Tests verifying that the generated SIRI v2.2 models correctly use
/// C# 11 <c>required</c> modifiers, nullable reference type annotations,
/// and the ShouldSerialize pattern for optional value types.
/// </summary>
public class NullabilityAndRequiredTests
{
    [Test]
    public void Should_construct_key_value_with_required_properties()
    {
        // arrange & act — KeyValueStructure has required string Key and Value
        var kv = new KeyValueStructure
        {
            Key = "test-key",
            Value = "test-value",
        };

        // assert
        kv.Key.Should().Be("test-key");
        kv.Value.Should().Be("test-value");
    }

    [Test]
    public void Should_construct_natural_language_string_with_required_value()
    {
        // arrange & act — NaturalLanguageStringStructure.Value is required
        // because it extends a base type with minLength=1
        var str = new NaturalLanguageStringStructure
        {
            Value = "hello",
        };

        // assert
        str.Value.Should().Be("hello");
    }

    [Test]
    public void Should_construct_accessibility_assessment_with_required_bool()
    {
        // arrange & act — MobilityImpairedAccess is a required bool
        var assessment = new AccessibilityAssessmentStructure
        {
            MobilityImpairedAccess = true,
        };

        // assert
        assessment.MobilityImpairedAccess.Should().BeTrue();
    }

    [Test]
    public void Should_allow_null_on_optional_reference_type_properties()
    {
        // arrange

        // act — all properties on the Siri envelope are optional
        var siri = new Siri
        {
            ServiceRequest = null,
            ServiceDelivery = null
        };

        // assert
        siri.ServiceRequest.Should().BeNull();
        siri.ServiceDelivery.Should().BeNull();
    }

    [Test]
    public void Should_allow_null_on_optional_string_attribute()
    {
        // arrange

        // act
        var str = new NaturalLanguageStringStructure { Value = "test",
            Lang = null
        };

        // assert
        str.Lang.Should().BeNull();
    }

    [Test]
    public void Should_serialize_nullable_value_type_when_set()
    {
        // arrange — InfoLinkStructure.LinkContent is Nullable<LinkContentEnumeration>
        var link = new InfoLinkStructure
        {
            Uri = "https://example.com",
            LinkContent = LinkContentEnumeration.Timetable,
        };

        // act & assert
        link.LinkContent.Should().Be(LinkContentEnumeration.Timetable);
        link.ShouldSerializeLinkContent().Should().BeTrue();
    }

    [Test]
    public void Should_not_serialize_nullable_value_type_when_null()
    {
        // arrange — LinkContent defaults to null
        var link = new InfoLinkStructure
        {
            Uri = "https://example.com",
        };

        // act & assert
        link.LinkContent.Should().BeNull();
        link.ShouldSerializeLinkContent().Should().BeFalse();
    }

    [Test]
    public void Should_initialize_collection_properties_via_constructor()
    {
        // arrange & act — ServiceDeliveryStructure initializes all List<> properties
        var delivery = new ServiceDeliveryStructure
        {
            ResponseTimestamp = DateTimeOffset.UtcNow,
        };

        // assert — collections are non-null after construction
        delivery.StopMonitoringDelivery.Should().NotBeNull();
        delivery.EstimatedTimetableDelivery.Should().NotBeNull();
        delivery.VehicleMonitoringDelivery.Should().NotBeNull();
    }

    [Test]
    public void Should_not_serialize_empty_collections()
    {
        // arrange
        var delivery = new ServiceDeliveryStructure
        {
            ResponseTimestamp = DateTimeOffset.UtcNow,
        };

        // act & assert — empty collections should not be serialized
        delivery.ShouldSerializeStopMonitoringDelivery().Should().BeFalse();
        delivery.ShouldSerializeEstimatedTimetableDelivery().Should().BeFalse();
    }

    [Test]
    public void Should_round_trip_required_properties_via_xml_serializer()
    {
        // arrange — XmlSerializer uses a parameterless constructor and ignores 'required'
        var serializer = new XmlSerializer(typeof(NaturalLanguageStringStructure));
        var original = new NaturalLanguageStringStructure { Value = "hello" };

        // act
        using var writer = new StringWriter();
        serializer.Serialize(writer, original);
        var xml = writer.ToString();

        using var reader = new StringReader(xml);
        var result = serializer.Deserialize(reader) as NaturalLanguageStringStructure;

        // assert
        result.Should().NotBeNull();
        result.Value.Should().Be("hello");
        result.Lang.Should().BeNull();
    }

    [Test]
    public void Should_have_required_modifier_on_key_value_key_property()
    {
        // arrange
        var property = typeof(KeyValueStructure)
            .GetProperty(nameof(KeyValueStructure.Key))!;

        // act
        var isRequired = property.IsDefined(typeof(RequiredMemberAttribute), inherit: false);

        // assert
        isRequired.Should().BeTrue();
    }

    [Test]
    public void Should_have_required_modifier_on_response_timestamp()
    {
        // arrange — ResponseStructure.ResponseTimestamp is required
        var property = typeof(ResponseStructure)
            .GetProperty(nameof(ResponseStructure.ResponseTimestamp))!;

        // act
        var isRequired = property.IsDefined(typeof(RequiredMemberAttribute), inherit: false);

        // assert
        isRequired.Should().BeTrue();
    }

    [Test]
    public void Should_not_have_required_modifier_on_optional_property()
    {
        // arrange — KeyValueStructure.TypeOfKey is optional
        var property = typeof(KeyValueStructure)
            .GetProperty(nameof(KeyValueStructure.TypeOfKey))!;

        // act
        var isRequired = property.IsDefined(typeof(RequiredMemberAttribute), inherit: false);

        // assert
        isRequired.Should().BeFalse();
    }

    [Test]
    public void Should_have_nullable_annotation_on_optional_reference_type_property()
    {
        // arrange — Siri.ServiceRequest is optional
        var property = typeof(Siri)
            .GetProperty(nameof(Siri.ServiceRequest))!;
        var context = new NullabilityInfoContext();

        // act
        var info = context.Create(property);

        // assert
        info.WriteState.Should().Be(NullabilityState.Nullable);
        info.ReadState.Should().Be(NullabilityState.Nullable);
    }

    [Test]
    public void Should_not_have_nullable_annotation_on_required_string_property()
    {
        // arrange — KeyValueStructure.Key is required (non-nullable)
        var property = typeof(KeyValueStructure)
            .GetProperty(nameof(KeyValueStructure.Key))!;
        var context = new NullabilityInfoContext();

        // act
        var info = context.Create(property);

        // assert
        info.WriteState.Should().Be(NullabilityState.NotNull);
        info.ReadState.Should().Be(NullabilityState.NotNull);
    }

    [Test]
    public void Should_have_nullable_annotation_on_optional_string_attribute()
    {
        // arrange — NaturalLanguageStringStructure.Lang is optional
        var property = typeof(NaturalLanguageStringStructure)
            .GetProperty(nameof(NaturalLanguageStringStructure.Lang))!;
        var context = new NullabilityInfoContext();

        // act
        var info = context.Create(property);

        // assert
        info.WriteState.Should().Be(NullabilityState.Nullable);
    }

    [Test]
    public void Should_have_nullable_context_enabled_on_generated_types()
    {
        // arrange — NullableContextAttribute is emitted by the compiler when
        // #nullable enable is active for the file
        var attr = typeof(Siri)
            .GetCustomAttribute<NullableContextAttribute>();

        // assert — a non-null attribute means the nullable context was active
        attr.Should().NotBeNull();
    }
}
