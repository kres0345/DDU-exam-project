using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandling : MonoBehaviour
{
    public static MusicHandling Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}