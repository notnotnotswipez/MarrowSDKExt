using System;
using Unity.Mathematics;
using UnityEngine;

namespace SLZ.Marrow.Interaction
{
	[Serializable]
	public class PolyLineVert
	{
		[SerializeField]
		public float3 position;

		[SerializeField]
		public float3 tangent;

		[SerializeField]
		public float3 normal;

		[SerializeField]
		public float3 forward;

		[SerializeField]
		public float3 backward;

		public bool IsPointInDirection(in float3 point, bool useForward)
		{
			return false;
		}
	}
}
