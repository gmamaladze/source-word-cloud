using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Gma.CodeCloud.Base
{
public static class PlinqCallbackExtensions
{

    public static ParallelQuery<TSource> WithCallback<TSource>(this ParallelQuery<TSource> source, Action<TSource> callback)
    {
        return source.Select(
            item =>
                {
                    callback.Invoke(item);
                    return item;
                });
    }

    //public static IEnumerable<TSource> WithCallback<TSource>(this IEnumerable<TSource> source, Action<TSource> callback)
    //{
    //    return source.Select(
    //        item =>
    //        {
    //            callback.Invoke(item);
    //            return item;
    //        });
    //}
}
}