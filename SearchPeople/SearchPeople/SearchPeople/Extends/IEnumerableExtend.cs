using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SearchPeople.Extends
{
    public static class IEnumerableExtend
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
        {
            ObservableCollection<T> itemsCollection = new ObservableCollection<T>();
            
            foreach (var item in items)
            {
                itemsCollection.Add(item);
            }

            return itemsCollection;
        }

    }
}
