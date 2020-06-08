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

    [SerializeField]
    [Range(0, 100)]
    private int population;

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

        for (int i = 0; i < population; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        AgentController ac = agent.GetComponent<AgentController>();
        agents.Add(agent);
        wc.KeepTrack(ac);
    }
}
