using UnityEngine;

public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T prefab;
    [SerializeField] private Transform[] spawnPoints;
    private int _minIndexSpawnPoints = 0;
    private int _maxIndexSpawnPoints = 2;

    public  T GetNewInstance()
    {
        return Instantiate(prefab, spawnPoints[Random.Range(_minIndexSpawnPoints, _maxIndexSpawnPoints)].position, spawnPoints[Random.Range(_minIndexSpawnPoints, _maxIndexSpawnPoints)].rotation);
    }
}
