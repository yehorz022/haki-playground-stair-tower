using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public class Quad
    {
        public Vector3 A { get; }
        public Vector3 B { get; }
        public Vector3 C { get; }
        public Vector3 D { get; }

        public Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}