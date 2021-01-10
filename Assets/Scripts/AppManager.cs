using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class AppManager : MonoBehaviour
{
    public JSONNode jsonResult;
    public GameObject loadingScreen;
    public static AppManager instance;
    
    private string url = "https://api.pubg.com/tournaments";
    private const string APIKey =
        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJmNmE5NDQ5MC0zMWRhLTAxMzktZGQ1ZC03MWI3MzQ5YWMyMzAiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNjA5ODg5MDE4LCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6Ii04YjMyODhhMi01OGQ2LTQyOGYtODc0Ni1iNzk3NmZmNWVmZDYifQ.qNONTnCm7pVVXBoYty9lybnKzgpuOPTW71K-uYFJ0mM";

    private void Awake()
    {
        instance = this;
        instance.StartCoroutine("GetTournaments");
    }

    /*
     * Send API request for PUBG Tournaments
     * Returns JSON file
     */
    IEnumerator GetTournaments()
    {
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = url;
        webRequest.SetRequestHeader("accept", "application/vnd.api+json");
        webRequest.SetRequestHeader("Authorization", $"bearer {APIKey}");
        yield return webRequest.SendWebRequest();
        string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
        jsonResult = JSON.Parse(rawJson);
        UI.instance.SetSegments(jsonResult["data"]);
        loadingScreen.SetActive(false);
    }

    /*
     * Filters tournaments list according to input received
     * Due to TMP_InputField the last character is not sent until enter 
     */
    public void GetTournamentById(string id)
    {
        if (String.IsNullOrEmpty(id))
        {
            UI.instance.SetSegments(jsonResult["data"]);
            return;
        }
        
        JSONArray records = jsonResult["data"].AsArray;
        JSONArray results = new JSONArray();

        for (int i = 0; i < records.Count; ++i)
        {
            string tmp = records[i]["id"];
            if (tmp.ToLower().StartsWith(id.ToLower()))
                results.Add(records[i]);
        }
        
        UI.instance.SetSegments(results);
    }

}
