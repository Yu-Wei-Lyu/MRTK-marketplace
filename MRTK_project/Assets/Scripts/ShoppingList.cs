using System.Collections.Generic;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    private Dictionary<int, int> _shoppingDictionary = new Dictionary<int, int>();

    public int Count
    {
        get
        {
            return _shoppingDictionary.Count;
        }
    }

    // Get list of ID
    public Dictionary<int, int> GetDictionary()
    {
        return new Dictionary<int, int>(_shoppingDictionary);
    }

    // Add furnitures
    public void AddFurnitures(int id, int amount)
    {
        if (_shoppingDictionary.ContainsKey(id))
        {
            _shoppingDictionary[id] += amount;
        }
        else
        {
            _shoppingDictionary[id] = amount;
        }
    }

    // Decrease or delete furniture
    public void DecreaseFurnitureByID(int id, int amount)
    {
        if (_shoppingDictionary.ContainsKey(id))
        {
            int currentAmount = _shoppingDictionary[id];
            currentAmount -= amount;
            if (currentAmount <= 0)
            {
                _shoppingDictionary.Remove(id);
            }
            else
            {
                _shoppingDictionary[id] = currentAmount;
            }
        }
    }
}
