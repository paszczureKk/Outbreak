using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private static IllnessController ic;
    public enum Gender
    {
        Male,
        Female,
        None
    }

    private AgentMovementController amc = null;
    private AgentBirthController agc = null;
    public void Awake()
    {
        amc = this.gameObject.GetComponent<AgentMovementController>();
        agc = this.gameObject.GetComponent<AgentBirthController>();
        mGender = Gender.None;
        Illness = false;
    }
    public void Start()
    {
        ic = IllnessController.Instance;
    }

    public Gender mGender { set; get; }
    public bool Illness { set; get; }

    public AgentVariables AgentVariables
    {
        set
        {
            amc.Velocity = new Vector2(0.0f, 0.0f);
            mGender = value.gender == "M" ? Gender.Male : value.gender == "F" ? Gender.Female : Gender.None;
            Illness = value.illness;
            MovementController.Instance.Plane = new Vector2(value.x, value.y);
        }
        get
        {
            AgentVariables av = new AgentVariables();
            av.x = transform.position.x;
            av.y = transform.position.z;
            av.gender = mGender == Gender.Male ? "M" : mGender == Gender.Female ? "F" : "N";
            av.illness = Illness;
            av.age = agc.Age;

            return av;
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

        if(this.mGender == Gender.Male && other.mGender == Gender.Female)
        {
            other.Birth(this);
        }
    }

    public void Birth(AgentController other)
    {
        agc.Birth(other);
    }

    public void Freeze()
    {
        amc.enabled = false;
        agc.enabled = false;
    }

    public void Unfreeze()
    {
        amc.enabled = true;
        agc.enabled = true;
    }
}
