using System.Collections.Generic;
using UnityEngine;

namespace Litefeel.UnityCommon
{
    public class SortingOrderRegistry
    {
        public const int ORDER_STEP = 200;

        private static List<SortingOrderByRenderer> s_Renderers = new List<SortingOrderByRenderer>();
        private static List<SortingOrderBySortingGroup> s_Groups = new List<SortingOrderBySortingGroup>();
        private static List<SortingOrderByCanvas> s_Canvass = new List<SortingOrderByCanvas>();
        private static List<SortingOrderByCanvas> s_TempCanvass = new List<SortingOrderByCanvas>();
        private static bool s_Sorting = false;


        public static void RegisterCanvas(SortingOrderByCanvas canvas)
        {
            var list = s_Sorting ? s_TempCanvass : s_Canvass;
            if (!list.Contains(canvas))
                list.Add(canvas);
        }
        public static void UnregisterCanvas(SortingOrderByCanvas canvas)
        {
            RemoveItem(s_TempCanvass, canvas);
            if (!s_Sorting)
                RemoveItem(s_Canvass, canvas);
        }

        public static void RegisterRender(SortingOrderByRenderer renderer)
        {
            if (!s_Renderers.Contains(renderer))
                s_Renderers.Add(renderer);
        }

        public static void UnregisterRender(SortingOrderByRenderer renderer)
        {
            RemoveItem(s_Renderers, renderer);
        }
        public static void RegisterSortingGroup(SortingOrderBySortingGroup group)
        {
            if (!s_Groups.Contains(group))
                s_Groups.Add(group);
        }

        public static void UnregisterSortingGroup(SortingOrderBySortingGroup group)
        {
            RemoveItem(s_Groups, group);
        }
        public static void Update()
        {
            SortCanvass(s_Canvass);
            if (s_TempCanvass.Count > 0)
            {
                var temp = s_Canvass;
                s_Canvass = s_TempCanvass;
                s_TempCanvass = temp;
                SortCanvass(s_Canvass);
            }
            if (s_Renderers.Count > 0)
            {
                s_Renderers.Sort(RendererSorter);
                foreach (var renderer in s_Renderers)
                    renderer.DoChange();
                s_Renderers.Clear();
            }
            if (s_Groups.Count > 0)
            {
                foreach (var group in s_Groups)
                    group.DoChange();
                s_Groups.Clear();
            }
        }

        private static void SortCanvass(List<SortingOrderByCanvas> list)
        {
            RemoveUnactives(list);
            if (list.Count > 0)
            {
                list.Sort(CanvasSorter);
                s_Sorting = true;
                foreach (var canvas in list)
                    canvas.DoChange();
                list.Clear();
                s_Sorting = false;
            }
        }

        private static void RemoveUnactives<T>(List<T> list) where T : Behaviour
        {
            if (list.Count == 0) return;
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                if (list[i] == null || !list[i].isActiveAndEnabled)
                    list[i] = list[--count];
            }
            if (count > 0)
                list.RemoveRange(count, list.Count - count);
        }

        private static void RemoveItem<T>(List<T> list, T item)
        {
            var idx = list.IndexOf(item);
            if (idx >= 0)
            {
                list[idx] = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
            }
        }

        private static int CanvasSorter(SortingOrderByCanvas x, SortingOrderByCanvas y)
        {
            return GetDepth(x.transform) - GetDepth(y.transform);
        }
        private static int RendererSorter(SortingOrderByRenderer x, SortingOrderByRenderer y)
        {
            return GetDepth(x.transform) - GetDepth(y.transform);
        }

        private static int GetDepth(Transform transform)
        {
            int depth = 0;
            while (transform.parent != null)
            {
                depth++;
                transform = transform.parent;
            }
            return depth;
        }

        internal static void ForceUpdateByRootCanvas(GameObject root)
        {
            using (var scope = ListPoolScope<SortingOrderByCanvas>.Create())
            {
                root.GetComponentsInChildren(false, scope.list);
                foreach (var item in scope.list)
                    item.DoChange();
            }

            using (var scope = ListPoolScope<SortingOrderByRenderer>.Create())
            {
                root.GetComponentsInChildren(false, scope.list);
                scope.list.Sort(RendererSorter);
                foreach (var item in scope.list)
                    item.DoChange();
            }

            using (var scope = ListPoolScope<SortingOrderBySortingGroup>.Create())
            {
                root.GetComponentsInChildren(false, scope.list);
                foreach (var item in scope.list)
                    item.DoChange();
            }
        }
    }
}