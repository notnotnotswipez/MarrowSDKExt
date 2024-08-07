using System;
using System.Runtime.CompilerServices;
using SLZ.Algorithms.Unity;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[HelpURL("https://github.com/StressLevelZero/MarrowSDK/wiki/VoidLogic/RotatingJoint")]
	[AddComponentMenu("VoidLogic/Sinks/VoidLogic Rotating Joint")]
	[Support(SupportFlags.BetaSupported, "This works, but uses ConfigurableJoint instead of Marrow primitives.")]
	public sealed class RotatingJoint : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, ISerializationCallbackReceiver, IVoidLogicActuator
	{
		[Interface(typeof(IVoidLogicSource), false)]
		[SerializeField]
		[Tooltip("Previous node in the chain")]
		[Obsolete("Replace with `_previousConnection`")]
		private MonoBehaviour _previousNode;

		private float? _priorValue;

		[SerializeField]
		private ConfigurableJoint _configurableJoint;

		private Rigidbody _rigidBody;

		[Header("Joint Control")]
		[SerializeField]
		private bool _varyTargetRotation;

		[SerializeField]
		private float _minDegreesX;

		[SerializeField]
		private float _maxDegreesX;

		[SerializeField]
		private bool _varyTargetAngVelocity;

		[SerializeField]
		private Vector3 _minAngVelocity;

		[SerializeField]
		private Vector3 _maxAngVelocity;

		[SerializeField]
		private bool _varyAngularDrive;

		[SerializeField]
		private Vector3 _xAngMinSpringDamperForce;

		[SerializeField]
		private Vector3 _xAngMaxSpringDamperForce;

		private static readonly PortMetadata _portMetadata;

		public VoidLogicSubgraph Subgraph
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public int InputCount => 0;

		public PortMetadata PortMetadata => default(PortMetadata);

		private void UnityEngine_002EISerializationCallbackReceiver_002EOnBeforeSerialize()
		{
		}

		private void UnityEngine_002EISerializationCallbackReceiver_002EOnAfterDeserialize()
		{
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void OnDestroy()
		{
		}

		private void Start()
		{
		}

		private void SLZ_002EMarrow_002EVoidLogic_002EIVoidLogicActuator_002EActuate(ref NodeState nodeState)
		{
		}

		private void SETJOINT(float voltage = 1f)
		{
		}

        public bool TryGetInputAtIndex(uint idx, out IVoidLogicSource input)
        {
            input = null;
			return true;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            
        }

        public void Actuate(ref NodeState nodeState)
        {
            
        }
    }
}
