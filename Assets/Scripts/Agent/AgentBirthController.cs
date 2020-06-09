using UnityEngine;

public class AgentBirthController : MonoBehaviour
{
    private static AgentSpawner agentSpawner;
    private static IllnessController ic;
    private static WorldController wc;
    private AgentController ac;

    private static int birthCooldown = 1;
    private int pregnancy;
    private int Pregnancy
    {
        set
        {
            if(value > pregnancy)
            {
                pregnancy = value;
            }
            else if(pregnancy > 0)
            {
                pregnancy = value;
                if(value == 0)
                {
                    Birth();
                }
            }
        }
        get
        {
            return pregnancy;
        }
    }

    private float DeathProbability
    {
        get
        {
            return ac.Age * 0.005f + 0.02f + (ac.Illness == true ? ic.Fatality : 0.0f);
        }
    }

    public void Start()
    {
        if (agentSpawner == null)
        {
            agentSpawner = AgentSpawner.Instance;
        }
        if(ic == null)
        {
            ic = IllnessController.Instance;
        }
        if(wc == null)
        {
            wc = WorldController.Instance;
        }
        ac = gameObject.transform.GetComponent<AgentController>();
    }

    public void Tick()
    {
        ac.Age++;

        Pregnancy--;

        if (UnityEngine.Random.value < DeathProbability)
        {
            ac.City.DropTrack(ac);
            Destroy(gameObject);
        }
    }

    private void Birth()
    {
        AgentController ac = agentSpawner.Spawn(gameObject.transform.position, this.ac.City);
        ac.Age = 0;
        ac.Illness = (UnityEngine.Random.value < (this.ac.Illness == true ? 1 : 0) * ic.Infectiousness);
    }

    public void Pregnant(AgentController other)
    {
        if(ac == null)
        {
            return;
        }
        if(ac.Age < 13 || ac.Age >= 50)
        {
            return;
        }

        float probability;
        if(ac.Age < 20 && ac.Age >= 13)
        {
            probability = 0.85f;
        }
        else if(ac.Age < 30 && ac.Age >= 20)
        {
            probability = 0.69f;
        }
        else if(ac.Age < 40 && ac.Age >= 30)
        {
            probability = 0.31f;
        }
        else
        {
            probability = 0.12f;
        }

        if(UnityEngine.Random.value < probability && pregnancy == 0)
        {
            pregnancy += birthCooldown;
        }
    }
}
