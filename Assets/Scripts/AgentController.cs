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
