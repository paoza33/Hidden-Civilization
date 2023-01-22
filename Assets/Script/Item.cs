using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// asset menu pour pouvoir créer des fichiers basés sur ces scriptable object que l'on retrouve sur l'onglet "asset" de unity
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    public string typeOfItem; //Weapon, Key, quest object
    public string nameItem;
    public string descriptionItem;
    public Sprite sprite;
}
