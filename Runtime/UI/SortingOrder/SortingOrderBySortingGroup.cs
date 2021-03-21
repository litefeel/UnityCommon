using UnityEngine;
using UnityEngine.Rendering;

namespace Litefeel.UnityCommon
{
    [RequireComponent(typeof(SortingGroup))]
    public class SortingOrderBySortingGroup : MonoBehaviour
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
        private SortingGroup group;

        public void SetDirty()
        {
            if (!isActiveAndEnabled || transform.parent == null) return;
            SortingOrderRegistry.RegisterSortingGroup(this);
        }

        internal void DoChange()
        {
            if (!isActiveAndEnabled || transform.parent == null) return;
            parentCanvas = GameObjectUtil.GetParentCanvas(transform.parent.gameObject);
            if (parentCanvas == null) return;
            group.sortingLayerID = parentCanvas.sortingLayerID;
            group.sortingOrder = parentCanvas.sortingOrder + Offset;
        }

        private void Awake()
        {
            group = GetComponent<SortingGroup>();
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
            SortingOrderRegistry.UnregisterSortingGroup(this);
        }
    }
}