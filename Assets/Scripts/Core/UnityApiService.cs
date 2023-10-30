using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace SR
{
    public class UnityApiService : MonoBehaviour
{
    public static UnityApiService Instance;
    private const string BaseUrl = "https://api.openworldnft.io/gold-game/";
    private const string ChildUrl = "https://api.openworldnft.io/spaceships/";


    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Get(string collection, string key, string queryvalue, Action<string> respone = null, Action<string> error = null,bool isRoot = false)
    {
        // Construct the complete URL with parameters
        string targetURL = isRoot? BaseUrl :ChildUrl;
        string url = $"{targetURL}{collection}?{key}={queryvalue}";
        StartCoroutine(SendGetApi( url, respone, error));
    }

    public IEnumerator SendGetApi(string url, Action<string> respone = null, Action<string> error = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                error.Invoke(webRequest.error);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                respone.Invoke(responseText);
                Debug.Log("API Response: " + responseText);
            }
        }
    }

    public void Post(string collection, WWWForm form, string jsonForm, Action<string> respone = null, Action<string> error = null, bool isRoot = false)
    {
        string targetURL = isRoot? BaseUrl :ChildUrl;
        string url = $"{targetURL}{collection}";
        StartCoroutine(SendPostApi(url, form, jsonForm, respone, error));
    }

    private IEnumerator SendPostApi(string url, WWWForm form = null, string jsonForm = null, Action<string> respone = null, Action<string> error = null)
    {
        if (jsonForm != null)
        {
            
            using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(url, jsonForm))
            {
                byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonForm);

                webRequest.uploadHandler  = new UploadHandlerRaw(postData);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                Debug.Log(jsonForm);

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + JsonConvert.SerializeObject(webRequest.downloadHandler.text));
                    error.Invoke(webRequest.downloadHandler.text);
                }
                else
                {
                    // Process the API response here
                    string responseText = webRequest.downloadHandler.text;
                    Debug.Log(responseText);
                    respone.Invoke(responseText);
                }
            }
        }
        else
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                   Debug.LogError("Error: " + JsonConvert.SerializeObject(webRequest.downloadHandler.text));
                       
                    error.Invoke(webRequest.downloadHandler.text);
                }
                else
                {
                    // Process the API response here
                    string responseText = webRequest.downloadHandler.text;
                    Debug.Log(responseText);
                    respone.Invoke(responseText);
                }
            }

    }
}
}