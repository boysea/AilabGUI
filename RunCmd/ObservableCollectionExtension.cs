using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace RunCmd
{
    public static class ObservableCollectionExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection,List<T> ts)
        {
            ts.ForEach(x => observableCollection.Add(x));
        }

        public static void RemoveAll<T>(this ObservableCollection<T> observableCollection, Expression<Func<T,bool>> expression)
        {
            var method = expression.Compile();
            var indexs = new List<int>();
            for(var i = 0; i < observableCollection.Count; i++)
            {
                if (method.Invoke(observableCollection[i]))
                {
                    indexs.Add(i);
                }
            }
            for(var i = indexs.Count - 1; i > -1; i--)
            {
                observableCollection.RemoveAt(indexs[i]);
            }
        }
    }
}