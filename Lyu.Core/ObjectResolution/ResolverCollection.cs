using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Lyu.Core.ObjectResolution
{
    /// <summary>
    /// 只是用来跟踪所有ManyObjectsResolverBase实例,这样我们可以
    /// 重置它们所有一次真的很容易。
    /// </summary>
    /// <remarks>
    /// 通常我们会用typefinding这但因为许多解析器内部这行不通。
    /// 我们宁愿不保持静态列表让我们动态地添加到这个列表根据基地
    /// ManyObjectsResolverBase的类。
    /// </remarks>
    internal static class ResolverCollection
    {
        private static readonly ConcurrentDictionary<ResolverBase, Action> Resolvers = new ConcurrentDictionary<ResolverBase, Action>();

        /// <summary>
        /// Returns the number of resolvers created
        /// </summary>
        internal static int Count
        {
            get { return Resolvers.Count; }
        }

        /// <summary>
        /// Resets all resolvers
        /// </summary>
        internal static void ResetAll()
        {
            //take out each item from the bag and reset it
            var keys = Resolvers.Keys.ToArray();
            foreach (var k in keys)
            {
                Action resetAction;
                while (Resolvers.TryRemove(k, out resetAction))
                {
                    //call the reset action for the resolver
                    resetAction();
                }
            }
        }

        /// <summary>
        /// This is called when the static Reset method or a ResolverBase{T} is called.
        /// </summary>
        internal static void Remove(ResolverBase resolver)
        {
            if (resolver == null) return;
            Action action;
            Resolvers.TryRemove(resolver, out action);
        }

        /// <summary>
        /// Adds a resolver to the collection
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="resetAction"></param>
        /// <remarks>
        /// This is called when the creation of a ResolverBase occurs
        /// </remarks>
        internal static void Add(ResolverBase resolver, Action resetAction)
        {
            Resolvers.TryAdd(resolver, resetAction);
        }
    }
}