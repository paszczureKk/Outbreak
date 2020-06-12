using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private static Material sick;
    private static Material healthy;

    private MeshRenderer mr;

    public City City { set; get; }

    private static IllnessController ic;
    private static WorldController wc;
    private static MovementController mc;
    public enum Gender
    {
        Male,
        Female,
        None
    }

    private AgentBirthController agc = null;
    private AgentTargetController atc = null;
    private List<AgentController> others;
    public void Awake()
    {
        mr = gameObject.transform.GetComponent<MeshRenderer>();
        if (healthy == null)
        {
            healthy = mr.material;
        }
        if (sick == null)
        {
            sick = new Material(mr.material);
            sick.color = Color.red;
        }

        agc = this.gameObject.GetComponent<AgentBirthController>();
        atc = this.gameObject.GetComponent<AgentTargetController>();

        if (ic == null)
        {
            ic = IllnessController.Instance;
        }
        if (wc == null)
        {
            wc = WorldController.Instance;
        }
        if (mc == null)
        {
            mc = MovementController.Instance;
        }
        setRandomAge();
        setRandomGender();
        Illness = false;

        others = new List<AgentController>();
    }

    public Gender mGender { set; get; }
    private bool illness;
    public bool Illness
    {
        set
        {
            if(value != illness)
            {
                illness = value;
                if (illness == true)
                {
                    mr.material = sick;
                }
                else
                {
                    mr.material = healthy;
                }
            }
        }
        get
        {
            return illness;
        }
    }

    private int age = 0;
    public int Age
    {
        set
        {
            age = value;

            if (age == 13)
            {
                atc.PickWork();
            }
            else if (age == 50)
            {
                atc.PickWork();
            }

        }
        get { return age; }
    }

    public void OnCollisionEnter(Collision collision)
    {
        AgentController other = collision.gameObject.GetComponent<AgentController>();
        if(other == null)
        {
            return;
        }
        if(ic == null)
        {
            return;
        }

        foreach (AgentController agent in others)
        {
            if(agent == other)
            {
                return;
            }
        }

        others.Add(other);

        if(this.Illness || other.Illness)
        {
            float rand = UnityEngine.Random.value;
            if (rand < ic.Infectiousness)
            {
                this.Illness = other.Illness = true;
            }
        }

        if(this.mGender == Gender.Female && other.mGender == Gender.Male)
        {
            agc.Pregnant(other);
        }
    }

    public void Tick()
    {
        if (UnityEngine.Random.value < IllnessController.Instance.Recovery)
        {
            Illness = false;
        }
        else if (UnityEngine.Random.value < IllnessController.Instance.IllnessProbability)
        {
            Illness = true;
        }

        others.Clear();

        if (City.Access == true && UnityEngine.Random.value < mc.TravelProbability)
        {
            City city = wc.TownTravel(City);
            
            if(city != null)
            {
                City = city;
            }
            atc.PickWork();
        }

        agc.Tick();
    }

    private void setRandomAge()
    {
        float rand_age = UnityEngine.Random.value;
        if (rand_age < 0.02f)
        {
            Age = UnityEngine.Random.Range(1, 5);
        }
        else if (rand_age < 0.1f)
        {
            Age = UnityEngine.Random.Range(5, 15);
        }
        else if (rand_age < 0.6f)
        {
            Age = UnityEngine.Random.Range(15, 30);
        }
        else if (rand_age < 0.8f)
        {
            Age = UnityEngine.Random.Range(30, 50);
        }
        else if (rand_age < 0.9f)
        {
            Age = UnityEngine.Random.Range(50, 70);
        }
        else
        {
            Age = UnityEngine.Random.Range(70, 100);
        }
    }
    private void setRandomGender()
    {
        float rand_gender = UnityEngine.Random.value;
        if (rand_gender > 0.5)
        {
            mGender = Gender.Male;
        }
        else
        {
            mGender = Gender.Female;
        }
    }
}
