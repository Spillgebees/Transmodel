# Upgrading to the upstream generator integration

This update is intended for a separate breaking release after the schema and documentation refresh. It bundles generator and analyzer packages `99.0.15-local`, built from fork commit [`4a47ec5`](https://github.com/igotinfected/XmlSchemaClassGenerator/commit/4a47ec5), which integrates upstream through [`a75f225`](https://github.com/mganss/XmlSchemaClassGenerator/commit/a75f225).

Some generated C# properties have been removed because they allowed XML that did not match the schema or duplicated another XML mapping. Rebuild consumers against the new packages and update uses of those properties.

## Replace removed properties

These examples use the SIRI `v2.3` GML bindings and NeTEx `v2.0.0` bindings. The affected properties vary by schema version.

| Previous API | Migration |
| --- | --- |
| GML `AbstractCurve`, `AbstractSurface`, or `AbstractGeometricPrimitive` element properties | Select a concrete alternative allowed at that location in the schema. Abstract XML elements cannot appear directly. |
| `OnlineServiceOperatorVersionStructure.RoadAddress` or `.PostalAddress` | Use the surviving `Address` property and its declared type. The local `Address` element does not inherit the global element's substitutes. |
| Equipment-reference alternatives on `ActivationAssignmentVersionStructure` | Use the local `EquipmentRef` property. Unrelated alternatives from the global substitution group no longer appear. |
| `ReliefPointsInFrameRelStructure.ParkingPoint1` | Use `ParkingPoint`. Both properties previously mapped the same XML element. |

Do not replace removed properties with arbitrary XML elements. Check the applicable XSD when choosing a concrete alternative. If a previously valid document no longer round-trips, retain the document as a regression fixture and report the schema version and generated type.

## Retained behaviour

Transmodel enables `UseLegacyMixedTextPropertyName`, preserving existing mixed-content `Text` properties and their collision suffixes. Direct users of the generator fork can enable this option too. Without it, upstream uses the configured `TextValuePropertyName`.

Required choice alternatives still belong in object initializers. Scalar default presence tracking, object-valued defaults, getter-only fixed values, optional fixed-value inclusion methods, and token-list collection APIs retain the behaviour described in [generated model behaviour](model-behaviour.md).

Strict decimal ranges now treat `fractionDigits` as a maximum number of fractional digits. For example, `totalDigits=3` and `fractionDigits=2` permit `999`. The generated range no longer incorrectly limits that value to `9.99`. A range attribute alone does not enforce every digit restriction in the XSD.

## Validation limits

The integrated generator passes `767` generator tests and `41` analyzer tests. Regenerating all `11` schema projects also passes the `191` Transmodel tests. These checks cover the reported regressions, but do not prove every generated type fully matches its XSD. Validate exchanged XML against the appropriate schema where complete schema validation is required.
