#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class IconGenerator : MonoBehaviour
{
    public PrefabRegistry prefabRegistry;
    public Camera renderCamera;
    public int iconSize;
    public string outputFolder;

    [ContextMenu("Generate Prefab Icons")]
    public void GeneratePrefabIcons()
    {
        if (prefabRegistry == null || renderCamera == null)
        {
            Debug.LogError("PrefabRegistry or Camera is not assigned");
            return;
        }

        if (!Directory.Exists(outputFolder))
        {
            Debug.LogError("Directory specified does not exist");
        }

        foreach (GameObject prefab in prefabRegistry.prefabs)
        {
            GameObject instance = Instantiate(prefab);
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.identity;

            Bounds bounds = GetBounds(instance);
            PositionCamera(bounds);

            RenderTexture rt = new RenderTexture(iconSize, iconSize, 24);
            renderCamera.targetTexture = rt;

            renderCamera.Render();

            RenderTexture.active = rt;

            Texture2D tex = new Texture2D(iconSize, iconSize, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
            tex.Apply();

            renderCamera.targetTexture = null;
            RenderTexture.active = null;
            rt.Release();

            DestroyImmediate(instance);

            // Save PNG
            byte[] pngData = tex.EncodeToPNG();
            if (pngData != null)
            {
                string fileName = prefab.name.ToLower() + ".png";
                string filePath = Path.Combine(outputFolder, fileName);
                File.WriteAllBytes(filePath, pngData);
                Debug.Log("Saved icon: " + filePath);
            }

            // Destroy temp texture to avoid memory leak
            DestroyImmediate(tex);
        }

        AssetDatabase.Refresh();
        Debug.Log("Prefab icon generation complete.");
    }

    private Bounds GetBounds(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(go.transform.position, Vector3.one);
        
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }

    private void PositionCamera(Bounds bounds)
    {
        renderCamera.transform.position = bounds.center + new Vector3(1, 1, 1).normalized * bounds.extents.magnitude * 2f;
        renderCamera.transform.LookAt(bounds.center);
        renderCamera.orthographicSize = bounds.extents.magnitude;
    }
}
#endif
