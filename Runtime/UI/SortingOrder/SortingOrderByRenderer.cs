using UnityEngine;

namespace Litefeel.UnityCommon
{
    public class SortingOrderByRenderer : MonoBehaviour
    {
        [SerializeField]
        [Range(-10, SortingOrderRegistry.ORDER_STEP - 10)]
        private int m_Offset = 1;
        public int Offset
        {
            get { return m_Offset; }
            set { if (m_Offset != value) { m_Offset = value; SetDirty(); } }
        }
        private Canvas parentCanvas;


        public void SetDirty()
        {
            if (!isActiveAndEnabled || transform.parent == null) return;
            SortingOrderRegistry.RegisterRender(this);
        }

        internal void DoChange()
        {
            parentCanvas = GameObjectUtil.GetParentCanvas(gameObject);
            if (parentCanvas == null) return;
            using (var scope = ListPoolScope<Renderer>.Create())
            {
                GetComponentsInChildren(true, scope.list);
                foreach (var renderer in scope.list)
                {
                    renderer.sortingLayerID = parentCanvas.sortingLayerID;
                    renderer.sortingOrder = parentCanvas.sortingOrder + Offset;
                }
            }
        }

        private void OnCanvasHierarchyChanged()
        {
            SetDirty();
        }
        private void OnTransformParentChanged()
        {
            SetDirty();
        }

        private void OnEnable()
        {
            SetDirty();
        }

        private void OnDisable()
        {
            SortingOrderRegistry.UnregisterRender(this);
        }
    }
}