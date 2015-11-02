using System;

namespace Lyu.Core
{
    /// <summary>
    /// 表示尝试操作的结果。
    /// </summary>
    /// <typeparam name="T">.尝试操作结果的类型</typeparam>
    [Serializable]
    public struct Attempt<T>
    {
        private readonly bool _success;
        private readonly T _result;
        private readonly Exception _exception;

        /// <summary>
        /// 获取一个值，该值指示是否该<see cref="Attempt{T}"/> 是成功的。
        /// </summary>
        public bool Success
        {
            get { return _success; }
        }

        /// <summary>
        /// 获取失败的异常结果
        /// </summary>
        public Exception Exception { get { return _exception; } }

        /// <summary>
        /// 获取结果
        /// </summary>
        public T Result
        {
            get { return _result; }
        }

        //优化，使用单例的失败尝试
        private static readonly Attempt<T> Failed = new Attempt<T>(false, default(T), null);

        // private - use Succeed() or Fail() methods to create attempts
        private Attempt(bool success, T result, Exception exception)
        {
            _success = success;
            _result = result;
            _exception = exception;
        }

        /// <summary>
        /// 创建一个成功的尝试
        /// </summary>
        /// <returns>The successful attempt.</returns>
        public static Attempt<T> Succeed()
        {
            return new Attempt<T>(true, default(T), null);
        }

        /// <summary>
        /// 创建一个成功的尝试结果
        /// </summary>
        /// <param name="result">尝试的结果</param>
        /// <returns>成功的尝试</returns>
        public static Attempt<T> Succeed(T result)
        {
            return new Attempt<T>(true, result, null);
        }

        /// <summary>
        /// 创建一个失败的尝试
        /// </summary>
        /// <returns>失败的尝试</returns>
        public static Attempt<T> Fail()
        {
            return Failed;
        }

        /// <summary>
        /// 创建一个失败的尝试
        /// </summary>
        /// <param name="exception">尝试的失败造成的异常</param>
        /// <returns>失败的尝试</returns>
        public static Attempt<T> Fail(Exception exception)
        {
            return new Attempt<T>(false, default(T), exception);
        }

        /// <summary>
        ///创建失败的尝试结果
        /// </summary>
        /// <param name="result">尝试的结果</param>
        /// <returns>失败的尝试.</returns>
        public static Attempt<T> Fail(T result)
        {
            return new Attempt<T>(false, result, null);
        }

        /// <summary>
        /// 创建失败的尝试结果
        /// </summary>
        /// <param name="result">尝试的结果.</param>
        /// <param name="exception">尝试的失败造成的异常.</param>
        /// <returns>失败的尝试.</returns>
        public static Attempt<T> Fail(T result, Exception exception)
        {
            return new Attempt<T>(false, result, exception);
        }

        /// <summary>
        /// 创建一个成功或失败的尝试
        /// </summary>
        /// <param name="condition">一个值，该值指示是否成功</param>
        /// <returns>这次尝试.</returns>
        public static Attempt<T> SucceedIf(bool condition)
        {
            return condition ? new Attempt<T>(true, default(T), null) : Failed;
        }

        /// <summary>
        /// 根据结果创建一个成功或失败的尝试
        /// </summary>
        /// <param name="condition">一个值，该值指示是否成功.</param>
        /// <param name="result">尝试的结果.</param>
        /// <returns>这次尝试.</returns>
        public static Attempt<T> SucceedIf(bool condition, T result)
        {
            return new Attempt<T>(condition, result, null);
        }

        public static implicit operator bool (Attempt<T> a)
        {
            return a.Success;
        }
    }
}