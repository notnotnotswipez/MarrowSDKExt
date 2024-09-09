using System;
using System.Collections.Generic;
using SLZ.Marrow.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLZ.Marrow.Interaction
{
	public class Tracker : MonoBehaviour
	{
		private static ComponentCache<Tracker> _cache;

		[SerializeField]
		private MarrowEntity _entity;

		[SerializeField]
		private MarrowBody _body;

		[SerializeField]
		private Collider _collider;

		private List<Action<Collider>> _onDisableActions;

		public static ComponentCache<Tracker> Cache => _cache;

		public bool HasBody
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public MarrowEntity Entity => _entity;

		public MarrowBody Body => _body;

		public Collider Collider => _collider;

		private bool CanUpdateValues(Vector3 orignal, Vector3 updated)
		{
			return false;
		}

		private bool CanUpdateValues(float original, float updated)
		{
			return false;
		}

		internal void Validate(TrackerSettings settings, MarrowBody body, MarrowEntity entity)
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		internal void OnDeactivate()
		{
		}

		public void AddDisableCallback(Action<Collider> callback)
		{
		}

		public void RemoveDisableCallback(Action<Collider> callback)
		{
		}

		public void ValidateBounds()
		{
#if UNITY_EDITOR
			Bounds bounds = _body.GetBounds();
			BoxCollider boxCollider = _collider as BoxCollider;
			if (boxCollider != null)
			{
   				Vector3 bodyScale = _body.transform.localScale;
   				Vector3 scalar = new Vector3(1/bodyScale.x, 1/bodyScale.y, 1/bodyScale.z);
       				
				boxCollider.center = Vector3.Scale(bounds.center, scalar);
				boxCollider.size = Vector3.Scale(bounds.size, scalar);
			}
			if (boxCollider.size == Vector3.zero)
			{
				boxCollider.size = new Vector3(0.05f,0.05f,0.05f);
			}
#endif
		}

		public void ValidateLayer()
		{
			if (gameObject.layer < 26)
			{
				gameObject.layer = 26;
			}
		}

		public void ValidateComponent()
		{

			//box collider self check
			if(_collider == null)
			{
				_collider = gameObject.AddComponent<BoxCollider>();
			}

			//check if is contained in marrowbody
			_body = gameObject.GetComponentInParent<MarrowBody>(true);
			if(_body != null)
			{
				gameObject.name = $"Tracker[{_body.gameObject.name}]";
				ValidateBounds();
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(this);
				UnityEngine.Object.DestroyImmediate(this.gameObject);
				return;
			}

			//MarrowEntity Check
			_entity = gameObject.GetComponentInParent<MarrowEntity>(true);
			

			//Layer Check
			ValidateLayer();
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(Tracker))]
	[DisallowMultipleComponent]
	public class TrackerEditor : Editor 
	{
	    public override void OnInspectorGUI()
	    {
			Tracker behaviour = (Tracker)target;

			if (!PrefabUtility.IsPartOfPrefabAsset(behaviour.gameObject))
			{
    		    if(GUILayout.Button("Validate"))
        		{
					behaviour.ValidateComponent();
        		}
			}
	
        	DrawDefaultInspector();
	    }
	}
#endif
}
