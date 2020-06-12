using UnityEngine;

public class IllnessController : MonoBehaviour
{
    public static IllnessController Instance { get; set; }

    private VariablesController vc;

    public float Infectiousness
    {
        get
        {
            return vc.Infectiousness;
        }
        set
        {
            vc.Infectiousness = value;
        }
    }
    public float Fatality
    {
        get
        {
            return vc.Fatality;
        }
        set
        {
            vc.Fatality = value;
        }
    }
    public float IllnessProbability
    {
        get
        {
            return vc.IllnessProbability;
        }
        set
        {
            vc.IllnessProbability = value;
        }
    }
    public float Recovery
    {
        get
        {
            return vc.Recovery;
        }
        set
        {
            vc.Recovery = value;
        }
    }
    public float MutationProbability
    {
        get
        {
            return vc.MutationProbability;
        }
        set
        {
            vc.MutationProbability = value;
        }
    }

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

        vc = VariablesController.Instance;

        if (vc.Random == true)
        {
            Infectiousness = UnityEngine.Random.Range(0.05f, vc.Infectiousness);
            Fatality = UnityEngine.Random.Range(0.01f, vc.Fatality);
            IllnessProbability = UnityEngine.Random.Range(0.01f, vc.IllnessProbability);
            Recovery = UnityEngine.Random.Range(0.05f, vc.Recovery);
            MutationProbability = UnityEngine.Random.Range(0.01f, vc.MutationProbability);
        }
    }
}
