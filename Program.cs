using System;
using LINQLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestingLinqExtension
{
    /// <summary>
    /// class Program
    /// </summary>
    class Program
    {
        /// <summary>
        /// main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            List<int> a = new List<int>();
            for (int i = 0; i < 10; ++i)
                a.Add(i);
            //testing extension select 
            IEnumerable<int> b = a.ExtensionSelect(x => x * x);
            foreach (var i in b)
                Console.WriteLine(i + " ");

            //testing extension select 
            b = a.ExtensionWhere(x => x % 2 == 0);
            foreach (var i in b)
                Console.WriteLine(i + " ");

            var dict = a.ExtensionToDictionary(x => x + "abc");
            foreach (var i in dict)
                Console.WriteLine(i + " ");

            //testing extension to List 
            var list = a.ExtensionToList();
            foreach (var i in list)
                Console.WriteLine(i + " ");

            List<string> str = new List<string>(6);
            str.Add("testing");
            str.Add("extension");
            str.Add("methods");
            str.Add("for");
            str.Add("sorting");
            str.Add("and");
            str.Add("grouping");

            //testing extension order by
            var ordered = str.ExtensionOrderBy(s => s.Length, false);
            foreach (var i in ordered)
                Console.WriteLine(i + " ");

            //testing extension group by 
            var group = str.ExtensionGroupBy(x => x.Length);
            foreach (var lengthGroup in group)
            {
                Console.WriteLine(lengthGroup.Key);
                foreach (var name in lengthGroup)
                    Console.WriteLine(name);
            }
        }
    }
}


