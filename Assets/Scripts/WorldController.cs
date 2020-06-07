using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; set; }

    private List<AgentController> agents;

    private int timeframe = 0;
    public static int WorldTick { get; set; }
    private int saveCount = 0;

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

        WorldTick = 1000;
        agents = new List<AgentController>();
    }
    public void FixedUpdate()
    {
        //Debug.Log(timeframe);
        if(++timeframe == WorldTick)
        {
            foreach (AgentController agent in agents)
            {
                agent.Freeze();
            }

            AgentsWrapper agentVariables = new AgentsWrapper();
            List<AgentVariables> avs = new List<AgentVariables>();
            foreach (AgentController agent in agents)
            {
                avs.Add(agent.AgentVariables);
            }
            agentVariables.agents = avs.ToArray();
            string json = JsonUtility.ToJson(agentVariables);
            System.IO.File.WriteAllText(Application.dataPath + "/Data/data" + ++saveCount + ".json", json);

            foreach (AgentController agent in agents)
            {
                agent.Unfreeze();
            }

            timeframe = 0;
        }
    }

    public void KeepTrack(AgentController agent)
    {
        agents.Add(agent);
    }
}
