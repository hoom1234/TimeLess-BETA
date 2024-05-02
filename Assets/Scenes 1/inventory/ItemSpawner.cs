using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    public List<Itemobject> itemobjects;
    public float minRadius = 2.0f;
    public float maxRadius = 10.0f;

    public Transform Player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnItem(SO_item item, int amount) //DROP FROM PLAYER!!!!!!
    {
        if (item.gamePrefab == null)
        {
            Debug.LogError("No prefab in" + item.name + "Pleaseeeeeeeee");
            return;
        }


        Vector2 randPos = Random.insideUnitCircle.normalized * minRadius;
        Vector3 offset = new Vector3(randPos.x, 0, randPos.y);
        GameObject spawnItem = Instantiate(item.gamePrefab, Player.position + offset, Quaternion.identity);

        spawnItem.GetComponent<Itemobject>().SetAmount(amount);
    }


    public void SpawnItemByGUI(int SpawnAmount =1 ) //DROP FROM PLAYER!!!!!!
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            int ind = Random.Range(0, itemobjects.Count);
            float distance = Random.Range(minRadius, maxRadius);
            Vector2 randPoint = Random.insideUnitCircle.normalized * distance;
            Vector3 offset = new Vector3(randPoint.x, 0, randPoint.y);
            Itemobject itemobjectspawn = Instantiate(itemobjects[ind], Player.position + offset, Quaternion.identity);
            itemobjectspawn.RandomAmount();
        }
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Spawn a Random Item"))
        {
            SpawnItemByGUI();
        }
        
    }

}
