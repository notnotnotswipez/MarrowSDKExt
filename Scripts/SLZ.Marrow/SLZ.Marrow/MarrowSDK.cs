using System;
 
using System.Collections.Generic;
 
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
 
using UnityEngine;
 
 
 
 
#if UNITY_EDITOR
  
using UnityEditor;

#endif
namespace SLZ.Marrow
{
    public static class MarrowSDK
    {
        public const string SDK_NAME = "Marrow SDK";
        public const string SDK_VERSION = "1.2.0";
        public const string MIN_COMPATIBLE_VERSION = "0.6.0";
        public const string DEV_NAME = "Stress Level Zero";
        public const string PACKAGE_NAME = "com.stresslevelzero.marrow.sdk.extended";
        public static readonly string PROJ4_NAME = "BONELAB";
        public static readonly string[] GAME_NAMES =
        {
            PROJ4_NAME
        };
        public const string RUNTIME_MODS_DIRECTORY_NAME = "Mods";
        public const string INSTALL_STAGING_DIRECTORY_NAME = "ModsStaging";
        public static readonly int MAX_PATH = 260;
#if UNITY_EDITOR
        public const string BUILT_PALLETS_NAME = "BuiltPallets";
        public const string EDITOR_ASSETS_FOLDER = "_MarrowAssets";
#endif
        private static Version _sdkVersion;
        public static Version SDKVersion
        {
            get
            {
                if (_sdkVersion == null)
                {
                    _sdkVersion = new Version(SDK_VERSION);
                }

                return _sdkVersion;
            }
        }

        private static Version _minCompatibleVersion;
        public static Version MinCompatibleVersion
        {
            get
            {
                if (_minCompatibleVersion == null)
                {
                    _minCompatibleVersion = new Version(MIN_COMPATIBLE_VERSION);
                }

                return _minCompatibleVersion;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void PrecachePaths()
        {
            _runtimeModsPath = Path.Combine(Application.persistentDataPath, RUNTIME_MODS_DIRECTORY_NAME);
            _installStagingPath = Path.Combine(Application.persistentDataPath, INSTALL_STAGING_DIRECTORY_NAME);
            _modsCachePath = Path.Combine(Application.persistentDataPath, "com.unity.addressables");
            _modSettingsPath = Path.Combine(Application.persistentDataPath, "settings_mods.json");
        }

        private static string _runtimeModsPath;
        public static string RuntimeModsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_runtimeModsPath))
                {
                    _runtimeModsPath = Path.Combine(Application.persistentDataPath, RUNTIME_MODS_DIRECTORY_NAME);
                }

                if (!Directory.Exists(_runtimeModsPath))
                {
                    Directory.CreateDirectory(_runtimeModsPath);
                }

                return _runtimeModsPath;
            }
        }

        private static string _installStagingPath;
        public static string InstallStagingPath
        {
            get
            {
                if (string.IsNullOrEmpty(_installStagingPath))
                {
                    _installStagingPath = Path.Combine(Application.persistentDataPath, INSTALL_STAGING_DIRECTORY_NAME);
                }

                if (!Directory.Exists(_installStagingPath))
                {
                    Directory.CreateDirectory(_installStagingPath);
                }

                return _installStagingPath;
            }
        }

        private static string _modsCachePath;
        public static string ModsCachePath
        {
            get
            {
                if (string.IsNullOrEmpty(_modsCachePath))
                {
                    _modsCachePath = Path.Combine(Application.persistentDataPath, "com.unity.addressables");
                }

                return _modsCachePath;
            }
        }

        private static string _modSettingsPath;
        public static string ModSettingsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_modSettingsPath))
                {
                    _modSettingsPath = Path.Combine(Application.persistentDataPath, "settings_mods.json");
                }

                return _modSettingsPath;
            }
        }

#if UNITY_EDITOR
        private static string _packagePath;
        public static string PackagePath
        {
            get
            {
                if (string.IsNullOrEmpty(_packagePath))
                {
                    _packagePath = Path.Combine("Packages", PACKAGE_NAME);
                }

                return _packagePath;
            }
        }

        private static string _libraryPath;
        public static string LibraryPath
        {
            get
            {
                if (string.IsNullOrEmpty(_libraryPath))
                {
                    _libraryPath = System.IO.Path.Combine("Library", SanitizeName(SDK_NAME).Replace(" ", ""));
                }

                return _libraryPath;
            }
        }

        private static Material _voidMaterial;
        public static Material VoidMaterial
        {
            get
            {
                if (_voidMaterial == null)
                {
                    _voidMaterial = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath("5f5d50ce8dbb5b84fb70780a2ccf11c8"));
                }

                return _voidMaterial;
            }
        }

        private static Material _voidMaterialAlt;
        public static Material VoidMaterialAlt
        {
            get
            {
                if (_voidMaterialAlt == null)
                {
                    _voidMaterialAlt = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath("bd5cce7e28b4978499d67c043dd19555"));
                }

                return _voidMaterialAlt;
            }
        }

        private static Mesh _voidMesh;
        public static Mesh VoidMesh
        {
            get
            {
                if (_voidMesh == null)
                {
                    _voidMesh = AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GUIDToAssetPath("81efad54518ef1b4a85dfca4f9c59fa4"));
                }

                return _voidMesh;
            }
        }

        private static Mesh _genericHumanMesh;
        public static Mesh GenericHumanMesh
        {
            get
            {
                if (_genericHumanMesh == null)
                {
                    _genericHumanMesh = AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GUIDToAssetPath("17f324bf405097f4294a63c4db12b4d7"));
                }

                return _genericHumanMesh;
            }
        }

        public static string GetPackagePath(string path)
        {
            if (!string.IsNullOrEmpty(PackagePath) && !string.IsNullOrEmpty(path))
                return Path.Combine(PackagePath, path);
            else
                return path;
        }

        public static string GetMarrowAssetsPath(string path = "")
        {
            if (!string.IsNullOrEmpty(path))
                return Path.Combine("Assets", EDITOR_ASSETS_FOLDER, path);
            else
                return Path.Combine("Assets", EDITOR_ASSETS_FOLDER);
        }

        public static string GetMarrowAssetsPath(params string[] paths)
        {
            var pathArr = new List<string>();
            pathArr.Add("Assets");
            pathArr.Add(EDITOR_ASSETS_FOLDER);
            pathArr.AddRange(paths);
            if (paths.Length > 0)
                return Path.Combine(pathArr.ToArray());
            else
                return Path.Combine("Assets", EDITOR_ASSETS_FOLDER);
        }

        public static string GetLibraryPath(string path)
        {
            if (!string.IsNullOrEmpty(MarrowSDK.LibraryPath) && !string.IsNullOrEmpty(path))
                return Path.Combine(MarrowSDK.LibraryPath, path);
            else
                return path;
        }

#endif
        private static char[] invalidFilenameCharacters;
        public static string SanitizeName(string name)
        {
            if (invalidFilenameCharacters == null)
            {
                var invalids = Path.GetInvalidFileNameChars().ToList();
                invalids.Add('[');
                invalids.Add(']');
                invalids.Add('-');
                invalidFilenameCharacters = invalids.ToArray();
            }

            return String.Join("", name.Split(invalidFilenameCharacters, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string SanitizeID(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return id;
            }

            Regex regex = new Regex("[^a-zA-Z0-9.]");
            return regex.Replace(id, "");
        }

        public static bool? CheckVersionCompatibility(string otherVersion)
        {
            try
            {
                Version parsedVersion = new Version(otherVersion);
                return CheckVersionCompatibility(parsedVersion);
            }
            catch (Exception _)
            {
                return null;
            }
        }

        public static bool CheckVersionCompatibility(Version otherVersion)
        {
            return VersionGreaterOrEqualIgnorePatch(otherVersion, MinCompatibleVersion) && VersionGreaterOrEqualIgnorePatch(SDKVersion, otherVersion);
        }

        private static bool VersionGreaterOrEqualIgnorePatch(Version version1, Version version2)
        {
            if (version1.Major != version2.Major)
                return version1.Major >= version2.Major;
            if (version1.Minor != version2.Minor)
                return version1.Minor >= version2.Minor;
            return true;
        }
    }
}