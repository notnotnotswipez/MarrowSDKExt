using UnityEngine;
using SLZ.Marrow;

namespace SLZ.Marrow.Circuits
{
    public class CircuitSocket : Circuit
    {
        [SerializeField]
        private Circuit _input;

        [SerializeField]
        protected Servo _servo;

        [SerializeField]
        protected Rigidbody _rb;

        public Circuit input
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        
        protected virtual void Reset()
        {
            TryGetComponent(out _servo);
            TryGetComponent(out _rb);
        }
    }
}