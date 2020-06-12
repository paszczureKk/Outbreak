using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public static AgentSpawner Instance { get; set; }

    [SerializeField]
    private GameObject agent_template;

    private int spawnerCounter = 0;

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
    }

    public void Spawn()
    {
        wc = WorldController.Instance;
        for (int i = 0; i < VariablesController.Instance.AgentsNumber; i++)
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

        GameObject go = Instantiate(agent_template, pos, Quaternion.identity, wc.AgentRealm);
        go.name = "Agent" + spawnerCounter++.ToString();
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
