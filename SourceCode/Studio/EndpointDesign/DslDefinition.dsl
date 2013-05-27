<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="60279b02-26cf-4430-b71e-b1191cba65f9" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointDesign" Name="EndpointDesign" DisplayName="EndpointDesign" Namespace="NServiceBus.Modeling.EndpointDesign" ProductName="NServiceBus.Modeling.EndpointDesign" CompanyName="Particular Software" PackageGuid="be731d99-2fd0-4514-9d8d-8d803fe6dc4a" PackageNamespace="NServiceBus.Modeling.EndpointDesign" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="18842bda-0564-4835-bd10-69a56e7903b1" Description="The root in which all other elements are embedded. Appears as a diagram." Name="EndpointModel" DisplayName="Endpoint Model" Namespace="NServiceBus.Modeling.EndpointDesign">
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Event" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EndpointModelHasEvents.Events</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Endpoint" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EndpointModelHasEndpoints.Endpoints</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Command" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EndpointModelHasCommands.Commands</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="e21c4731-2726-4484-8fac-640a7335b17b" Description="Elements embedded in the model. Appear as boxes on the diagram." Name="SendEndpoint" DisplayName="Send Endpoint" Namespace="NServiceBus.Modeling.EndpointDesign">
      <BaseClass>
        <DomainClassMoniker Name="Endpoint" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="fb4969d1-0587-4190-9e24-58ed1027cf0e" Description="Description for NServiceBus.Modeling.EndpointDesign.NamedElement" Name="NamedElement" DisplayName="Named Element" InheritanceModifier="Abstract" Namespace="NServiceBus.Modeling.EndpointDesign">
      <Properties>
        <DomainProperty Id="11d22a89-919a-48fe-aaef-d31986cdab2d" Description="Description for NServiceBus.Modeling.EndpointDesign.NamedElement.Name" Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7f078efd-3f61-4449-a2a5-65fbd0cec4c8" Description="Description for NServiceBus.Modeling.EndpointDesign.NamedElement.Description" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="f2b8d64e-2f61-4da1-ab47-65803bb9a09a" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpoint" Name="SendReceiveEndpoint" DisplayName="Send Receive Endpoint" Namespace="NServiceBus.Modeling.EndpointDesign">
      <BaseClass>
        <DomainClassMoniker Name="Endpoint" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="0ba0f6e6-7c0c-4ab0-9f68-7e5330b858a3" Description="Description for NServiceBus.Modeling.EndpointDesign.Event" Name="Event" DisplayName="Event" Namespace="NServiceBus.Modeling.EndpointDesign">
      <BaseClass>
        <DomainClassMoniker Name="NamedElement" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="84f56034-ebeb-47e5-9bf6-e7ee36e36218" Description="Description for NServiceBus.Modeling.EndpointDesign.Command" Name="Command" DisplayName="Command" Namespace="NServiceBus.Modeling.EndpointDesign">
      <BaseClass>
        <DomainClassMoniker Name="NamedElement" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="1ae46b43-25f0-4911-9a33-b61c2c1f77ee" Description="Description for NServiceBus.Modeling.EndpointDesign.Endpoint" Name="Endpoint" DisplayName="Endpoint" InheritanceModifier="Abstract" Namespace="NServiceBus.Modeling.EndpointDesign">
      <BaseClass>
        <DomainClassMoniker Name="NamedElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="ddd78a91-74d8-4306-8b0d-51752b0b7772" Description="Description for NServiceBus.Modeling.EndpointDesign.Endpoint.Type" Name="Type" DisplayName="Type">
          <Type>
            <ExternalTypeMoniker Name="/NServiceBus.Modeling.EndpointDesign.Interfaces/EndpointType" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="8fa0aebb-8141-46f1-bfe4-1b08a53b2c6f" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasEvents" Name="EndpointModelHasEvents" DisplayName="Endpoint Model Has Events" Namespace="NServiceBus.Modeling.EndpointDesign" IsEmbedding="true">
      <Source>
        <DomainRole Id="dc5a8e34-7c6e-4157-9858-97db1cf10187" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasEvents.EndpointModel" Name="EndpointModel" DisplayName="Endpoint Model" PropertyName="Events" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Events">
          <RolePlayer>
            <DomainClassMoniker Name="EndpointModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="aca62130-701d-4552-956e-a214c30e35a2" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasEvents.Event" Name="Event" DisplayName="Event" PropertyName="EndpointModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Endpoint Model">
          <RolePlayer>
            <DomainClassMoniker Name="Event" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4b427a2a-39d5-4ea5-9313-abd59ca52c79" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasEndpoints" Name="EndpointModelHasEndpoints" DisplayName="Endpoint Model Has Endpoints" Namespace="NServiceBus.Modeling.EndpointDesign" IsEmbedding="true">
      <Source>
        <DomainRole Id="390adaa9-8f4c-4060-92f1-445a3efce8df" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasEndpoints.EndpointModel" Name="EndpointModel" DisplayName="Endpoint Model" PropertyName="Endpoints" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Endpoints">
          <RolePlayer>
            <DomainClassMoniker Name="EndpointModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="33be3340-6dc7-4d56-b417-f1588026e3e2" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasEndpoints.Endpoint" Name="Endpoint" DisplayName="Endpoint" PropertyName="EndpointModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Endpoint Model">
          <RolePlayer>
            <DomainClassMoniker Name="Endpoint" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="52c02bec-dba7-4a4f-9d60-57c9b979effe" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasCommands" Name="EndpointModelHasCommands" DisplayName="Endpoint Model Has Commands" Namespace="NServiceBus.Modeling.EndpointDesign" IsEmbedding="true">
      <Source>
        <DomainRole Id="e9775036-ba02-47bc-90ae-3e871a3f5823" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasCommands.EndpointModel" Name="EndpointModel" DisplayName="Endpoint Model" PropertyName="Commands" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Commands">
          <RolePlayer>
            <DomainClassMoniker Name="EndpointModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="6fc4ba07-f1e8-41c4-9105-3049b24fa902" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointModelHasCommands.Command" Name="Command" DisplayName="Command" PropertyName="EndpointModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Endpoint Model">
          <RolePlayer>
            <DomainClassMoniker Name="Command" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="0d98957c-a135-4cc1-9957-e0a8ae7b8582" Description="Description for NServiceBus.Modeling.EndpointDesign.CommandIsProcessedBySendReceiveEndpoint" Name="CommandIsProcessedBySendReceiveEndpoint" DisplayName="Command Is Processed By Send Receive Endpoint" Namespace="NServiceBus.Modeling.EndpointDesign" AllowsDuplicates="true">
      <Source>
        <DomainRole Id="3e2895c4-7e5a-4f16-b19f-0e43b57f46c8" Description="Description for NServiceBus.Modeling.EndpointDesign.CommandIsProcessedBySendReceiveEndpoint.Command" Name="Command" DisplayName="Command" PropertyName="ProcessingEndpoint" PropertyDisplayName="Processing Endpoint">
          <RolePlayer>
            <DomainClassMoniker Name="Command" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="2cabc1d3-0678-40d4-8605-2ae32d814a4b" Description="Description for NServiceBus.Modeling.EndpointDesign.CommandIsProcessedBySendReceiveEndpoint.SendReceiveEndpoint" Name="SendReceiveEndpoint" DisplayName="Send Receive Endpoint" PropertyName="ProcessCommands" PropertyDisplayName="Process Commands">
          <RolePlayer>
            <DomainClassMoniker Name="SendReceiveEndpoint" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="435940d8-2cd2-4084-88d5-a68f72be64ea" Description="Description for NServiceBus.Modeling.EndpointDesign.EventsAreProcessedBySendReceiveEndpoints" Name="EventsAreProcessedBySendReceiveEndpoints" DisplayName="Events Are Processed By Send Receive Endpoints" Namespace="NServiceBus.Modeling.EndpointDesign" AllowsDuplicates="true">
      <Source>
        <DomainRole Id="ff85dc3c-5e73-4609-bb53-5b31ad473e61" Description="Description for NServiceBus.Modeling.EndpointDesign.EventsAreProcessedBySendReceiveEndpoints.Event" Name="Event" DisplayName="Event" PropertyName="ProcessingEndpoints" PropertyDisplayName="Processing Endpoints">
          <RolePlayer>
            <DomainClassMoniker Name="Event" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="3058e310-334f-4a61-ab62-d479c32cda75" Description="Description for NServiceBus.Modeling.EndpointDesign.EventsAreProcessedBySendReceiveEndpoints.SendReceiveEndpoint" Name="SendReceiveEndpoint" DisplayName="Send Receive Endpoint" PropertyName="ProcessEvents" PropertyDisplayName="Process Events">
          <RolePlayer>
            <DomainClassMoniker Name="SendReceiveEndpoint" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="2b373871-5fc2-4452-8a0d-944a038c8714" Description="Description for NServiceBus.Modeling.EndpointDesign.SendEndpointEmitsCommands" Name="SendEndpointEmitsCommands" DisplayName="Send Endpoint Emits Commands" Namespace="NServiceBus.Modeling.EndpointDesign" AllowsDuplicates="true">
      <Source>
        <DomainRole Id="870aa2ad-6c36-4c54-ad97-323d578c710a" Description="Description for NServiceBus.Modeling.EndpointDesign.SendEndpointEmitsCommands.SendEndpoint" Name="SendEndpoint" DisplayName="Send Endpoint" PropertyName="EmittedCommands" PropertyDisplayName="Emitted Commands">
          <RolePlayer>
            <DomainClassMoniker Name="SendEndpoint" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="e947f357-9aab-425f-beaf-7801a7bf20bb" Description="Description for NServiceBus.Modeling.EndpointDesign.SendEndpointEmitsCommands.Command" Name="Command" DisplayName="Command" PropertyName="SendEndpoints" PropertyDisplayName="Send Endpoints">
          <RolePlayer>
            <DomainClassMoniker Name="Command" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="2190eda0-a758-40b4-9148-123380940f6f" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpointEmitsEvents" Name="SendReceiveEndpointEmitsEvents" DisplayName="Send Receive Endpoint Emits Events" Namespace="NServiceBus.Modeling.EndpointDesign" AllowsDuplicates="true">
      <Source>
        <DomainRole Id="b190155d-d4c5-4d75-86f5-a6c8f762f886" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpointEmitsEvents.SendReceiveEndpoint" Name="SendReceiveEndpoint" DisplayName="Send Receive Endpoint" PropertyName="EmittedEvents" PropertyDisplayName="Emitted Events">
          <RolePlayer>
            <DomainClassMoniker Name="SendReceiveEndpoint" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="f096d249-d78f-4ca4-bd22-bc67c98269af" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpointEmitsEvents.Event" Name="Event" DisplayName="Event" PropertyName="EmitterEndpoint" PropertyDisplayName="Emitter Endpoint">
          <RolePlayer>
            <DomainClassMoniker Name="Event" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="db478779-0e3f-4614-b8fd-388328833a83" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpointEmitCommands" Name="SendReceiveEndpointEmitCommands" DisplayName="Send Receive Endpoint Emit Commands" Namespace="NServiceBus.Modeling.EndpointDesign" AllowsDuplicates="true">
      <Source>
        <DomainRole Id="96cfb879-d433-459e-a6fa-3d99681f3ede" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpointEmitCommands.SendReceiveEndpoint" Name="SendReceiveEndpoint" DisplayName="Send Receive Endpoint" PropertyName="EmittedCommands" PropertyDisplayName="Emitted Commands">
          <RolePlayer>
            <DomainClassMoniker Name="SendReceiveEndpoint" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="edad5a5f-ab04-496e-a0c0-55423764ff63" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpointEmitCommands.Command" Name="Command" DisplayName="Command" PropertyName="SendReceiveEndpoints" PropertyDisplayName="Send Receive Endpoints">
          <RolePlayer>
            <DomainClassMoniker Name="Command" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <ExternalType Name="EndpointType" Namespace="NServiceBus.Modeling.EndpointDesign.Interfaces" />
  </Types>
  <Shapes>
    <ImageShape Id="42a64d6e-69f8-48ee-bf10-457c0ff5879d" Description="Description for NServiceBus.Modeling.EndpointDesign.CommandShape" Name="CommandShape" DisplayName="Command Shape" Namespace="NServiceBus.Modeling.EndpointDesign" FixedTooltipText="Command Shape" InitialHeight="1" Image="Resources\CommandShape.bmp">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontSize="12" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="OuterBottomCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionDecorator" DisplayName="Description Decorator" DefaultText="DescriptionDecorator" />
      </ShapeHasDecorators>
    </ImageShape>
    <ImageShape Id="8325751f-1cdc-439b-8813-65d33b935a5f" Description="Description for NServiceBus.Modeling.EndpointDesign.EventShape" Name="EventShape" DisplayName="Event Shape" Namespace="NServiceBus.Modeling.EndpointDesign" FixedTooltipText="Event Shape" InitialHeight="1" Image="Resources\EventShape.bmp">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontSize="12" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="OuterBottomCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionDecorator" DisplayName="Description Decorator" DefaultText="DescriptionDecorator" />
      </ShapeHasDecorators>
    </ImageShape>
    <ImageShape Id="ce7c36db-c0a9-4558-bcf2-c4ee59ecb785" Description="Description for NServiceBus.Modeling.EndpointDesign.SendReceiveEndpointShape" Name="SendReceiveEndpointShape" DisplayName="Send Receive Endpoint Shape" Namespace="NServiceBus.Modeling.EndpointDesign" FixedTooltipText="Send Receive Endpoint Shape" InitialHeight="1" Image="Resources\SendReceiveShape.bmp">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontSize="12" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="OuterBottomCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionDecorator" DisplayName="Description Decorator" DefaultText="DescriptionDecorator" />
      </ShapeHasDecorators>
    </ImageShape>
    <ImageShape Id="03f5db15-96f0-4025-9479-61cfb3f8dbb9" Description="Description for NServiceBus.Modeling.EndpointDesign.SendEndpointShape" Name="SendEndpointShape" DisplayName="Send Endpoint Shape" Namespace="NServiceBus.Modeling.EndpointDesign" FixedTooltipText="Send Endpoint Shape" InitialHeight="1" Image="Resources\SendShape.bmp">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontSize="12" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="OuterBottomCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionDecorator" DisplayName="Description Decorator" DefaultText="DescriptionDecorator" />
      </ShapeHasDecorators>
    </ImageShape>
  </Shapes>
  <Connectors>
    <Connector Id="f87c89a2-aa36-4c44-913b-9438f941d5f5" Description="Description for NServiceBus.Modeling.EndpointDesign.EmitConnector" Name="EmitConnector" DisplayName="Emit Connector" Namespace="NServiceBus.Modeling.EndpointDesign" FixedTooltipText="Emit Connector" TargetEndStyle="EmptyArrow" Thickness="0.02" RoutingStyle="Straight" />
    <Connector Id="c342a530-75be-4bfe-8671-4630ee2c1415" Description="Description for NServiceBus.Modeling.EndpointDesign.ProcessConnector" Name="ProcessConnector" DisplayName="Process Connector" Namespace="NServiceBus.Modeling.EndpointDesign" FixedTooltipText="Process Connector" DashStyle="Dash" TargetEndStyle="EmptyArrow" Thickness="0.02" RoutingStyle="Straight" />
  </Connectors>
  <XmlSerializationBehavior Name="EndpointDesignSerializationBehavior" Namespace="NServiceBus.Modeling.EndpointDesign">
    <ClassData>
      <XmlClassData TypeName="EndpointModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="endpointModelMoniker" ElementName="endpointModel" MonikerTypeName="EndpointModelMoniker">
        <DomainClassMoniker Name="EndpointModel" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="events">
            <DomainRelationshipMoniker Name="EndpointModelHasEvents" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="endpoints">
            <DomainRelationshipMoniker Name="EndpointModelHasEndpoints" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="commands">
            <DomainRelationshipMoniker Name="EndpointModelHasCommands" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="SendEndpoint" MonikerAttributeName="" SerializeId="true" MonikerElementName="sendEndpointMoniker" ElementName="sendEndpoint" MonikerTypeName="SendEndpointMoniker">
        <DomainClassMoniker Name="SendEndpoint" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="emittedCommands">
            <DomainRelationshipMoniker Name="SendEndpointEmitsCommands" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EndpointDesignDiagram" MonikerAttributeName="" SerializeId="true" MonikerElementName="endpointDesignDiagramMoniker" ElementName="endpointDesignDiagram" MonikerTypeName="EndpointDesignDiagramMoniker">
        <DiagramMoniker Name="EndpointDesignDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="NamedElement" MonikerAttributeName="name" SerializeId="true" MonikerElementName="namedElementMoniker" ElementName="namedElement" MonikerTypeName="NamedElementMoniker">
        <DomainClassMoniker Name="NamedElement" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="NamedElement/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="NamedElement/Description" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="SendReceiveEndpoint" MonikerAttributeName="" SerializeId="true" MonikerElementName="sendReceiveEndpointMoniker" ElementName="sendReceiveEndpoint" MonikerTypeName="SendReceiveEndpointMoniker">
        <DomainClassMoniker Name="SendReceiveEndpoint" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="emittedEvents">
            <DomainRelationshipMoniker Name="SendReceiveEndpointEmitsEvents" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="emittedCommands">
            <DomainRelationshipMoniker Name="SendReceiveEndpointEmitCommands" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Event" MonikerAttributeName="" SerializeId="true" MonikerElementName="eventMoniker" ElementName="event" MonikerTypeName="EventMoniker">
        <DomainClassMoniker Name="Event" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="processingEndpoints">
            <DomainRelationshipMoniker Name="EventsAreProcessedBySendReceiveEndpoints" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Command" MonikerAttributeName="" SerializeId="true" MonikerElementName="commandMoniker" ElementName="command" MonikerTypeName="CommandMoniker">
        <DomainClassMoniker Name="Command" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="processingEndpoint">
            <DomainRelationshipMoniker Name="CommandIsProcessedBySendReceiveEndpoint" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EndpointModelHasEvents" MonikerAttributeName="" SerializeId="true" MonikerElementName="endpointModelHasEventsMoniker" ElementName="endpointModelHasEvents" MonikerTypeName="EndpointModelHasEventsMoniker">
        <DomainRelationshipMoniker Name="EndpointModelHasEvents" />
      </XmlClassData>
      <XmlClassData TypeName="Endpoint" MonikerAttributeName="" SerializeId="true" MonikerElementName="endpointMoniker" ElementName="endpoint" MonikerTypeName="EndpointMoniker">
        <DomainClassMoniker Name="Endpoint" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="Endpoint/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EndpointModelHasEndpoints" MonikerAttributeName="" SerializeId="true" MonikerElementName="endpointModelHasEndpointsMoniker" ElementName="endpointModelHasEndpoints" MonikerTypeName="EndpointModelHasEndpointsMoniker">
        <DomainRelationshipMoniker Name="EndpointModelHasEndpoints" />
      </XmlClassData>
      <XmlClassData TypeName="EmitConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="emitConnectorMoniker" ElementName="emitConnector" MonikerTypeName="EmitConnectorMoniker">
        <ConnectorMoniker Name="EmitConnector" />
      </XmlClassData>
      <XmlClassData TypeName="ProcessConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="processConnectorMoniker" ElementName="processConnector" MonikerTypeName="ProcessConnectorMoniker">
        <ConnectorMoniker Name="ProcessConnector" />
      </XmlClassData>
      <XmlClassData TypeName="EndpointModelHasCommands" MonikerAttributeName="" SerializeId="true" MonikerElementName="endpointModelHasCommandsMoniker" ElementName="endpointModelHasCommands" MonikerTypeName="EndpointModelHasCommandsMoniker">
        <DomainRelationshipMoniker Name="EndpointModelHasCommands" />
      </XmlClassData>
      <XmlClassData TypeName="CommandShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="commandShapeMoniker" ElementName="commandShape" MonikerTypeName="CommandShapeMoniker">
        <ImageShapeMoniker Name="CommandShape" />
      </XmlClassData>
      <XmlClassData TypeName="CommandIsProcessedBySendReceiveEndpoint" MonikerAttributeName="" SerializeId="true" MonikerElementName="commandIsProcessedBySendReceiveEndpointMoniker" ElementName="commandIsProcessedBySendReceiveEndpoint" MonikerTypeName="CommandIsProcessedBySendReceiveEndpointMoniker">
        <DomainRelationshipMoniker Name="CommandIsProcessedBySendReceiveEndpoint" />
      </XmlClassData>
      <XmlClassData TypeName="EventsAreProcessedBySendReceiveEndpoints" MonikerAttributeName="" SerializeId="true" MonikerElementName="eventsAreProcessedBySendReceiveEndpointsMoniker" ElementName="eventsAreProcessedBySendReceiveEndpoints" MonikerTypeName="EventsAreProcessedBySendReceiveEndpointsMoniker">
        <DomainRelationshipMoniker Name="EventsAreProcessedBySendReceiveEndpoints" />
      </XmlClassData>
      <XmlClassData TypeName="EventShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="eventShapeMoniker" ElementName="eventShape" MonikerTypeName="EventShapeMoniker">
        <ImageShapeMoniker Name="EventShape" />
      </XmlClassData>
      <XmlClassData TypeName="SendReceiveEndpointShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="sendReceiveEndpointShapeMoniker" ElementName="sendReceiveEndpointShape" MonikerTypeName="SendReceiveEndpointShapeMoniker">
        <ImageShapeMoniker Name="SendReceiveEndpointShape" />
      </XmlClassData>
      <XmlClassData TypeName="SendEndpointShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="sendEndpointShapeMoniker" ElementName="sendEndpointShape" MonikerTypeName="SendEndpointShapeMoniker">
        <ImageShapeMoniker Name="SendEndpointShape" />
      </XmlClassData>
      <XmlClassData TypeName="SendEndpointEmitsCommands" MonikerAttributeName="" SerializeId="true" MonikerElementName="sendEndpointEmitsCommandsMoniker" ElementName="sendEndpointEmitsCommands" MonikerTypeName="SendEndpointEmitsCommandsMoniker">
        <DomainRelationshipMoniker Name="SendEndpointEmitsCommands" />
      </XmlClassData>
      <XmlClassData TypeName="SendReceiveEndpointEmitsEvents" MonikerAttributeName="" SerializeId="true" MonikerElementName="sendReceiveEndpointEmitsEventsMoniker" ElementName="sendReceiveEndpointEmitsEvents" MonikerTypeName="SendReceiveEndpointEmitsEventsMoniker">
        <DomainRelationshipMoniker Name="SendReceiveEndpointEmitsEvents" />
      </XmlClassData>
      <XmlClassData TypeName="SendReceiveEndpointEmitCommands" MonikerAttributeName="" SerializeId="true" MonikerElementName="sendReceiveEndpointEmitCommandsMoniker" ElementName="sendReceiveEndpointEmitCommands" MonikerTypeName="SendReceiveEndpointEmitCommandsMoniker">
        <DomainRelationshipMoniker Name="SendReceiveEndpointEmitCommands" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="EndpointDesignerExplorer" />
  <ConnectionBuilders>
    <ConnectionBuilder Name="ProcessConnectionBuilder" IsCustom="true">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="CommandIsProcessedBySendReceiveEndpoint" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Command" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="SendReceiveEndpoint" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EventsAreProcessedBySendReceiveEndpoints" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Event" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="SendReceiveEndpoint" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="EmitConnectionBuilder" IsCustom="true">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="SendReceiveEndpointEmitsEvents" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="SendReceiveEndpoint" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Event" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="SendEndpointEmitsCommands" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="SendEndpoint" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Command" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="SendReceiveEndpointEmitCommands" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="SendReceiveEndpoint" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Command" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="b6fa988a-9a81-4e59-8960-eb357c6aae1d" Description="Description for NServiceBus.Modeling.EndpointDesign.EndpointDesignDiagram" Name="EndpointDesignDiagram" DisplayName="EndpointDesignDiagram" Namespace="NServiceBus.Modeling.EndpointDesign">
    <Class>
      <DomainClassMoniker Name="EndpointModel" />
    </Class>
    <ShapeMaps>
      <ShapeMap>
        <DomainClassMoniker Name="Command" />
        <ParentElementPath>
          <DomainPath>EndpointModelHasCommands.EndpointModel/!EndpointModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="CommandShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="CommandShape/DescriptionDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <ImageShapeMoniker Name="CommandShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="Event" />
        <ParentElementPath>
          <DomainPath>EndpointModelHasEvents.EndpointModel/!EndpointModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EventShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="CommandShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EventShape/DescriptionDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <ImageShapeMoniker Name="EventShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="SendReceiveEndpoint" />
        <ParentElementPath>
          <DomainPath>EndpointModelHasEndpoints.EndpointModel/!EndpointModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="SendReceiveEndpointShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="SendReceiveEndpointShape/DescriptionDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <ImageShapeMoniker Name="SendReceiveEndpointShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="SendEndpoint" />
        <ParentElementPath>
          <DomainPath>EndpointModelHasEndpoints.EndpointModel/!EndpointModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="SendEndpointShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="SendEndpointShape/DescriptionDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="NamedElement/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <ImageShapeMoniker Name="SendEndpointShape" />
      </ShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="ProcessConnector" />
        <DomainRelationshipMoniker Name="EventsAreProcessedBySendReceiveEndpoints" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ProcessConnector" />
        <DomainRelationshipMoniker Name="CommandIsProcessedBySendReceiveEndpoint" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="EmitConnector" />
        <DomainRelationshipMoniker Name="SendEndpointEmitsCommands" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="EmitConnector" />
        <DomainRelationshipMoniker Name="SendReceiveEndpointEmitsEvents" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="EmitConnector" />
        <DomainRelationshipMoniker Name="SendReceiveEndpointEmitCommands" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="endpoints" EditorGuid="40762e29-3159-47d9-8766-9501177c7142">
    <RootClass>
      <DomainClassMoniker Name="EndpointModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="false">
      <XmlSerializationBehaviorMoniker Name="EndpointDesignSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="NServiceBus">
      <ElementTool Name="SendEndpoint" ToolboxIcon="Resources\Send.bmp" Caption="Send-only endpoint" Tooltip="Creates an Send-only endpoint" HelpKeyword="">
        <DomainClassMoniker Name="SendEndpoint" />
      </ElementTool>
      <ElementTool Name="SendReceiveEndpoint" ToolboxIcon="Resources\SendReceive.bmp" Caption="Send/receive endpoint" Tooltip="Creates an Send/receive endpoint" HelpKeyword="">
        <DomainClassMoniker Name="SendReceiveEndpoint" />
      </ElementTool>
      <ElementTool Name="Event" ToolboxIcon="Resources\Event.bmp" Caption="Event" Tooltip="Creates an Event" HelpKeyword="">
        <DomainClassMoniker Name="Event" />
      </ElementTool>
      <ElementTool Name="Command" ToolboxIcon="Resources\Command.bmp" Caption="Command" Tooltip="Creates an Command" HelpKeyword="">
        <DomainClassMoniker Name="Command" />
      </ElementTool>
      <ConnectionTool Name="Process" ToolboxIcon="Resources\Process.bmp" Caption="Process" Tooltip="Process" HelpKeyword="Process">
        <ConnectionBuilderMoniker Name="EndpointDesign/ProcessConnectionBuilder" />
      </ConnectionTool>
      <ConnectionTool Name="Emit" ToolboxIcon="Resources\Emit.bmp" Caption="Emit" Tooltip="Emit" HelpKeyword="Emit">
        <ConnectionBuilderMoniker Name="EndpointDesign/EmitConnectionBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="false" UsesSave="true" UsesLoad="false" />
    <DiagramMoniker Name="EndpointDesignDiagram" />
  </Designer>
  <Explorer ExplorerGuid="90bc3db7-ebd2-48b6-b29c-3de758c8c06a" Title="EndpointDesigner Explorer">
    <ExplorerBehaviorMoniker Name="EndpointDesign/EndpointDesignerExplorer" />
  </Explorer>
</Dsl>