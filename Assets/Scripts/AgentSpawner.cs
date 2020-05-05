using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField]
    private TextAsset jsonFile;

    [SerializeField]
    private GameObject agent_template;

    [SerializeField]
    private GameObject agentRealm;

    private List<GameObject> agents;

    // Start is called before the first frame update
    public void Awake()
    {
        agentRealm = Instantiate<GameObject>(agentRealm);

        AgentsWrapper agentVariables = JsonUtility.FromJson<AgentsWrapper>(jsonFile.text);
        agents = new List<GameObject>();

        foreach (AgentVariables variables in agentVariables.agents)
        {
            GameObject agent = Instantiate<GameObject>(agent_template, agentRealm.transform);
            AgentController ac = agent.GetComponent<AgentController>();
            ac.AgentVariables = variables;
            agents.Add(agent);
        }
    }
}
