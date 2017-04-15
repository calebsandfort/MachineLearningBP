using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearningBP.Web.Framework
{
    public static class Extensions
    {
        public static List<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static List<ListItem> EnumToListItems<T>()
        {
            List<ListItem> list = new List<ListItem>();
            foreach(T item in Enum.GetValues(typeof(T)).Cast<T>())
            {
                if(item.ToString() != "None")
                    list.Add(new ListItem { Display = item.ToString(), Value = ((int)(object)item).ToString() });
            }

            return list;
        }
    }
}