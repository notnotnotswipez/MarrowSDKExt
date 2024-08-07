using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace SLZ.Marrow.SceneStreaming
{
	[HelpURL("https://github.com/StressLevelZero/MarrowSDK/wiki/Levels")]
	[RequireComponent(typeof(BoxCollider))]
	[Obsolete("Chunk trigger is now obsolete use the new zone/entity system for this functionality")]
	public class ChunkTrigger : MonoBehaviour
	{
		public Chunk chunk;

		public BoxCollider trigger;

		public bool ignorePlayer;

		public UnityEvent OnChunkLoaded;

		public UnityEvent OnChunkUnloaded;

		[SerializeField]
		private LayerMask _layers;

		[HideInInspector]
		public bool isActive;

		private List<ChunkTracker> _trackers;

		private HashSet<ChunkTracker> _trackersSet;

		public StreamStatus Status
		{
			[CompilerGenerated]
			get
			{
				return default(StreamStatus);
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		private void Reset()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnTriggerEnter(Collider other)
		{
		}

		private void OnTriggerExit(Collider other)
		{
		}

		public void RemoveTracker(ChunkTracker tracker)
		{
		}

		public bool WarmupHasPlayer()
		{
			return false;
		}

		private void HandleTriggerEnter(Collider other)
		{
		}

		public void OnLoad()
		{
		}

		public void OnUnload()
		{
		}
	}
}
