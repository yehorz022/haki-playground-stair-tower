using System;
using UnityEngine;

namespace Assets.Scripts.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ComponentConnectionInfo), menuName = "ScaffoldingComponent/Connection/Connection ScaffoldingComponent", order = 10)]
    public class ComponentConnectionInfo : ScriptableObject
    {

        public enum RotationOrientation
        {
            Vertical,
            Horizontal,

        }

        private Guid id;
        [SerializeField] 
        internal RotationOrientation rotationOrientation;
        [SerializeField]
        public Guid Id
        {
            get
            {
                if (id == Guid.Empty)
                {
                    Id = Guid.NewGuid();
                }

                return id;
            }
            set
            {
                if (id == Guid.Empty)
                    id = value;
            }
        }


        public bool Equals(ComponentConnectionInfo other)
        {
            return Id.Equals(other?.Id);
        }
    }
}