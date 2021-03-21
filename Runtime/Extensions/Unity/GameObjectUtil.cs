using System;
using UnityEngine;

namespace Litefeel.UnityCommon
{
    public static class GameObjectUtil
    {
        public static Canvas GetParentCanvas(GameObject gameObject)
        {
            using (var scope = ListPoolScope<Canvas>.Create())
            {
                gameObject.GetComponentsInParent(false, scope.list);
                foreach (var canvas in scope.list)
                {
                    if (canvas.isActiveAndEnabled)
                        return canvas;
                }
            }
            return null;
        }

        public static float CalcCanvasGroupAlpha(GameObject go)
        {
            if (go == null) return 1;
            var alpha = 1f;
            using (var scope = ListPoolScope<CanvasGroup>.Create())
            {
                go.GetComponentsInParent(false, scope.list);
                for (var i = 0; i < scope.list.Count; i++)
                {
                    alpha *= scope.list[i].alpha;
                    if (scope.list[i].ignoreParentGroups)
                        break;
                }
            }
            return alpha;
        }
    }
}
