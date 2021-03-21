using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Litefeel.UnityCommon
{
    [RequireComponent(typeof(Graphic), typeof(RectTransform))]
    public class CanvasRendererRebinder : MonoBehaviour
    {
        [SerializeField]
        private Transform m_TargetParent;
        private RectTransform m_TargetTrans;
        private CanvasRenderer m_TargetRenderer;
        private RectTransform m_Trans;

        private static FieldInfo m_CanvasRendererField;

        private void Awake()
        {
            m_Trans = transform as RectTransform;
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer));
            m_TargetRenderer = go.GetComponent<CanvasRenderer>();
            m_TargetTrans = go.transform as RectTransform;
            m_TargetTrans.SetParent(m_TargetParent, false);
            m_TargetTrans.sizeDelta = m_Trans.rect.size;
            m_TargetTrans.pivot = m_Trans.pivot;
            m_TargetTrans.position = m_Trans.position;

            if (m_CanvasRendererField == null)
                m_CanvasRendererField = typeof(Graphic).GetField("m_CanvasRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
            var graphic = GetComponent<Graphic>();
            Debug.Assert(graphic != null);
            Debug.Assert(m_TargetRenderer != null);
            Debug.Assert(m_CanvasRendererField != null);
            m_CanvasRendererField.SetValue(graphic, m_TargetRenderer);
        }

        private void OnCanvasGroupChanged()
        {
            if (m_TargetRenderer != null)
                m_TargetRenderer.SetAlpha(GameObjectUtil.CalcCanvasGroupAlpha(gameObject));
        }

        private void Update()
        {
            if (m_Trans.hasChanged)
            {
                m_Trans.hasChanged = false;
                m_TargetTrans.position = m_Trans.position;
            }
        }
    }
}