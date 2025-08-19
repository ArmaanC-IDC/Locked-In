using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

[ExecuteInEditMode]
public class RoomLoader : MonoBehaviour
{
    [SerializeField] private Room room;
    [SerializeField] private PrefabRegistry prefabRegistry;
    [SerializeField] private bool inEditor = false; //true if in-game editor

    [SerializeField] bool generate = false;

    private int unitSize = 2;
    private List<Vector3> wallGapsPositions = new List<Vector3>();
    private List<Quaternion> wallGapsRotations = new List<Quaternion>();
    
    void Start()
    {
        GenerateRoom();
    }

    void Update()
    {
        if (generate)
        {
            generate = false;
            GenerateRoom();
        }
    }

    private GameObject GetPrefab(string name)
    {
        int index = prefabRegistry.prefabNames.IndexOf(name);
        if (index<0)
        {
            Debug.LogError("Could not find prefab with name " + name);
            return null;
        }
        return prefabRegistry.prefabs[index];
    }

    public void GenerateRoom()
    {
        wallGapsPositions.Clear();
        wallGapsRotations.Clear();

        #region clear children
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        #endregion

        #region create floor
        GameObject floorParent = new GameObject("Floor");
        floorParent.transform.SetParent(transform);
        for (int x = 0; x<room.height; x++)
        {
            for (int z = 0; z<room.width; z++)
            {
                GameObject go = Instantiate(
                    GetPrefab("floortile"), 
                    new Vector3(x*unitSize, 0, z*unitSize - 0.4f), 
                    Quaternion.Euler(0, 0, 0), 
                    floorParent.transform
                );
                if (inEditor)
                {
                    go.AddComponent<Placeable>();
                }
            }
        }
        #endregion

        #region create ceiling
        GameObject ceilingParent = new GameObject("Ceiling");
        ceilingParent.transform.SetParent(transform);
        for (int x = 0; x<room.height; x++)
        {
            for (int z = 0; z<room.width; z++)
            {
                GameObject go = Instantiate(
                    GetPrefab("ceilingtile"), 
                    new Vector3(x*unitSize, 0, z*unitSize), 
                    Quaternion.Euler(0, 0, 0), 
                    ceilingParent.transform
                );

                if (inEditor)
                {
                    go.AddComponent<Placeable>();
                }
            }
        }
        #endregion

        #region create objects
        GameObject objectParent = new GameObject("Objects");
        objectParent.transform.SetParent(transform);
        wallGapsPositions = new List<Vector3>();
        wallGapsRotations = new List<Quaternion>();

        foreach (ObjectData obj in room.objectData)
        {
            if (obj.type==ObjectType.wall) //destroy the wall where a door is
            {
                wallGapsPositions.Add(obj.position);
                wallGapsRotations.Add(Quaternion.Euler(obj.rotation));
            }

            GameObject inSceneObj = Instantiate(
                GetPrefab(obj.prefabName),
                obj.position * unitSize,
                Quaternion.Euler(obj.rotation),
                objectParent.transform
            );

            if (inEditor)
            {
                Interactable interactable = inSceneObj.GetComponent<Interactable>();
                if (interactable!=null)
                    interactable.enabled = false;
                inSceneObj.AddComponent<Placeable>();
            }

            for (int i = 0; i < obj.paramNames.Count; i++)
            {
                SetParam(inSceneObj, obj.paramNames[i], obj.paramValues[i]);
            }
        }
        #endregion
    
        #region create walls
        GameObject wallsParent = new GameObject("Walls");
        wallsParent.transform.SetParent(transform);
        //facing pos x
        CreateWallLine(
            new Vector3(0, 0, 0),
            room.width,
            new Vector3(0, 0, 1) * unitSize,
            Quaternion.Euler(0, 90, 0),
            wallsParent.transform
        );

        //facing neg x
        CreateWallLine(
            new Vector3((room.height-1)*unitSize, 0, 0),
            room.width,
            new Vector3(0, 0, 1) * unitSize,
            Quaternion.Euler(0, -90, 0),
            wallsParent.transform
        );

        //facing pos z
        CreateWallLine(
            new Vector3(0, 0, 0),
            room.height,
            new Vector3(1, 0, 0) * unitSize,
            Quaternion.Euler(0, 0, 0),
            wallsParent.transform
        );

        //facing neg z
        CreateWallLine(
            new Vector3(0, 0, (room.width-1)*unitSize),
            room.height,
            new Vector3(1, 0, 0) * unitSize,
            Quaternion.Euler(0, 180, 0),
            wallsParent.transform
        );        
        #endregion
    }

    void CreateWallLine(Vector3 startPosition, int count, Vector3 positionOffset, Quaternion rotation, Transform parent)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = startPosition + i * positionOffset;
            int posI = wallGapsPositions.IndexOf(position);
            int rotI = wallGapsRotations.IndexOf(rotation);
            if (posI!=rotI | posI<0)
            {
                GameObject go = Instantiate(GetPrefab("wall"), position, rotation, parent);
                if (inEditor)
                {
                    go.AddComponent<Placeable>();
                }
            }
        }
    }
  
    void SetParam(GameObject root, string paramPath, string val)
    {
        string[] paramParts = paramPath.Split("/");
        if (paramParts.Length < 2)
        {
            Debug.LogError("All params must have at least two parts: Component/ParamName");
            return ;
        }

        string propertyName = paramParts[paramParts.Length - 1];
        string componentName = paramParts[paramParts.Length - 2];

        string pathToChild = string.Join("/", paramParts, 0, paramParts.Length - 2);
        Transform child;
        if (pathToChild=="")
        {
            child = root.transform;
        }else
        {
            child = root.transform.Find(pathToChild);
            if (child == null)
            {
                Debug.LogError("Child with the path " + pathToChild + " not found");
                return ;
            }
        }

        Component component = child.GetComponent(componentName);
        if (component == null)
        {
            Debug.LogError($"Component {componentName} not found in child {child.name}");
            return ;
        }

        Type type = component.GetType();
        PropertyInfo property = type.GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            object converted = Convert.ChangeType(val, property.PropertyType);
            property.SetValue(component, converted);
            return;
        }
        FieldInfo field = type.GetField(propertyName);
        if (field != null)
        {
            object converted = Convert.ChangeType(val, field.FieldType);
            field.SetValue(component, converted);
            return ;
        }
        
        Debug.LogError($"Error setting property or field {propertyName} in type {componentName}");
    }
}
