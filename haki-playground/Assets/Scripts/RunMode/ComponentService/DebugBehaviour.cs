using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public enum DebugOptions
    {
        None = 0,

        DisableInPlayMode = 1,
        DisableinEditor = 2,
        DisableWhenNotSelected = 3,
    }


    public abstract class DebugBehaviour : MonoBehaviour
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
        void OnDrawGizmos()
        {
            DebugRender();
        }

        private void DebugRender()
        {
            bool isSelectedInEditorNotWorkingCorrectly = Selection.Contains(this);
            if (ShouldRenderDebugNotWorkingCorrectly(isSelectedInEditorNotWorkingCorrectly) == false)
                return;

            BeforeDrawGizmos();
            DebugDraw(isSelectedInEditorNotWorkingCorrectly);
            AfterDrawGizmos();
        }


        private bool ShouldRenderDebugNotWorkingCorrectly(bool isSelected)
        {
            if (debugOptions == DebugOptions.DisableInPlayMode && Application.IsPlaying(this))
            {
                Debug.Log(this.GetType().Name + " / " + name + " is playing");
                return false;
            }

            if (debugOptions == DebugOptions.DisableinEditor && Application.isEditor)
                return false;

            if (debugOptions == DebugOptions.DisableWhenNotSelected && isSelected == false)
                return false;

            return true;
        }


        protected virtual void DebugDraw(bool isSelected)
        {
            //this method is empty on purpose. 
            //it could have been made abstract, but that would enforce implementation for every non-abstract inheritor class 
            //in the event when a implementation is not needed, the implementation would look exactly like this one. 
        }
    }
}