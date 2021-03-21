using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Litefeel.UnityCommon
{

    [RequireComponent(typeof(CanvasRenderer), typeof(RectTransform), typeof(MaskableGraphic))]
    public class IgnoreCanvasClippable : UIBehaviour, IClippable
    {
        private CanvasRenderer canvasRenderer;
        private RectTransform m_RectTransform;
        private Graphic graphic;
        private RectMask2D m_ParentMask;

        public RectTransform rectTransform => m_RectTransform;

        readonly Vector3[] m_Corners = new Vector3[4];
        private Rect rootCanvasRect
        {
            get
            {
                rectTransform.GetWorldCorners(m_Corners);

                if (graphic.canvas)
                {
                    Matrix4x4 mat = graphic.canvas.rootCanvas.transform.worldToLocalMatrix;
                    for (int i = 0; i < 4; ++i)
                        m_Corners[i] = mat.MultiplyPoint(m_Corners[i]);
                }

                return new Rect(m_Corners[0].x, m_Corners[0].y, m_Corners[2].x - m_Corners[0].x, m_Corners[2].y - m_Corners[0].y);
            }
        }

        public void Cull(Rect clipRect, bool validRect)
        {
            var cull = !validRect || !clipRect.Overlaps(rootCanvasRect, true);
            UpdateCull(cull);
        }
        private void UpdateCull(bool cull)
        {
            if (canvasRenderer.cull != cull)
            {
                canvasRenderer.cull = cull;
                UISystemProfilerApi.AddMarker("MaskableGraphic.cullingChanged", this);
                graphic.OnCullingChanged();
            }
        }
        public void RecalculateClipping()
        {
            var newParent = isActiveAndEnabled ? GetRectMaskForClippable(this) : null;

            // if the new parent is different OR is now inactive
            if (m_ParentMask != null && (newParent != m_ParentMask || !newParent.IsActive()))
            {
                m_ParentMask.RemoveClippable(this);
                UpdateCull(false);
            }

            // don't re-add it if the newparent is inactive
            if (newParent != null && newParent.IsActive())
                newParent.AddClippable(this);

            m_ParentMask = newParent;
        }

        public void SetClipRect(Rect clipRect, bool validRect)
        {
            if (validRect)
                canvasRenderer.EnableRectClipping(clipRect);
            else
                canvasRenderer.DisableRectClipping();
        }

        protected override void Awake()
        {
            m_RectTransform = transform as RectTransform;
            canvasRenderer = GetComponent<CanvasRenderer>();
            graphic = GetComponent<Graphic>();
        }

        protected override void OnEnable()
        {
            RecalculateClipping();
        }

        protected override void OnDisable()
        {
            RecalculateClipping();
        }

        protected override void OnTransformParentChanged()
        {
            if (!isActiveAndEnabled)
                return;
            RecalculateClipping();
        }

        protected override void OnCanvasHierarchyChanged()
        {
            if (!isActiveAndEnabled)
                return;
            RecalculateClipping();
        }



        static RectMask2D GetRectMaskForClippable(IClippable clippable)
        {
            List<RectMask2D> rectMaskComponents = ListPool<RectMask2D>.Get();
            RectMask2D componentToReturn = null;

            clippable.rectTransform.GetComponentsInParent(false, rectMaskComponents);

            if (rectMaskComponents.Count > 0)
            {
                for (int rmi = 0; rmi < rectMaskComponents.Count; rmi++)
                {
                    componentToReturn = rectMaskComponents[rmi];
                    if (componentToReturn.gameObject == clippable.gameObject)
                    {
                        componentToReturn = null;
                        continue;
                    }
                    if (!componentToReturn.isActiveAndEnabled)
                    {
                        componentToReturn = null;
                        continue;
                    }
                    break;
                }
            }

            ListPool<RectMask2D>.Release(rectMaskComponents);

            return componentToReturn;
        }

#if UNITY_2019_4_OR_NEWER
#if !UNITY_2019_4_0 && !UNITY_2019_4_1 && !UNITY_2019_4_3 && !UNITY_2019_4_4 && !UNITY_2019_4_5 && !UNITY_2019_4_5 && !UNITY_2019_4_5
#if !UNITY_2019_4_6 && !UNITY_2019_4_7 && !UNITY_2019_4_8 && !UNITY_2019_4_9 && !UNITY_2019_4_10 && !UNITY_2019_4_11
        public void SetClipSoftness(Vector2 clipSoftness)
        {
            throw new System.NotImplementedException();
        }
#endif
#endif
#endif
    }
}