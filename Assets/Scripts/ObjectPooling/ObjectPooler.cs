using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public Dictionary<string,Queue<Component>> poolDictionary = new Dictionary<string, Queue<Component>>();

    public void EnqueueObject<T> (T item,string name) where T : Component
    {
        if(!item.gameObject.activeSelf) { return; }
        item.transform.position = Vector2.zero;
        poolDictionary[name].Enqueue(item);
        item.gameObject.SetActive(false);
    }
    public T DequeueObject<T> (string key) where T : Component
    {
        return (T)poolDictionary[key].Dequeue();
    }
    public void SetupPool<T>(T pooledItemPrefab,int poolSize, string dictionaryEntry) where T : Component
    {
        poolDictionary.Add(dictionaryEntry, new Queue<Component>());
        for(int i = 0; i < poolSize; i++)
        {
            T pooledInstance = Object.Instantiate(pooledItemPrefab);
            pooledItemPrefab.gameObject.SetActive(false);
            poolDictionary[dictionaryEntry].Enqueue((T)pooledInstance);
        }
    }
}
