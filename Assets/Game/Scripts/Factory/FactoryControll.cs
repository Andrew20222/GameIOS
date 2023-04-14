using System.Collections;
using UnityEngine;

public class FactoryControll : MonoBehaviour
{
    [SerializeField] private ShakeFactory shakeFactory;
    [SerializeField] private float timeToSpawn;

    private void Update()
    {
        StartCoroutine(startSpawn());
    }
    private IEnumerator startSpawn()
    {
        yield return new WaitForSeconds(timeToSpawn);
        shakeFactory.GetNewInstance();
    }
}
