using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public static AgentSpawner Instance { get; set; }

    [SerializeField]
    private TextAsset jsonFile;

    [SerializeField]
    private GameObject agent_template;

    [SerializeField]
    private GameObject agentRealm;

    private List<GameObject> agents;

    private WorldController wc;
    private IllnessController ic;

    // Start is called before the first frame update
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

        agentRealm = Instantiate<GameObject>(agentRealm);
        agents = new List<GameObject>();
    }

    public void Start()
    {
        wc = WorldController.Instance;
        ic = IllnessController.Instance;

        AgentsWrapper agentVariables = JsonUtility.FromJson<AgentsWrapper>(jsonFile.text);
        ic.Infectiousness = (float)agentVariables.Infectiousness;
        ic.Fatality = (float)agentVariables.Fatality;

        foreach (AgentVariables variables in agentVariables.agents)
        {
            Spawn(variables);
        }
    }

    public void Spawn(AgentVariables av)
    {
        GameObject agent = Instantiate<GameObject>(agent_template, new Vector3(av.x, agent_template.transform.position.y, av.y), Quaternion.identity, agentRealm.transform);
        AgentController ac = agent.GetComponent<AgentController>();
        ac.AgentVariables = av;
        agents.Add(agent);

        wc.KeepTrack(ac);
    }
}
