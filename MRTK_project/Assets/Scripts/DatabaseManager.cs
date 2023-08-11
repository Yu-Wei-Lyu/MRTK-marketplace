using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField]
    private List<DataObject> datas;
    [SerializeField]
    private List<DataObject> shoppingList;

    // API endpoint URL
    private string apiUrl = "ws://118.150.125.153:8765";
    private DataObject cacheDataObject;

    // Coroutine to fetch data from the API
/*    IEnumerator FetchDataFromAPI()
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
            // Do something
        }
    }*/

    public DataObject CacheDataObject
    {
        get { return this.cacheDataObject; }
    }

    public int GetShoppingListCount()
    {
        return this.shoppingList.Count;
    }

    public DataObject GetShoppingItem(int index)
    {
        DataObject targetDataObject = null;
        if (index >= 0 && index < shoppingList.Count)
        {
            targetDataObject = this.shoppingList[index];
        }
        else
        {
            Debug.LogError("Get index out of range : {index}");
        }
        return targetDataObject;
    }

    // Add cache data object to shopping list
    public void AddToShoppingList()
    {
        if (this.cacheDataObject != null)
        {
            this.shoppingList.Add(this.cacheDataObject);
        }
        else
        {
            Debug.LogError("cacheDataObject == null");
        }
    }

    // Remove data object from shopping list by index
    public void RemoveItemFromShoppingList(int index)
    {
        if (index >= 0 && index < shoppingList.Count)
        {
            shoppingList.RemoveAt(index);
        }
        else
        {
            Debug.LogError($"Removing index out of range : {index}");
        }
    }

    // OnData1ButtonClick
    public void OnData1ButtonClick()
    {
        this.cacheDataObject = datas[0];
    }

    // OnData2ButtonClick
    public void OnData2ButtonClick()
    {
        this.cacheDataObject = datas[1];
    }

    // OnData2ButtonClick
    public void OnData3ButtonClick()
    {
        this.cacheDataObject = datas[2];
    }
}
