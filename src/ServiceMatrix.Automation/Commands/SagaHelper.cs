using System;
using System.Globalization;
using System.Windows;
using NServiceBusStudio.Automation.Properties;

namespace NServiceBusStudio.Automation.Commands
{
    class SagaHelper
    {
        public static void CheckAndPromptForSagaUpdate(IComponent handlerComponent, IMessageBoxService messageBoxService, IDialogWindowFactory windowFactory)
        {
            if (handlerComponent.ProcessesMultipleMessages)
            {
                var sagaRecommendationMessage =
                    handlerComponent.IsSaga
                        ? String.Format(Resources.Saga_UpdateQuery)
                        : String.Format(CultureInfo.CurrentCulture, Resources.Saga_ConvertQuery, handlerComponent.CodeIdentifier);

                var result = messageBoxService.Show(sagaRecommendationMessage, Resources.Saga_QueryTitle, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    new ShowComponentSagaStarterPicker { WindowFactory = windowFactory, CurrentElement = handlerComponent }.Execute();
                }
            }
        }
    }
}