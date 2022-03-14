using System;
using UnityEngine;

namespace Assets.Scripts.Shared.Behaviours
{
    [Flags]
    public enum DebugOptions
    {
        None = 0,

        AllowInPlayMode = 1 << 0,
        AllowInEditorMode = 1 << 1,
        RenderWhenSelected = 1 << 2,
        RenderWhenNotSelected = 1 << 3,
    }

    public abstract class DebugBehaviour : SceneMemberInjectDependencies
    {
        [SerializeField] private DebugOptions debugOptions;
        private Color gizmoColor;
        private void BeforeDrawGizmos()
        {
            gizmoColor = Gizmos.color;
        }
       
        private void AfterDrawGizmos()
        {
            Gizmos.color = gizmoColor;
        }

        [ExecuteInEditMode]
        void OnDrawGizmosSelected()
        {
            if (debugOptions.HasFlag(DebugOptions.RenderWhenSelected))
            {
                DebugRender(true);
            }
        }

        [ExecuteInEditMode]
        void OnDrawGizmos()
        {
            DebugRender(false);
        }

        private void DebugRender(bool isSelected)
        {
#if UNITY_EDITOR
            if (debugOptions.HasFlag(DebugOptions.AllowInEditorMode) == false)
            {
                return;
            }
#endif



            BeforeDrawGizmos();
            DebugDraw(isSelected);
            AfterDrawGizmos();
        }


        protected virtual void DebugDraw(bool isSelected)
        {

        }
    }
}