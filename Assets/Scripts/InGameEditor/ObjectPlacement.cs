using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectPlacement : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private PrefabSelectionPanel prefabPanel;
    [SerializeField] private PrefabRegistry prefabRegistry;
    [SerializeField] private Room currentRoom;
    [SerializeField] private RoomLoader roomLoader;
    [SerializeField] private InputAction rotateInputAction;
    [SerializeField] private InspectorPanel inspectorPanel;

    [Header("Values")]
    [SerializeField] private float surfaceSnappingInverval = 0.1f;

    private string currentPrefab = "";
    private GameObject indicator; //the gameobject that is in the scene
    private int tileSize = 2;

    private Vector3 lastMousePosition = Vector3.zero;
    private float tolerance = 0.1f;

    #region enable and disable input actions
    void OnEnable()
    {
        rotateInputAction.Enable();
        rotateInputAction.started += OnRotatePressed;
    }
    void OnDisable()
    {
        rotateInputAction.started -= OnRotatePressed;
        rotateInputAction.Disable();
    }
    #endregion

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
        UpdateIndicator();

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.GetComponentInParent<Placeable>())
                {
                    inspectorPanel.currentObj = hit.transform.gameObject.GetComponentInParent<Placeable>().gameObject;
                }else
                {
                    Debug.Log("no placeable");
                }
            }else
            {
                PlaceObject();
            }
        }
    }

    PlacementType GetPlacementType()
    {
        return prefabRegistry.placementTypes[prefabRegistry.prefabNames.IndexOf(currentPrefab)];
    }

    void UpdateIndicator()
    {
        #region create indicator if needed
        currentPrefab = prefabPanel.GetSelectedPrefabName();
        if (currentPrefab=="") return ;

        if (indicator==null || currentPrefab != indicator.name)
        {
            if (indicator!=null) DestroyImmediate(indicator);
            indicator = Instantiate(GetPrefab(currentPrefab));

            Collider[] colliders = indicator.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            Interactable interactable = indicator.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.enabled = false;
            }

            indicator.name = currentPrefab;
        }
        #endregion

        #region update indicator's position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (Vector3.Distance(Input.mousePosition, lastMousePosition) > tolerance)
            {
                lastMousePosition = Input.mousePosition;
                Vector3 position;
                if (GetPlacementType()==PlacementType.Surface)
                {
                    position = new Vector3(
                        Mathf.Round(hit.point.x / (tileSize*surfaceSnappingInverval)),
                        Mathf.Round(hit.point.y / (tileSize*surfaceSnappingInverval)),
                        Mathf.Round(hit.point.z / (tileSize*surfaceSnappingInverval))
                    ) * (tileSize*surfaceSnappingInverval);
                }else
                {
                    position = new Vector3(
                        Mathf.Round(hit.point.x / tileSize),
                        Mathf.Round(hit.point.y / tileSize),
                        Mathf.Round(hit.point.z / tileSize)
                    ) * tileSize;
                }
                indicator.transform.position = position;
            }
        }else
        {
            indicator.transform.position = new Vector3(0, -1000, 0);
        }
        #endregion
    }

    void OnRotatePressed(InputAction.CallbackContext context)
    {
        if (GetPlacementType()==PlacementType.Surface)
        {
            indicator.transform.rotation *= Quaternion.Euler(0, 30, 0);
        }else
        {
            indicator.transform.rotation *= Quaternion.Euler(0, 90, 0);
        }
    }

    void PlaceObject()
    {
        ObjectData data = ScriptableObject.CreateInstance<ObjectData>();
        if (GetPlacementType()==PlacementType.Wall)
        {
            data.type = ObjectType.wall;
        }else
        {
            data.type = ObjectType.item;
        }
        data.prefabName = currentPrefab;
        data.position = new Vector3(
            indicator.transform.position.x/tileSize, 
            indicator.transform.position.y/tileSize,
            indicator.transform.position.z/tileSize
        );
        data.rotation = indicator.transform.rotation.eulerAngles;
        Debug.Log("TODO: implement params");
        data.paramNames = new List<string>();
        data.paramValues = new List<string>();
        currentRoom.objectData.Add(data);

        roomLoader.GenerateRoom();
    }
}
