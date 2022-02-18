

using System;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CreateAssetMenu(fileName = nameof(ConnectionType), menuName = "Component/Connection/Connection Type", order = 10)]
    public class ConnectionType : ScriptableObject
    {
        private Guid id = Guid.NewGuid();

        [SerializeField]
        internal Guid Id
        {
            get => id;
            set => id = value;
        }


        public bool Equals(ConnectionType other)
        {
            //Debug.Log(Id);
            return Id.Equals(other?.Id);
        }
    }
}