﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.PuppetMasta;
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Zones;
using UltEvents;
using UnityEngine;
using UnityEngine.Serialization;

namespace SLZ.Marrow.AI
{
	[Serializable]
	public class SpawnGroup : ISpawnListenable
	{
		public int DeadCount
		{
			get
			{
				return 0;
			}
		}

		public int SpawnCount
		{
			get
			{
				return 0;
			}
		}

		public void GetAISettings()
		{
		}

		public void GetCrateRandomizers()
		{
		}

		public void KillAll()
		{
		}

		public void MentalAllGroup(TriggerRefProxy proxy)
		{
		}

		public void Setup()
		{
		}

		public void ResetVariables()
		{
		}

		public void Cleanup()
		{
		}

		private void HandleBrainRegistration(AIBrain brain, bool val)
		{
		}

		public void OnSpawn(GameObject go)
		{
		}

		private void PostReactivate(BehaviourBaseNav baseNav)
		{
		}

		public void OnDespawn(GameObject gameObject)
		{
		}

		private void OnResurrect(AIBrain aiBrain)
		{
		}

		private void OnDeath(AIBrain aiBrain)
		{
		}

		private void AddEncounter(AIBrain aiBrain)
		{
		}

		private void RemoveEncounter(AIBrain aiBrain)
		{
		}

		public bool IsAllDead()
		{
			return default(bool);
		}

		public bool IsUnderTotalSpawned()
		{
			return default(bool);
		}

		public bool IsUnderMaxAlive()
		{
			return default(bool);
		}

		public bool IsOverMaxDead()
		{
			return default(bool);
		}

		public bool IsOverMaxAlive()
		{
			return default(bool);
		}

		public UniTask SpawnAsync()
		{
			return default(UniTask);
		}

		public UniTask SpawnProfileAsync()
		{
			return default(UniTask);
		}

		public void KillAlive()
		{
		}

		public void Despawn()
		{
		}

		public UniTask DespawnBrain(AIBrain brain)
		{
			return default(UniTask);
		}

		public UniTask WaitAndDespawnAllDead()
		{
			return default(UniTask);
		}

		public UniTask DespawnAllBrains(bool useGroupDelay)
		{
			return default(UniTask);
		}

		public void CompleteGroup()
		{
		}

		public UniTask DespawnToMaxDeadCount()
		{
			return default(UniTask);
		}

		public SpawnGroup()
		{
		}

		[HideInInspector]
		public EncounterProfile encounterProfile;

		[FormerlySerializedAs("concurrent")]
		[Tooltip("Maximum alive AI active at a time for this group before spawn")]
		public uint maxAlive;

		[Tooltip("Maximum dead AI active at a time for this group before despawn")]
		public uint maxDead;

		[Tooltip("Total AI that will be spawned for this group, locked by encounter profile")]
		[FormerlySerializedAs("maxSpawn")]
		public uint totalSpawn;

		[Tooltip("Initial delay for the group.")]
		public float initialGroupDelay;

		[Tooltip("Time between each spawn")]
		public float spawnInterval;

		[Tooltip("Delay in seconds after death before despawn")]
		public float despawnInterval;

		[Tooltip("If parallel spawn order, ensure spawners are unique to each spawn group")]
		public CrateSpawner[] spawners;

		private SpawnAISettings[] aiSettings;

		[HideInInspector]
		public List<SpawnerToggle> spawnerToggles;

		public bool hostileGroup;

		private int _deadCount;

		private int _spawnCount;

		private HashSet<AIBrain> _aliveBrains;

		private HashSet<AIBrain> _deadBrains;

		private HashSet<AIBrain> _spawnedBrains;

		private HashSet<AIBrain> _ignoredBrains;

		private int _spawnerIndex;

		private Action<GameObject> _onCrateDespawned;

		private Action<AIBrain> _onDeath;

		private Action<AIBrain> _onResurrect;

		public Action<GameObject, SpawnGroup> OnGroupSpawned;

		public Action<SpawnGroup> OnGroupComplete;

		public Action<SpawnGroup> OnCleanup;

		[HideInInspector]
		public bool isComplete;

		[Tooltip("None:Mental state will not change when spawned, Agro: agro on player proxy, Investigate: increase bubble and move to player position")]
		public SpawnGroup.MentalMode mentalMode;

		[Tooltip("Duration in seconds npc spends investigating player position")]
		public float investigateTimeout;

		public bool useSpawnerToggle;

		public bool useSpawnEffect;

		private Dictionary<CrateSpawner, RandomizeCrate> _crateRandLookup;

		private TriggerRefProxy playerProxy;

		public UltEvent onComplete;

		public UltEvent<AIBrain> OnDeathEvent;

		private bool isSetup;

		public enum MentalMode
		{
			NONE,
			INVESTIGATE,
			AGRO
		}
	}
}
