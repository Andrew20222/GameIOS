using System.Collections;
using UnityEngine;

public class FactoryControll : MonoBehaviour
{
    [SerializeField] private ShakeFactory shakeFactory;
    [SerializeField] private BossShakeFactory bossShakeFactory;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int countShakes = 4;
    [SerializeField] private float interval = 2;
    [SerializeField] private float startLatency = 3;
    [SerializeField] private float intervalBoss = 60;
    [SerializeField] private int countBoss = 1;

    private void Start()
    {
        StartSnakesSpawn(countShakes,interval,startLatency);
        StartBossShakeSpawn(intervalBoss);
    }

    public async void StartSnakesSpawn(int count, float interval, float startLatency) // async є асинхроність метода і використання await
    {
        await Awaiters.Seconds(startLatency); // в цьому місці ми очікуємо час interval
        if (spawnPoints != null)
        {
            for (int i = 0; i < count; i++)
            {
                shakeFactory.GetNewInstance();
                await Awaiters.Seconds(interval);
            }
        }
    }

    public async void StartBossShakeSpawn(float interval)
    {
        await Awaiters.Seconds(interval);
        if (spawnPoints != null)
        {
            if (countBoss == 1)
            {
                bossShakeFactory.GetNewInstance();
                countBoss++;
            }
        }
    }
}
