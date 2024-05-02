using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Create New item", order = 4)]
public class SO_item : ScriptableObject
{
    public Sprite icon;
    public string id;
    public string itemname;
    public string description;
    public int maxStack;

    [Header("In Game Object")]
    public GameObject gamePrefab;
}
