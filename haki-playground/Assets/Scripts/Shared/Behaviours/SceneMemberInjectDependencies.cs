using System;
using UnityEngine;

namespace Assets.Scripts.Shared.Behaviours
{
    public abstract class SceneMemberInjectDependencies : MonoBehaviour
    {
        private bool dependencyInjectionStatus = false;

        public bool GetDependencyInjectionStatus()
        {
            return dependencyInjectionStatus;
        }


        public void FinalizeDependancyInjection()
        {
            if (dependencyInjectionStatus)
                throw new Exception("This method should not be called by anything, but ServiceManager and only once");

            dependencyInjectionStatus = true;
        }
    }
}