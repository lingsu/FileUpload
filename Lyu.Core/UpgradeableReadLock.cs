using System;
using System.Threading;

namespace Lyu.Core
{
    /// <summary>
    /// 提供了一种方便的方法来实现锁定访问资源。
    /// </summary>
    /// <remarks>
    /// Intended as an infrastructure class.
    /// </remarks>
    public class UpgradeableReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;
        private bool _upgraded = false;

        /// <summary>
		/// Initializes a new instance of the <see cref="ReadLock"/> class.
        /// </summary>
        /// <param name="rwLock">The rw lock.</param>
		public UpgradeableReadLock(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterUpgradeableReadLock();
        }

        public void UpgradeToWriteLock()
        {
            _rwLock.EnterWriteLock();
            _upgraded = true;
        }

        void IDisposable.Dispose()
        {
            if (_upgraded)
            {
                _rwLock.ExitWriteLock();
            }
            _rwLock.ExitUpgradeableReadLock();
        }
    }
}