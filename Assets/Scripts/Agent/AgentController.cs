using UnityEngine;

public class AgentController : MonoBehaviour
{
    private static Material sick;
    private static Material healthy;

    private MeshRenderer mr;

    public City City { set; get; }

    private static IllnessController ic;
    public enum Gender
    {
        Male,
        Female,
        None
    }

    private AgentBirthController agc = null;
    private AgentMovementController amc = null;
    private AgentTargetController atc = null;
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
        amc = this.gameObject.GetComponent<AgentMovementController>();
        atc = this.gameObject.GetComponent<AgentTargetController>();

        ic = IllnessController.Instance;
        setRandomAge();
        setRandomGender();
        setRandomIllnessState();
    }
    public void Run()
    {
        atc.PickWork();
        amc.enabled = true;
    }

    public Gender mGender { set; get; }
    private bool illness;
    public bool Illness
    {
        set
        {
            illness = value;
            if(illness == true)
            {
                mr.material = sick;
            }
            else
            {
                mr.material = healthy;
            }
        }
        get
        {
            return illness;
        }
    }

    private int age = 100;
    public int Age
    {
        set
        {
            int temp = age;
            age = value;
            if ((temp < 13 && age >= 13) || (temp < 50 && age >= 50))
            {
                atc.PickWork();
            }
        }
        get
        {
            return age;
        }
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

        if(this.Illness || other.Illness)
        {
            if (UnityEngine.Random.value < ic.Infectiousness)
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

        if (WorldController.Instance.Day == true)
        {
            atc.enabled = false;
            atc.Work();
        }
        else
        {
            atc.enabled = true;
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

    private void setRandomIllnessState()
    {
        float rand_illness = UnityEngine.Random.value;
        if (rand_illness < ic.InitialIllnessProbability)
        {
            Illness = true;
        }
        else
        {
            Illness = false;
        }
    }
}
