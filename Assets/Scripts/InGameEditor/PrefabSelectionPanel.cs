using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrefabSelectionPanel : MonoBehaviour
{
    [SerializeField] GameObject sectionHeader;
    [SerializeField] GameObject prefabSlot;
    [SerializeField] PrefabRegistry prefabRegistry;

    [SerializeField] Transform headingParent;
    [SerializeField] Transform slotsParent;

    [SerializeField] string iconFolderPath;

    private Dictionary<string, List<string>> organizedPrefabs = new Dictionary<string, List<string>>();
    private List<string> headings;
    private List<string> prefabs;

    private int selectedHeading = 0;
    private int selectedPrefab = -1;

    // Start is called before the first frame update
    void Start()
    {
        #region organize prefabs
        for (int i = 0; i < prefabRegistry.prefabNames.Count; i++)
        {
            string location = prefabRegistry.locations[i];
            if (!organizedPrefabs.ContainsKey(location))
            {
                organizedPrefabs[location] = new List<string>();
            }
            organizedPrefabs[location].Add(prefabRegistry.prefabNames[i]);
        }
        headings = organizedPrefabs.Keys.ToList();
        #endregion

        PopulateHeadings();

        PopulatePrefabs();
    }

    void PopulateHeadings()
    {
        while (headingParent.childCount > 0)
        {
            DestroyImmediate(headingParent.GetChild(0).gameObject);
        }

        for (int i = 0; i<headings.Count; i++)
        {
            GameObject header = Instantiate(sectionHeader, headingParent);
            header.GetComponent<RectTransform>().anchoredPosition = new Vector2(-340 + 110*i, 0);
            header.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = headings[i];

            Color currentColor = header.GetComponent<Image>().color;
            Color newColor;
            if (i==selectedHeading)
            {
                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1);
            }else
            {
                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
            }
            header.GetComponent<Image>().color = newColor;

            int index = i;
            header.GetComponent<Button>().onClick.AddListener(() => SelectHeading(index));
        }
    }

    void PopulatePrefabs()
    {
        while (slotsParent.childCount > 0)
        {
            DestroyImmediate(slotsParent.GetChild(0).gameObject);
        }

        prefabs = organizedPrefabs[headings[selectedHeading]];
        for (int i = 0; i<prefabs.Count; i++)
        {
            GameObject slot = Instantiate(prefabSlot);
            slot.transform.SetParent(slotsParent);
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-360 + 60*i, 0);
            slot.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            int index = prefabRegistry.prefabNames.IndexOf(prefabs[i]);
            slot.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = prefabRegistry.displayNames[index];

            //load the image
            byte[] bytes = File.ReadAllBytes(iconFolderPath + "/" + prefabs[i].ToLower() + ".png");
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(
                texture, 
                new Rect(0f, 0f, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f)
            );
            slot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;

            Color currentColor = slot.GetComponent<Image>().color;
            Color newColor;
            if (i==selectedPrefab)
            {
                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            }else
            {
                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
            }
            slot.GetComponent<Image>().color = newColor;

            int _index = i;
            slot.GetComponent<Button>().onClick.AddListener(() => SelectPrefab(_index));
        }
    }

    void SelectHeading(int num)
    {
        selectedPrefab = -1;
        selectedHeading = num;

        PopulateHeadings();
        PopulatePrefabs();
    }

    void SelectPrefab(int num)
    {
        selectedPrefab = num;
        PopulatePrefabs();
    }

    public string GetSelectedPrefabName()
    {
        if (selectedPrefab==-1)
        {
            return "";
        }
        return prefabs[selectedPrefab];
    }
}
