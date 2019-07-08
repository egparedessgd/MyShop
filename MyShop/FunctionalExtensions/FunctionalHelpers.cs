using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FunctionalExtensions
{
    public static class FunctionalHelpers
    {
        public static TOut MapNullable<TObject, TOut>
            (Func<TObject> valueFunction, Func<TObject, TOut> OnNotNull, Func<TObject, TOut> OnNull)
            where TObject : class
        {
            TObject t = valueFunction();
            if (t == null)
            {
                return OnNull(t);
            }
            else
            {
                return OnNotNull(t);
            }
        }

        public static Func<Func<TObject>, Func<TObject, TOut>, TOut> MapNullableSetNotFound<TObject, TOut>(Func<TObject, TOut> OnNotFound)
            where TObject : class =>
            (valueFunction, OnNotNull) => MapNullable(valueFunction, OnNotNull, OnNotFound);

        public static Func<T1, T2, TResult> ApplyPartial<T1, T2, T3, TResult>
            (this Func<T1, T2, T3, TResult> function, T3 tailArg) =>
            (t1, t2) => function(t1, t2, tailArg);

        public static Func<T1, TResult> ApplyPartial<T1, T2, TResult>
            (this Func<T1, T2, TResult> function, T2 tailArg) =>
            (t1) => function(t1, tailArg);
    }
}
