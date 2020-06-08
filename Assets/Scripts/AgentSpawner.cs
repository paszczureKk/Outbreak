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
    private int population = 30;

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
            Spawn(Vector3.zero);
        }
    }

    public AgentController Spawn(Vector3 pos, City city = null)
    {
        if(city == null)
        {
            city = wc.Town;
        }

        GameObject go = Instantiate(agent_template, pos, Quaternion.identity , city.gameObject.transform);
        AgentController ac = go.transform.GetComponent<AgentController>();
        ac.City = city;

        city.KeepTrack(ac);

        if(pos == Vector3.zero)
        {
            go.transform.position = ac.City.RandomPos;
        }

        return ac;
    }
}
