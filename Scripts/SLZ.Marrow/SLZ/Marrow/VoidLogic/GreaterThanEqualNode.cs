using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/Nodes/VoidLogic Greater Than or Equal")]
	[Support(SupportFlags.Supported, null)]
	[HelpURL("https://github.com/StressLevelZero/MarrowSDK/wiki/VoidLogic/GreaterThanEqualNode")]
	public class GreaterThanEqualNode : BaseNode
	{
		[Tooltip("Amount of approximation allowed in the equality check.\n0 will use Mathf.Approximately/Mathf.Epsilon to approximate.\nMake sure your tolerances match for consistent results!")]
		[SerializeField]
		private float _tolerance;

		private static readonly PortMetadata _portMetadata;

		public override PortMetadata PortMetadata => default(PortMetadata);

		public override void Calculate(ref NodeState nodeState)
		{
		}
	}
}
