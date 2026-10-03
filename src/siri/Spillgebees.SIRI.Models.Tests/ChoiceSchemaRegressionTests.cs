using System.Reflection;
using AwesomeAssertions;

namespace Spillgebees.SIRI.Models.Tests;

public class ChoiceSchemaRegressionTests
{
    [Test]
    [Arguments(typeof(V2_1.SIRI.AffectedSectionStructureIndirectSectionRef))]
    [Arguments(typeof(V2_2.SIRI.AffectedSectionStructureIndirectSectionRef))]
    public void Should_allow_mixed_references_in_repeated_situation_exchange_choice(Type sectionType)
    {
        // arrange
        var properties = new[] { "IntermediateStopPointRef", "IntermediateStopPlaceRef", "IntermediateQuayRef" };

        // act
        var attributes = properties.SelectMany(name => sectionType.GetProperty(name)!.GetCustomAttributesData())
            .Where(attribute => attribute.AttributeType.Name == "XmlChoiceGroupAttribute")
            .ToList();

        // assert
        attributes.Should().BeEmpty("each repetition can select a different reference type");
    }

    [Test]
    [Arguments(typeof(V2_1.SIRI.VehicleMonitoringSubscriptionStructure), "ChangeBeforeUpdates", "UpdateInterval")]
    [Arguments(typeof(V2_2.SIRI.VehicleMonitoringSubscriptionStructure), "ChangeBeforeUpdates", "UpdateInterval")]
    [Arguments(typeof(V2_1.SIRI.SituationExchangeRequestStructure), "LineRef", "Lines")]
    [Arguments(typeof(V2_2.SIRI.SituationExchangeRequestStructure), "LineRef", "Lines")]
    public void Should_allow_omitting_choices_with_optional_alternatives(Type modelType, string first, string second)
    {
        // arrange
        var properties = new[] { first, second };

        // act
        var attributes = properties.Select(name => modelType.GetProperty(name)!.GetCustomAttributesData()
            .Single(attribute => attribute.AttributeType.Name == "XmlChoiceGroupAttribute")).ToList();

        // assert
        attributes[0].ConstructorArguments[0].Value.Should().Be(attributes[1].ConstructorArguments[0].Value);
        attributes[0].ConstructorArguments[1].Value.Should().NotBe(attributes[1].ConstructorArguments[1].Value);
        attributes.Should().AllSatisfy(attribute =>
            attribute.NamedArguments.Single(argument => argument.MemberName == "IsRequired")
                .TypedValue.Value.Should().Be(false));
    }
}
