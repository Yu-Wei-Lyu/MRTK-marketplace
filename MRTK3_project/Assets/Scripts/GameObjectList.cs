using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects;

    public void push(GameObject gameObject)
    {
        gameObjects.Add(gameObject);
    }

    public bool contains(GameObject gameObject)
    {
        return gameObjects.Contains(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
