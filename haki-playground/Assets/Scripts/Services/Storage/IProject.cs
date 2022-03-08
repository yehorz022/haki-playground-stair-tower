using Assets.Scripts.Shared.Behaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Storage
{
    public interface IProject
    {
        void Save();
        void Load(Transform parent, List<HakiComponent> components);
    }

}