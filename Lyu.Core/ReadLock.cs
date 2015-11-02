using System;
using System.Threading;

namespace Lyu.Core
{
    /// <summary>
    ///提供了一个方便的方法实现对锁定访问资源
    /// </summary>
    /// <remarks>
    /// 作为一个基础设施类
    /// </remarks>
    public class ReadLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
		/// Initializes a new instance of the <see cref="ReadLock"/> class.
        /// </summary>
		public ReadLock(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterReadLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitReadLock();
        }
    }
}