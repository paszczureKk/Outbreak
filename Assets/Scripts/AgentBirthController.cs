using UnityEngine;

public class AgentBirthController : MonoBehaviour
{
    private static AgentSpawner agentSpawner;
    private static IllnessController ic;
    private static WorldController wc;
    private AgentController ac;

    private static int birthCooldown = 1;
    private int lastBirth = 0;
    public int Age { get; set; }

    private float DeathProbability
    {
        get
        {
            return Age * 0.005f + 0.02f + (ac.Illness == true ? ic.Fatality : 0.0f);
        }
    }

    public void Awake()
    {
        this.Age = 0;
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
        ac = this.gameObject.GetComponent<AgentController>();
    }

    // Update is called once per frame
    public void Tick()
    {
        Age++;
            
        if(lastBirth > 0)
        {
            lastBirth--;
        }

        if (UnityEngine.Random.value < DeathProbability)
        {
            wc.DropTrack(ac);
            Destroy(gameObject);
        }
    }

    public void Birth(AgentController other)
    {
        if(Age < 13 || Age >= 50)
        {
            return;
        }

        float probability;
        if(Age < 20 && Age >= 13)
        {
            probability = 0.85f;
        }
        else if(Age < 30 && Age >= 20)
        {
            probability = 0.69f;
        }
        else if(Age < 40 && Age >= 30)
        {
            probability = 0.31f;
        }
        else
        {
            probability = 0.12f;
        }

        if(UnityEngine.Random.value < probability && lastBirth == 0)
        {
            AgentVariables av = new AgentVariables();
            AgentVariables otherVariables = other.AgentVariables;
            av.x = (ac.AgentVariables.x + otherVariables.x) / 2.0f;
            av.y = (ac.AgentVariables.y + otherVariables.y) / 2.0f;
            av.gender = UnityEngine.Random.value < 0.5f ? "M" : "F";
            av.illness = (UnityEngine.Random.value < ic.Infectiousness ? true : false);

            agentSpawner.Spawn(av);

            lastBirth += birthCooldown;
        }
    }
}
