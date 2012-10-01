using System.Linq;
using Microsoft.VisualStudio.Modeling;
using System.Diagnostics;
using System;

namespace NServiceBus.Modeling.EndpointDesign
{
	public static partial class ProcessConnectionBuilder
	{
		public static bool CanAcceptSource(ModelElement candidate)
		{
			if(candidate == null)
			{
				return false;
			}
			else if(candidate is Event)
			{
				return true;
			}
			else if(candidate is Command)
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
			else if(candidate is SendReceiveEndpoint)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool CanAcceptSourceAndTarget(ModelElement candidateSource, ModelElement candidateTarget)
		{
			// Accepts null, null; source, null; source, target but NOT null, target
			if(candidateSource == null)
			{
				if(candidateTarget != null)
				{
					throw new ArgumentNullException("candidateSource");
				}
				else // Both null
				{
					return false;
				}
			}
			bool acceptSource = CanAcceptSource(candidateSource);
			// If the source wasn't accepted then there's no point checking targets.
			// If there is no target then the source controls the accept.
			if(!acceptSource || candidateTarget == null)
			{
				return acceptSource;
			}
			else // Check combinations
			{
				if(candidateSource is Event)
				{
					if(candidateTarget is SendReceiveEndpoint)
					{
						Event sourceEvent = (Event)candidateSource;
						SendReceiveEndpoint targetSendReceiveEndpoint = (SendReceiveEndpoint)candidateTarget;
						if(targetSendReceiveEndpoint == null || sourceEvent == null || EventsAreProcessedBySendReceiveEndpoints.GetLinks(sourceEvent, targetSendReceiveEndpoint).Count > 0) return false;

						if(targetSendReceiveEndpoint.EmittedEvents.Any(e => e == sourceEvent))
						{
							return false;
						}

						return true;
					}
				}
				if(candidateSource is Command)
				{
					if(candidateTarget is SendReceiveEndpoint)
					{
						Command sourceCommand = (Command)candidateSource;
						SendReceiveEndpoint targetSendReceiveEndpoint = (SendReceiveEndpoint)candidateTarget;
						if(sourceCommand == null || CommandIsProcessedBySendReceiveEndpoint.GetLinksToProcessingEndpoint(sourceCommand) != null) return false;
						if(targetSendReceiveEndpoint == null || sourceCommand == null || CommandIsProcessedBySendReceiveEndpoint.GetLinks(sourceCommand, targetSendReceiveEndpoint).Count > 0) return false;

						if(targetSendReceiveEndpoint.EmittedCommands.Any(c => c == sourceCommand))
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
				throw new ArgumentNullException("source");
			}
			if(target == null)
			{
				throw new ArgumentNullException("target");
			}

			if(CanAcceptSourceAndTarget(source, target))
			{
				if(source is Event)
				{
					if(target is SendReceiveEndpoint)
					{
						Event sourceAccepted = (Event)source;
						SendReceiveEndpoint targetAccepted = (SendReceiveEndpoint)target;
						ElementLink result = new EventsAreProcessedBySendReceiveEndpoints(sourceAccepted, targetAccepted);
						if(DomainClassInfo.HasNameProperty(result))
						{
							DomainClassInfo.SetUniqueName(result);
						}
						return result;
					}
				}
				if(source is Command)
				{
					if(target is SendReceiveEndpoint)
					{
						Command sourceAccepted = (Command)source;
						SendReceiveEndpoint targetAccepted = (SendReceiveEndpoint)target;
						ElementLink result = new CommandIsProcessedBySendReceiveEndpoint(sourceAccepted, targetAccepted);
						if(DomainClassInfo.HasNameProperty(result))
						{
							DomainClassInfo.SetUniqueName(result);
						}
						return result;
					}
				}

			}
			Debug.Fail("Having agreed that the connection can be accepted we should never fail to make one.");
			throw new InvalidOperationException();
		}
	}
}
