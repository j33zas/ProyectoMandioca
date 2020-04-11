using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
{    
    public ItemWorld SpawnItem(ItemWorld item, Transform position)
    {
        ItemWorld myItem = GameObject.Instantiate(item);
        myItem.transform.position = position.position;
        return myItem;

    }
    public GameObject SpawnItem(GameObject gameObject, Transform position)
    {
        GameObject myItem = GameObject.Instantiate(gameObject);
        myItem.transform.position = position.position;
        return myItem;
    }

    public List<ItemWorld> spawnListItems(ItemWorld obj, Transform pos, int quantity)
    {
        List<ItemWorld> aux = new List<ItemWorld>();
        for (int i = 0; i < quantity; i++)
        {
            aux.Add(SpawnItem(obj, pos));
        }
        return aux;
    }
    public List<GameObject> spawnListItems(GameObject obj, Transform pos, int quantity)
    {
        List<GameObject> aux = new List<GameObject>();
        for (int i = 0; i < quantity; i++)
        {
            aux.Add(SpawnItem(obj, pos));
        }
        return aux;
    }

    public GameObject SpawnItem(Item item, Transform position)
    {
        GameObject myItem = GameObject.Instantiate(item.model);
        myItem.transform.position = position.position;
        return myItem;
    }

   
    public GameObject SpawnByWheel(SpawnData data, Transform pos)
    {
        GameObject myGameObject;
        List<System.Tuple<int, GameObject>> tuples = new List<System.Tuple<int, GameObject>>();

        for (int i = 0; i < data.ListaItems.Length; i++)
        {
            tuples.Add(new System.Tuple<int, GameObject>(data.ListaItems[i].probability, data.ListaItems[i].item));
        }
      
        myGameObject = Tools.Extensions.Extensions.WheelSelection<GameObject>(tuples);

        GameObject.Instantiate(myGameObject, pos.position , pos.rotation);
        return myGameObject;
    }
   
}
