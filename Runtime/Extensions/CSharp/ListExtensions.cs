using System.Collections.Generic;
using System.Reflection;

namespace Litefeel.UnityCommon
{
    public static class ListExtensions
    {
        public static T[] InnerArray<T>(this List<T> list)
        {
            return InnerList<T>.itemField.GetValue(list) as T[];
        }


        static class InnerList<T>
        {
            public static FieldInfo itemField = typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}

