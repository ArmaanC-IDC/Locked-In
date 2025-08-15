using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField] PrefabSelectionPanel prefabPanel;
    [SerializeField] PrefabRegistry prefabRegistry;

    private GameObject currentPrefab;
    private GameObject go; //the gameobject that is in the scene

    private GameObject GetPrefab(string name)
    {
        int index = prefabRegistry.prefabNames.IndexOf(name);
        if (index<0)
        {
            return null;
        }
        return prefabRegistry.prefabs[index];
    }

    void Update()
    {
        currentPrefab = GetPrefab(prefabPanel.GetSelectedPrefabName());
        if (currentPrefab==null) return ;

        if (go==null)
        {
            go = Instantiate(currentPrefab, transform);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                
            }
        }

    }
}
