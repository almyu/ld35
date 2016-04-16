using UnityEngine;
using System.Collections.Generic;

namespace LD35 {

    public static class ListUtility {

        public static bool SwapRemove<T>(this List<T> list, T item) {
            var index = list.IndexOf(item);
            if (index == -1) return false;

            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return true;
        }
    }
}
