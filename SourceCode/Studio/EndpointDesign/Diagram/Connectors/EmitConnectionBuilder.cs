using System.Linq;
using Microsoft.VisualStudio.Modeling;

namespace NServiceBus.Modeling.EndpointDesign
{
	public static partial class EmitConnectionBuilder
	{
		public static bool CanAcceptSource(ModelElement candidate)
		{
			if(candidate == null)
			{
				return false;
			}
			else if(candidate is SendEndpoint)
			{
				return true;
			}
			else if(candidate is SendReceiveEndpoint)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool CanAcceptTarget(ModelElement candidate)
		{
			if(candidate == null)
			{
				return false;
			}
			else if(candidate is Command)
			{
				return true;
			}
			else if(candidate is Event)
			{
				return true;
			}
			else
				return false;
		}

		public static bool CanAcceptSourceAndTarget(ModelElement candidateSource, ModelElement candidateTarget)
		{
			if(candidateSource == null)
			{
				if(candidateTarget != null)
				{
					throw new global::System.ArgumentNullException("candidateSource");
				}
				else
				{
					return false;
				}
			}

			var acceptSource = CanAcceptSource(candidateSource);

			if(!acceptSource || candidateTarget == null)
			{
				return acceptSource;
			}
			else
			{
				if(candidateSource is SendEndpoint)
				{
					if(candidateTarget is Command)
					{
						var sourceSendEndpoint = (SendEndpoint)candidateSource;
						var targetCommand = (Command)candidateTarget;
						if(targetCommand == null || SendEndpointEmitsCommands.GetLinksToSendEndpoints(targetCommand) != null) return false;
						if(targetCommand == null || sourceSendEndpoint == null || SendEndpointEmitsCommands.GetLinks(sourceSendEndpoint, targetCommand).Count > 0) return false;
						return true;
					}
				}
				if(candidateSource is SendReceiveEndpoint)
				{
					if(candidateTarget is Command)
					{
						var sourceSendReceiveEndpoint = (SendReceiveEndpoint)candidateSource;
						var targetCommand = (Command)candidateTarget;
						if(targetCommand == null || SendReceiveEndpointEmitCommands.GetLinksToSendReceiveEndpoints(targetCommand) != null) return false;
						if(targetCommand == null || sourceSendReceiveEndpoint == null || SendReceiveEndpointEmitCommands.GetLinks(sourceSendReceiveEndpoint, targetCommand).Count > 0) return false;

						if(sourceSendReceiveEndpoint.ProcessCommands.Any(c => c == targetCommand))
						{
							return false;
						}
						
						return true;
					}
					else if(candidateTarget is Event)
					{
						var sourceSendReceiveEndpoint = (SendReceiveEndpoint)candidateSource;
						var targetEvent = (Event)candidateTarget;
						if(targetEvent == null || SendReceiveEndpointEmitsEvents.GetLinksToEmitterEndpoint(targetEvent) != null) return false;
						if(targetEvent == null || sourceSendReceiveEndpoint == null || SendReceiveEndpointEmitsEvents.GetLinks(sourceSendReceiveEndpoint, targetEvent).Count > 0) return false;

						if(sourceSendReceiveEndpoint.ProcessEvents.Any(e => e == targetEvent))
						{
							return false;
						}
						
						return true;
					}
				}

			}

			return false;
		}

		public static ElementLink Connect(ModelElement source, ModelElement target)
		{
			if(source == null)
			{
				throw new global::System.ArgumentNullException("source");
			}
			if(target == null)
			{
				throw new global::System.ArgumentNullException("target");
			}

			if(CanAcceptSourceAndTarget(source, target))
			{
				if(source is SendEndpoint)
				{
					if(target is Command)
					{
						SendEndpoint sourceAccepted = (SendEndpoint)source;
						Command targetAccepted = (Command)target;
						ElementLink result = new SendEndpointEmitsCommands(sourceAccepted, targetAccepted);
						if(DomainClassInfo.HasNameProperty(result))
						{
							DomainClassInfo.SetUniqueName(result);
						}
						return result;
					}
				}
				if(source is SendReceiveEndpoint)
				{
					if(target is Command)
					{
						SendReceiveEndpoint sourceAccepted = (SendReceiveEndpoint)source;
						Command targetAccepted = (Command)target;
						ElementLink result = new SendReceiveEndpointEmitCommands(sourceAccepted, targetAccepted);
						if(DomainClassInfo.HasNameProperty(result))
						{
							DomainClassInfo.SetUniqueName(result);
						}
						return result;
					}
					else if(target is Event)
					{
						SendReceiveEndpoint sourceAccepted = (SendReceiveEndpoint)source;
						Event targetAccepted = (Event)target;
						ElementLink result = new SendReceiveEndpointEmitsEvents(sourceAccepted, targetAccepted);
						if(DomainClassInfo.HasNameProperty(result))
						{
							DomainClassInfo.SetUniqueName(result);
						}
						return result;
					}
				}

			}
			global::System.Diagnostics.Debug.Fail("Having agreed that the connection can be accepted we should never fail to make one.");
			throw new global::System.InvalidOperationException();
		}
	}
}
