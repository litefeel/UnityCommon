using UnityEngine;

namespace Litefeel.UnityCommon
{
    [RequireComponent(typeof(Canvas))]
    public class SortingOrderByCanvas : MonoBehaviour
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
        private Canvas canvas;

        public void SetDirty()
        {
            if (!isActiveAndEnabled || transform.parent == null) return;
            SortingOrderRegistry.RegisterCanvas(this);
        }

        internal void DoChange()
        {
            if (!isActiveAndEnabled || transform.parent == null) return;
            parentCanvas = GameObjectUtil.GetParentCanvas(transform.parent.gameObject);
            if (parentCanvas == null) return;
            canvas.overrideSorting = true;
            canvas.sortingLayerID = parentCanvas.sortingLayerID;
            canvas.sortingOrder = parentCanvas.sortingOrder + Offset;
        }

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
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
            SortingOrderRegistry.UnregisterCanvas(this);
        }
    }
}
