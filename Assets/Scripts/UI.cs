using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;

public class UI : MonoBehaviour
{
    public RectTransform container;

    public GameObject segmentPrefab;
    public GameObject noResultsScreen;
    public GameObject inputField;
    private List<GameObject> segments = new List<GameObject>();

    public static UI instance;

    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        // preload 10 segments
        PreLoadSegments(10);
    }

    private GameObject CreateNewSegment()
    {
        GameObject segment = Instantiate(segmentPrefab, container.transform, true);
        segments.Add(segment);
        return segment;
    }
    
    // instantiates a set number of segments to use later on
    void PreLoadSegments (int amount)
    {
        for(int i = 0; i < amount; ++i)
            CreateNewSegment();
    }

    /*
     * Receives: records from API call with tournament info
     * Creates UI segments for each tournament with its id and date
     */
    public void SetSegments(JSONNode records)
    {
        DeactivateAllSegments();

        if (records.Count == 0)
        {
            noResultsScreen.SetActive(true);
            return;
        }

        noResultsScreen.SetActive(false);
        
        for (int i = 0; i < records.Count; ++i)
        {
            GameObject segment = i < segments.Count ? segments[i] : CreateNewSegment();
            segment.SetActive(true);

            TextMeshProUGUI idText = segment.transform.Find("IDText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI dateText = segment.transform.Find("DateText").GetComponent<TextMeshProUGUI>();
            
            idText.text = records[i]["id"];
            dateText.text = GetFormattedDate(records[i]["attributes"]["createdAt"]);
        }
        
        container.sizeDelta = new Vector2(container.sizeDelta.x, GetContainerHeight(records.Count));
    }

    void DeactivateAllSegments()
    {
        // loop through all segments and deactivate them
        foreach(GameObject segment in segments)
            segment.SetActive(false);
    }
    
    string GetFormattedDate (string rawDate)
    {
        DateTime date = DateTime.Parse(rawDate);

        return string.Format("{0}-{1}-{2}", date.Day, date.Month, date.Year);
    }
        
    // returns a height to make the container so it clamps to the size of all segments
    float GetContainerHeight (int count)
    {
        float height = 0.0f;
     
        // include all segment heights
        height += count * (segmentPrefab.GetComponent<RectTransform>().sizeDelta.y + 1);
     
        // include the spacing between segments
        height += count * container.GetComponent<VerticalLayoutGroup>().spacing;
     
     
        return height;
    }

    public void OnSearchById()
    {
        string text = inputField.GetComponent<TMP_InputField>().text;
        AppManager.instance.GetTournamentById(text);
    }

}
