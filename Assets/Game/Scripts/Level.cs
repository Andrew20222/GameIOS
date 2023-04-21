using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public int CurrentLevel;

    private void Start()
    {
        CurrentLevel = 1;
    }
}
