using System;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CreateAssetMenu(fileName = nameof(ConnectionInfo), menuName = "ScaffoldingComponent/Connection/Connection ScaffoldingComponent", order = 10)]
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
            Debug.Log(Id);
            return Id.Equals(other?.Id);
        }
    }
}