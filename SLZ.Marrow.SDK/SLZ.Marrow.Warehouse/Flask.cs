using Newtonsoft.Json.Linq;
using SLZ.Serialize;
using UnityEngine.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace SLZ.Marrow.Warehouse
{
    public class Flask : CrateT<FlaskLabel>
    {
        [FormerlySerializedAs("_assetReference")]
        [SerializeField]
        private MarrowAsset _mainAsset;

        public override System.Type AssetType
        {
            get => typeof(FlaskLabel);
        }

        public override MarrowAsset MainAsset {
            get => _mainAsset;
            set
            {
                base.MainAsset = value;
                _mainAsset = value;
            }
        }

        public override void Pack(ObjectStore store, JObject json)
        {
            //base.Pack(store, json);
        }
    }
}
