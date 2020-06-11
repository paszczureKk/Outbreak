using UnityEngine;

public class IllnessController : MonoBehaviour
{
    public static IllnessController Instance { get; set; }
    public float Infectiousness { get; set; }
    public float Fatality { get; set; }
    public float IllnessProbability { get; set; }
    public float Recovery { get; set; }

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

        Infectiousness = UnityEngine.Random.Range(0.05f, 0.20f);
        Fatality = UnityEngine.Random.Range(0.01f, 0.10f);
        IllnessProbability = UnityEngine.Random.Range(0.01f, 0.05f);
        Recovery = UnityEngine.Random.Range(0.05f, 0.30f);
    }
}
