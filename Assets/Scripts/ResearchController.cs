using System.Collections.Generic;
using UnityEngine;

public class ResearchController : MonoBehaviour
{
    public static ResearchController Instance;
    private static IllnessController ic;

    [SerializeField]
    private float cure = 0.00f;
    public float Cure
    {
        set
        {
            if (value < 0.0f)
            {
                cure = 0.0f;
            }
            else if(value > 1.0f)
            {
                cure = 1.0f;
            }
            else
            {
                cure = value;
            }
        }
        get
        {
            return cure;
        }
    }
    public float CureProbablity { set; get; }

    [SerializeField]
    private float curedLevel = 0.10f;
    public float CuredLevel
    {
        get
        {
            return curedLevel;
        }
    }

    private float initalTardiness;
    [SerializeField]
    private float tardiness = 0.10f;
    private float Tardiness
    {
        set
        {
            if (value > 1.00f)
            {
                tardiness = 1.00f;
            }
            else
            {
                tardiness = value;
            }
        }
        get
        {
            return tardiness;
        }
    }

    private float initialAlertLevel;
    [SerializeField]
    private float alertLevel = 0.80f;
    public float AlertLevel
    {
        set
        {
            if (value < 0.0f)
            {
                alertLevel = 0.0f;
            }
            else
            {
                alertLevel = value;
            }
        }
        get
        {
            return alertLevel;
        }
    }
    public bool Aware
    {
        get
        {
            return cure > 0.02f;
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
        }

        initalTardiness = tardiness;
        initialAlertLevel = alertLevel;
    }

    public void Start()
    {
        if (ic == null)
        {
            ic = IllnessController.Instance;
            CureProbablity = ic.Recovery;
        }
    }

    public void Alert(City city)
    {
        AlertLevel -= AlertLevel / initialAlertLevel * Tardiness;
        Research();
        Tardiness += UnityEngine.Random.Range(0, initalTardiness);

        city.Access = false;
    }

    public void Research()
    {
        Cure += UnityEngine.Random.Range(0, ic.Recovery);
        CureProbablity = Mathf.Max(CureProbablity, cure);
    }

    public void Cured(City city)
    {
        AlertLevel += AlertLevel / initialAlertLevel * Tardiness;
        Tardiness -= UnityEngine.Random.Range(0, initalTardiness);

        city.Access = true;
    }

    public void Drop()
    {
        AlertLevel -= AlertLevel / initialAlertLevel * Tardiness;
        Tardiness += UnityEngine.Random.Range(0, initalTardiness);

        Cure -= UnityEngine.Random.Range(0, ic.Recovery);
        CureProbablity = Mathf.Max(CureProbablity, cure);
    }
}
