using UnityEngine;

public class FruitFactory : MonoBehaviour
{
    [SerializeField] private ObjectPool applePool;
    [SerializeField] private ObjectPool pearPool;

    public GameObject CreateFruit(string type)
    {
        // String-based selection kept to decouple spawner logic from concrete fruit classes
        ObjectPool pool = type == "Apple" ? applePool : pearPool;

        // Null guard allows factory to fail safely during misconfiguration
        if (pool == null)
            return null;

        // Delegated to pool to avoid allocation during gameplay
        return pool.GetObject();
    }
}






