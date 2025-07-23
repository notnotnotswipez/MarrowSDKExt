 
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    public class ButtonController : CircuitSocket
    {
        [SerializeField]
        private ButtonMode _buttonMode = ButtonMode.ClickUp;

        private const float downThreshold = 0.9f;

        private const float upThreshold = 0.1f;

        private bool _charged;
        
        private bool _toggleCharged;

        [Tooltip("Clip(s) to be played on button press")]
        [SerializeField]
        protected AudioClip[] _pressClips;

        [Tooltip("Clip(s) to be played on button unpress")]
        [SerializeField]
        protected AudioClip[] _depressClips;

        public enum ButtonMode
        {
            ClickUp,
            ClickDown,
            Toggle,
            ContinuousDown,
            ContinuousFloat
        }
    }
}