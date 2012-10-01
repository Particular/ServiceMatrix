using System;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;

namespace NServiceBus.Modeling.EndpointDesign
{
    public static class TransactionManagerExtensions
    {
        public static void DoWithinTransaction(this TransactionManager manager, Action action)
        {
            DoWithinTransaction(manager, action, string.Empty, false);
        }

        public static void DoWithinTransaction(this TransactionManager manager, Action action, string transactionName)
        {
            DoWithinTransaction(manager, action, transactionName, false);
        }

        public static void DoWithinTransaction(this TransactionManager manager, Action action, bool serializing)
        {
            DoWithinTransaction(manager, action, string.Empty, serializing);
        }

        public static void DoWithinTransaction(this TransactionManager manager, Action action, string transactionName, bool serializing)
        {
            Guard.NotNull(() => manager, manager);
            Guard.NotNull(() => action, action);

            var transacName = transactionName;

            if (string.IsNullOrEmpty(transacName))
            {
                transacName = string.Format(
                    CultureInfo.CurrentCulture,
                    "Executing {0} @ {1}",
                    action.Method.Name,
                    DateTime.Now.ToString());
            }

            if (manager.Store.InUndoRedoOrRollback || manager.InTransaction || (manager.CurrentTransaction != null && manager.CurrentTransaction.InRollback))
            {
                // Do not create nested transaction in rollback scenarios.
                action();
            }
            else
            {
                using (var tx = manager.BeginTransaction(transacName, serializing))
                {
                    action();
                    tx.Commit();
                }
            }
        }
    }
}