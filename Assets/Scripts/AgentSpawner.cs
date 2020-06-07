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

        AgentsWrapper agentVariables = JsonUtility.FromJson<AgentsWrapper>(jsonFile.text);
        foreach (AgentVariables variables in agentVariables.agents)
        {
            Spawn(agentRealm.transform, variables);
        }
    }

    public void Spawn(Transform transform, AgentVariables av)
    {
        GameObject agent = Instantiate<GameObject>(agent_template, transform);
        AgentController ac = agent.GetComponent<AgentController>();
        if(ac == null)
        {
            Debug.Log("blah");
        }
        ac.AgentVariables = av;
        agents.Add(agent);

        wc.KeepTrack(ac);
    }
}
