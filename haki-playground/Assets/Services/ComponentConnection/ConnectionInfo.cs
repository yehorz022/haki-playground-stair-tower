

using System;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CreateAssetMenu(fileName = nameof(ConnectionInfo), menuName = "Component/Connection/Connection Type", order = 10)]
    public class ConnectionInfo : ScriptableObject
    {

        public enum RotationOrientation
        {
            Vertical,
            Horizontal,

        }

        private Guid id;
        [SerializeField] internal RotationOrientation rotationOrientation;
        [SerializeField]
        internal Guid Id
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


        public bool Equals(ConnectionInfo other)
        {
            return Id.Equals(other?.Id);
        }
    }
}