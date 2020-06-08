using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public City City { set; get; }

    private static IllnessController ic;
    public enum Gender
    {
        Male,
        Female,
        None
    }

    private AgentMovementController amc = null;
    private AgentBirthController agc = null;
    private AgentTargetController atc = null;
    public void Awake()
    {
        amc = this.gameObject.GetComponent<AgentMovementController>();
        agc = this.gameObject.GetComponent<AgentBirthController>();
        atc = this.gameObject.GetComponent<AgentTargetController>();

        setRandomAge();
        setRandomGender();
        setRandomIllnessState();
    }
    public void Start()
    {
        ic = IllnessController.Instance;
    }

    public Gender mGender { set; get; }
    public bool Illness { set; get; }

    public int Age { set; get; }

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

        if(this.mGender == Gender.Male && other.mGender == Gender.Female)
        {
            other.Birth(this);
        }
    }

    public void Birth(AgentController other)
    {
        agc.Birth(other);
    }

    public void Tick()
    {
        if(WorldController.Instance.Day == true)
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
