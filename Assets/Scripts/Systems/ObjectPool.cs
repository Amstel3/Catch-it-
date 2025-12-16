using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        // Prewarmed to avoid runtime allocations during early gameplay
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, transform);

            // Pool reference injected to allow self-return without global lookups
            obj.GetComponent<FallingObject>()?.SetPool(this);

            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        // Reuse preferred to keep memory and GC stable during spikes
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Soft cap used to prevent silent, unbounded pool growth
        if (pool.Count >= poolSize * 2)
        {
            Debug.LogWarning("Pool limit reached");
            return null;
        }

        // Fallback allocation allowed to avoid hard failure under load
        GameObject newObj = Instantiate(objectPrefab, transform);
        newObj.GetComponent<FallingObject>()?.SetPool(this);
        newObj.SetActive(true);
        pool.Add(newObj);

        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        // Deactivated instead of destroyed to keep references and state reusable
        obj.SetActive(false);
    }
}



