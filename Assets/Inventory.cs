using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public static Inventory instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il existe plus d'une instance de Inventory");
            return;
        }
        instance = this;
    }

    public bool FindItem(string nameItem)
    {
        bool found = false;
        foreach(Item item in items)
        {
            if (string.Equals(item.name, nameItem))
            {
                found = true;
            }
        }
        return found;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }
}
