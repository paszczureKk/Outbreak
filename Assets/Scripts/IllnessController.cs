using UnityEngine;

public class IllnessController : MonoBehaviour
{
    public static IllnessController Instance { get; set; }
    public float Infectiousness { get; set; }
    public float Fatality { get; set; }
    public float InitialIllnessProbability { get; set; }

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

        Infectiousness = UnityEngine.Random.Range(0.25f, 0.80f);
        Fatality = UnityEngine.Random.Range(0.01f, 0.10f);
        InitialIllnessProbability = UnityEngine.Random.Range(0.10f, 0.40f);
    }
}
