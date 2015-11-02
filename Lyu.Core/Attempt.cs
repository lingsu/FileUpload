using System;

namespace Lyu.Core
{
    /// <summary>
    /// 提供方法来创建尝试。
    /// </summary>
    public static class Attempt
    {
        /// <summary>
        /// 创造一个成功的尝试，结果。
        /// </summary>
        /// <typeparam name="T">尝试操作结果的类型.</typeparam>
        /// <param name="result">尝试的结果.</param>
        /// <returns>成功的尝试.</returns>
        public static Attempt<T> Succeed<T>(T result)
        {
            return Attempt<T>.Succeed(result);
        }

        /// <summary>
        /// 创建失败的尝试结果.
        /// </summary>
        /// <typeparam name="T">尝试操作结果的类型.</typeparam>
        /// <param name="result">尝试的结果.</param>
        /// <returns>The failed attempt.</returns>
        public static Attempt<T> Fail<T>(T result)
        {
            return Attempt<T>.Fail(result);
        }

        /// <summary>
        /// 创建失败的尝试结果和异常.
        /// </summary>
        /// <typeparam name="T">尝试操作结果的类型.</typeparam>
        /// <param name="result">尝试的结果.</param>
        /// <param name="exception">尝试的失败造成的异常.</param>
        /// <returns>The failed attempt.</returns>
        public static Attempt<T> Fail<T>(T result, Exception exception)
        {
            return Attempt<T>.Fail(result, exception);
        }

        /// <summary>
        /// 根据结果创建一个成功或失败的尝试.
        /// </summary>
        /// <typeparam name="T">尝试操作结果的类型.</typeparam>
        /// <param name="success">一个值，该值指示是否成功.</param>
        /// <param name="result">尝试的结果.</param>
        /// <returns>The attempt.</returns>
        public static Attempt<T> If<T>(bool success, T result)
        {
            return Attempt<T>.SucceedIf(success, result);
        }


        /// <summary>
        /// 执行回调函数
        /// </summary>
        /// <typeparam name="T">尝试操作结果的类型.</typeparam>
        /// <param name="attempt">尝试函数返回的尝试.</param>
        /// <param name="onSuccess">在尝试成功的情况下执行的操作.</param>
        /// <param name="onFail">在尝试失败时执行的操作.</param>
        /// <returns>尝试的结果.</returns>
        /// <remarks>根据<paramref name="onSuccess"/> 或 <paramref name="onFail"/> 运行
        /// 尝试功能报告是否成功或失败.</remarks>
        public static Outcome Try<T>(Attempt<T> attempt, Action<T> onSuccess, Action<Exception> onFail = null)
        {
            if (attempt.Success)
            {
                onSuccess(attempt.Result);
                return Outcome.Success;
            }

            if (onFail != null)
            {
                onFail(attempt.Exception);
            }

            return Outcome.Failure;
        }

        /// <summary>
        /// 代表尝试的结果.
        /// </summary>
        /// <remarks> 可以是成功还是失败，可以链式尝试 </remarks>
        public struct Outcome
        {
            private readonly bool _success;

            /// <summary>
            /// 取得成功的结果
            /// </summary>
            public static readonly Outcome Success = new Outcome(true);

            /// <summary>
            /// 取得失败的结果.
            /// </summary>
            public static readonly Outcome Failure = new Outcome(false);

            private Outcome(bool success)
            {
                _success = success;
            }

            /// <summary>
            /// 执行另一个函数,如果前一个失败则回调函数
            /// </summary>
            /// <typeparam name="T">尝试操作结果的类型.</typeparam>
            /// <param name="nextFunction">尝试执行，返回一个尝试.</param>
            /// <param name="onSuccess">在尝试成功的情况下执行的操作.</param>
            /// <param name="onFail">在尝试失败时执行的操作.</param>
            /// <returns>如果它执行时,返回的结果尝试,否则返回一个成功的结果.</returns>
            /// <remarks>
            /// <para>仅当先前的尝试失败，否则不执行和返回一个成功结果.</para>
            /// <para>如果它执行，然后运行 <paramref name="onSuccess"/> 或 <paramref name="onFail"/> 根据不同
            /// 尝试是成功或失败.</para>
            /// </remarks>
            public Outcome OnFailure<T>(Func<Attempt<T>> nextFunction, Action<T> onSuccess, Action<Exception> onFail = null)
            {
                return _success
                    ? Success
                    : ExecuteNextFunction(nextFunction, onSuccess, onFail);
            }

            /// <summary>
            ///执行一次函数，如果前一个成功了，而回调
            /// </summary>
            /// <typeparam name="T">尝试操作结果的类型.</typeparam>
            /// <param name="nextFunction">尝试执行，返回一个尝试.</param>
            /// <param name="onSuccess">在尝试成功的情况下执行的操作.</param>
            /// <param name="onFail">在尝试失败时执行的操作.</param>
            /// <returns>如果它执行，返回尝试的结果，否则返回一个失败的结果.</returns>
            /// <remarks>
            /// <para>只执行，如果先前的尝试成功，否则不执行和返回一个成功的结果.</para>
            /// <para>根据结果 运行 <paramref name="onSuccess"/> 或 <paramref name="onFail"/> </para>
            /// </remarks>
            public Outcome OnSuccess<T>(Func<Attempt<T>> nextFunction, Action<T> onSuccess, Action<Exception> onFail = null)
            {
                return _success
                    ? ExecuteNextFunction(nextFunction, onSuccess, onFail)
                    : Failure;
            }

            private static Outcome ExecuteNextFunction<T>(Func<Attempt<T>> nextFunction, Action<T> onSuccess, Action<Exception> onFail = null)
            {
                var attempt = nextFunction();

                if (attempt.Success)
                {
                    onSuccess(attempt.Result);
                    return Success;
                }

                if (onFail != null)
                {
                    onFail(attempt.Exception);
                }

                return Failure;
            }
        }

    }
}