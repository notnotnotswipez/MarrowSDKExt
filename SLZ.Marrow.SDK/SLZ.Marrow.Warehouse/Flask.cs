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
    public class Flask : Crate
    {
        public FlaskInfo flaskInfo;

        public override System.Type AssetType
        {
            get => typeof(FlaskInfo);
        }
    }
}
