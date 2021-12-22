using System;
using System.Threading;

namespace UsefulThings
{

    /// <summary>
    ///     Provides access to a threadsafe random.
    ///     I dunno why this really exists. Used by shuffle.
    /// </summary>
    public static class ThreadSafeRandom
    {

        [ThreadStatic]
        private static Random Local;

        /// <summary>
        ///     Gets a threadsafe random.
        /// </summary>
        public static Random ThisThreadsRandom =>
            Local ??
            ( Local = new Random( unchecked( Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId ) ) );

    }

}
