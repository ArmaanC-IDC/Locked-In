using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


[ExecuteInEditMode]
public class PrefabEditorLoader : MonoBehaviour
{
    public PrefabRegistry prefabRegistry;
    
    public string prefabFolder = "Assets/Prefabs";

    public bool load = false;

    void Update()
    {
        if (!Application.isPlaying && load)
        {
            load = false;

            if (prefabRegistry == null)
            {
                Debug.LogError("Assign prefabRegistry ScriptableObject asset!");
                return;
            }

            prefabRegistry.prefabNames = new List<string>();
            prefabRegistry.prefabs = new List<GameObject>();
            prefabRegistry.locations = new List<string>();

            // Find all prefab GUIDs in the folder
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { prefabFolder });

            List<GameObject> loadedPrefabs = new List<GameObject>();
            List<string> loadedPrefabNames = new List<string>();
            List<string> loadedPrefabLocations = new List<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                {
                    loadedPrefabs.Add(prefab);
                    loadedPrefabNames.Add(prefab.name.ToLower());

                    string directoryPath = Path.GetDirectoryName(path);
                    string location = "";
                    if (directoryPath.Length > prefabFolder.Length)
                    {
                        location = directoryPath.Substring(prefabFolder.Length + 1);
                    }
                    loadedPrefabLocations.Add(location.ToLower());
                }
            }

            // Assign loaded prefabs to the prefabRegistry
            for (int i = 0; i<loadedPrefabs.Count; i++)
            {
                prefabRegistry.prefabNames.Add(loadedPrefabNames[i]);
                prefabRegistry.prefabs.Add(loadedPrefabs[i]);
                prefabRegistry.locations.Add(loadedPrefabLocations[i]);
            }

            EditorUtility.SetDirty(prefabRegistry);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Loaded {loadedPrefabs.Count} prefabs from folder '{prefabFolder}' into prefabRegistry.");
        }
    }
}
