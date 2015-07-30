using System;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace Sharper.C.Data
{
    public static class LazyFn
    {
        [MethodImpl(AggressiveInlining)]
        public static Lazy<A> Defer<A>(Func<A> f) => new Lazy<A>(f);

        public static Func<A> Thunk<A>(Lazy<A> la) => () => la.Value;

        public static Lazy<B> Map<A, B>(this Lazy<A> la, Func<A, B> f)
            => Defer(() => f(la.Value));

        public static Lazy<B> FlatMap<A, B>(this Lazy<A> la, Func<A, Lazy<B>> f)
            => Defer(() => f(la.Value).Value);

        [MethodImpl(AggressiveInlining)]
        public static Lazy<B> Select<A, B>(this Lazy<A> la, Func<A, B> f)
            => Map(la, f);

        public static Lazy<C> SelectMany<A, B, C>(
            this Lazy<A> la,
            Func<A, Lazy<B>> f,
            Func<A, B, C> g) => Defer(
                () =>
                {
                    var a = la.Value;
                    var b = f(a).Value;
                    return g(a, b);
                });
    }
}
