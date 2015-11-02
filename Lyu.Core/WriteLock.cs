using System;
using System.Threading;

namespace Lyu.Core
{
    /// <summary>
    /// 提供了一种方便的方法来实现锁定访问资源
    /// </summary>
    /// <remarks>
    /// Intended as an infrastructure class.
    /// </remarks>
    public class WriteLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLock"/> class.
        /// </summary>
        /// <param name="rwLock">The rw lock.</param>
        public WriteLock(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterWriteLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }
    }
}