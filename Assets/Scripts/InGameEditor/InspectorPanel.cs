using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class EditableParameter
{
    public string path;
    public Type type;

    public EditableParameter(string path, Type type)
    {
        this.path = path;
        this.type = type;
    }
}

public class InspectorPanel : MonoBehaviour
{
    private List<EditableParameter> paramList = new List<EditableParameter>();

    private GameObject _currentObj;
    public GameObject currentObj{
        get { return _currentObj; }
        set { 
            _currentObj = value; 
            Debug.Log("Updated CurrentObj");
            paramList = new List<EditableParameter>();
            GetParams(_currentObj.transform, "");
            foreach (var param in paramList)
            {
                Debug.Log($"Path: {param.path}, Type: {param.type}");
            }
        }
    }

    private void GetParams(Transform root, string currentPath)
    {
        Debug.Log("searching " + root.name);
        Component[] components = root.gameObject.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component == null) { continue; }
            Type componentType = component.GetType();

            #region get fields and add them
            foreach (FieldInfo field in componentType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (Attribute.IsDefined(field, typeof(EditableParamTag)))
                {
                    paramList.Add(new EditableParameter(
                        currentPath + "/" + component.GetType().Name + "/" + field.Name,
                        field.FieldType
                    ));
                }
            }
            #endregion

            #region get properties and add them
            foreach (PropertyInfo field in componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (Attribute.IsDefined(field, typeof(EditableParamTag)))
                {
                    paramList.Add(new EditableParameter(
                        currentPath + "/" + component.GetType().Name + "/" + field.Name,
                        field.PropertyType
                    ));
                }
            }
            #endregion
        }
        foreach (Transform child in root)
        {
            GetParams(child, currentPath + "/" + child.name);
        }
    }
}
