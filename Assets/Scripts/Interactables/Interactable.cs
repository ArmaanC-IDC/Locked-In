using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material originalMaterial;
    [SerializeField] private Material glowMaterial;
    private GameObject mesh;
    private bool emissionEnabled = false;

    protected void Start()
    {
        mesh = FindInChildren(transform, "Mesh").gameObject;
        if (mesh==null)
        {
            mesh = gameObject;
        }

        objectRenderer = mesh.GetComponent<Renderer>();
        
        originalMaterial = objectRenderer.material;
        if (glowMaterial==null)
        {
            glowMaterial = new Material(Shader.Find("Standard"));
            glowMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            glowMaterial.EnableKeyword("_EMISSION");
            glowMaterial.SetColor("_EmissionColor", Color.blue * 2f);
        }

    }

    #region handle hover glow
    private Transform FindInChildren(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindInChildren(child, name);
            if (result != null)
                return result;
        }

        return null;
    }

    public void OnHover()
    {
        if (!emissionEnabled && objectRenderer != null && objectRenderer.material.HasProperty("_EmissionColor"))
        {
            objectRenderer.material = glowMaterial;
            emissionEnabled = true;
        }
    }

    public void ClearHover()
    {
        if (emissionEnabled && objectRenderer != null)
        {
            objectRenderer.material = originalMaterial;
            emissionEnabled = false;
        }
    }

    #endregion

    public virtual void OnInteract(Item item)
    {
        Debug.Log("UNIMPLEMENTED INTERACTION");
    }
}

