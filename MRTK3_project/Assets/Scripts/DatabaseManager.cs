using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    // API endpoint URL
    private string apiUrl = "https://example.com/api/getdata";

    // Coroutine to fetch data from the API
    IEnumerator FetchDataFromAPI()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching data: " + www.error);
            }
            else
            {
                // Data is successfully fetched
                string jsonData = www.downloadHandler.text;
                ProcessData(jsonData);
            }
        }
    }

    // Process the fetched data (in JSON format)
    private void ProcessData(string jsonData)
    {
        // Here you can parse the JSON data and use it as needed
        // For example, you can use JSONUtility to deserialize the JSON data into a custom class

        // Example of deserializing JSON data into a list of objects
        DataObject[] dataObjects = JsonUtility.FromJson<DataObject[]>(jsonData);

        // Now you can use the dataObjects array to work with the fetched data
        foreach (DataObject dataObject in dataObjects)
        {
            Debug.Log("ID: " + dataObject.id + ", Name: " + dataObject.name + ", Age: " + dataObject.age);
        }
    }
}

// Custom class to represent the data structure
[System.Serializable]
public class DataObject
{
    public int id;
    public string name;
    public int age;
}
