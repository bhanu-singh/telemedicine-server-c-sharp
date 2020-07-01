using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataServices
{
    public enum ConsultationStatus
    {
        Accepted, Rejected, Completed
    }
    public enum SupplierType
    {
        Hospital,
        Doctor
    }
    public enum ConsultationMode
    {
        Phone,
        Video,
        Text
    }
    public static class DistinctDayExtension
    {
        public static ICollection<T> AddTo<T>(this IEnumerable<T> list,
                                      ICollection<T> collection)
        {
            foreach (T item in list)
            {
                collection.Add(item);
            }
            return collection;
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
    public static class NullToDbNul
    {
        public static object DbNullIfNull(this object obj)
        {
            return obj != null ? obj : DBNull.Value;
        }

    }

    public enum ActionType
    {
        NoAction = 0,
        Add = 1,
        Delete = 2
    }


    public enum YesNo
    {
        Yes = 1,
        No = 0
    }

    public enum StoreProcStatusEnum
    {
        EmailAlreadyExists = -2,
        EntryAlreadyExists = -1,
        InUse = -3,
        Success = 0,

    }
}
