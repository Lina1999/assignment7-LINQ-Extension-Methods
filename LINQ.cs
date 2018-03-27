using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace LINQLib
{
    /// <summary>
    /// class Linq implementing some LINQ extension methods
    /// </summary>
    public static class LINQ
    {
        /// <summary>
        /// ExtensionSelect method which changes the elements of source depending on selector function.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> ExtensionSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            foreach (var item in source)
                yield return selector(item);
        }


        /// <summary>
        /// ExtensionWhere method selects only some elements from source based on predicate.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ExtensionWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var elem in source)
                if(predicate(elem))
                    yield return elem;
        }
        /// <summary>
        /// Class Grouping which implements IGrouping interace.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        private class Grouping<TKey, TSource> : IGrouping<TKey, TSource>
        {
            /// <summary>
            /// Key.
            /// </summary>
            public TKey key;
            /// <summary>
            /// Source.
            /// </summary>
            public IEnumerable<TSource> source;

            /// <summary>
            /// Parameterless constructers.
            /// </summary>
            public Grouping() { }

            /// <summary>
            /// Constructor with parameter.
            /// </summary>
            /// <param name="source"></param>
            public Grouping(IEnumerable<TSource> source)
            {
                this.source = source;
            }

            /// <summary>
            /// Getter.
            /// </summary>
            public TKey Key => key;

            /// <summary>
            /// GetEnumerator method.
            /// </summary>
            /// <returns></returns>
            public IEnumerator<TSource> GetEnumerator()
            {
                return source.GetEnumerator();
            }

            /// <summary>
            /// IEnumerable.GetEnumerator method.
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return source.GetEnumerator();
            }
        }

        /// <summary>
        /// Grouping elements of source based on keySelector function.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<TKey, TSource>> ExtensionGroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var group = new Grouping<TKey, TSource>();
            var hashSet = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (hashSet.Add(keySelector(item)))
                {
                    var groupedItems = source.ExtensionWhere(i => keySelector(i).Equals(keySelector(item)));
                    group.source = groupedItems;
                    group.key = keySelector(item);
                    yield return group;
                }
            }
        }
        
        /// <summary>
        /// Making list from given source.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TSource> ExtensionToList<TSource>(this IEnumerable<TSource> source)
        {
            List<TSource> ans=new List<TSource>();
            foreach (var elem in source)
                ans.Add(elem);
            return ans;
        }

        /// <summary>
        /// class OrderedEnumerable implementing IOrderedEnumerable interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class OrderedEnumerable<T> : IOrderedEnumerable<T>
        {
            /// <summary>
            /// Source.
            /// </summary>
            private readonly List<T> source;

            /// <summary>
            /// Parameterless constructor
            /// </summary>
            public OrderedEnumerable()
            {
            }

            /// <summary>
            /// constructor with parameter
            /// </summary>
            /// <param name="source"></param>
            public OrderedEnumerable(List<T> source)
            {
                this.source = source;
            }


            /// <summary>
            /// ordering enumerable
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <param name="keySelector"></param>
            /// <param name="comparer"></param>
            /// <param name="descending"></param>
            /// <returns></returns>
            public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
            {
                source.Sort();
                return new OrderedEnumerable<T>(source);
            }

            /// <summary>
            /// GetEnumerator method.
            /// </summary>
            /// <returns></returns>
            public IEnumerator<T> GetEnumerator()
            {
                return source.GetEnumerator();
            }

            /// <summary>
            /// IEnumerable.GetEnumerator method.
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return source.GetEnumerator();
            }
        }

        /// <summary>
        /// sorting source with key selector by just using bubble sort. 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ExtensionOrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)where TKey:IComparable<TKey>
        {
            var arr = source.ExtensionToList();
            bool isSorted = true;
            while (isSorted)
            {
                isSorted = false;
                for (int i = 0; i < arr.Count - 1; ++i)
                {
                    if ((descending && (Compare(arr[i], arr[i + 1], keySelector) < 0))
                        || (!descending && (Compare(arr[i], arr[i + 1], keySelector) > 0)))
                    {
                        
                            var temp = arr[i];
                            arr[i] = arr[i + 1];
                            arr[i + 1] = temp;
                            isSorted = true;
                        
                    }
                }
            }
            return new OrderedEnumerable<TSource>(arr);
        }

        /// <summary>
        /// Comparing x and y based on keySelector.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        private static int Compare<TSource, TKey>(TSource x, TSource y, Func<TSource, TKey> keySelector)
        {
            TKey key1 = keySelector(x);
            TKey key2 = keySelector(y);
            IComparer<TKey> comparer = Comparer<TKey>.Default;
            return comparer.Compare(key1, key2);
        }
        
        /// <summary>
        /// Making dictionary from source based on keySelector.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TSource> ExtensionToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var dict = new Dictionary<TKey, TSource>();
            foreach (var elem in source)
                dict.Add(keySelector(elem), elem);
            return dict;
        }
    }

}
