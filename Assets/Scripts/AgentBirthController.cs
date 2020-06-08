using UnityEngine;

public class AgentBirthController : MonoBehaviour
{
    private static AgentSpawner agentSpawner;
    private static IllnessController ic;
    private static WorldController wc;
    private AgentController ac;

    private static int birthCooldown = 1;
    private int pregnancy = 0;
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

    public void Tick()
    {
        Age++;
            
        if(pregnancy > 0)
        {
            pregnancy--;
            Birth();
        }

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

        if(UnityEngine.Random.value < probability && pregnancy == 0)
        {
            pregnancy += birthCooldown;
        }
    }
}
