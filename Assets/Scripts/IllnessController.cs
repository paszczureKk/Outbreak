using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllnessController : MonoBehaviour
{
    public static IllnessController Instance { get; set; }
    public float Infectiousness { get; set; }
    public float Fatality { get; set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        Infectiousness = UnityEngine.Random.value * 100;
        Fatality = UnityEngine.Random.value * 100;
    }
}
