using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace NServiceBus.Modeling.EndpointDesign
{
    [DebuggerStepThrough]
    public static class Guard
    {
        public static void NotNull<T>(Expression<Func<T>> reference, T value)
        {
            if (value == null)
            {
				throw new ArgumentNullException(GetParameterName(reference), "Parameter cannot be null.");
            }
        }

        public static void NotNullOrEmpty(Expression<Func<string>> reference, string value)
        {
            NotNull(reference, value);

            if (value.Length == 0)
            {
				throw new ArgumentOutOfRangeException(GetParameterName(reference), "Parameter cannot be empty.");
            }
        }

        private static string GetParameterName<T>(Expression<T> reference)
        {
            return ((MemberExpression)reference.Body).Member.Name;
        }
    }
}