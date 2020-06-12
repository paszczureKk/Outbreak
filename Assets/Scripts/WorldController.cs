using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; set; }
    private MovementController mc;
    private ResearchController rc;
    private IllnessController ic;

    private List<City> cities;
    private List<GameObject> trash;

    private int worldTick;
    public int WorldTick
    {
        set
        {
            worldTick = value;
        }
        get
        {
            return worldTick;
        }
    }

    private int citiesNumber;
    private int cityRange;

    [SerializeField]
    private GameObject city_template;

    [SerializeField]
    private GameObject agentRealm;
    public Transform AgentRealm
    {
        get
        {
            return agentRealm.transform;
        }
    }

    [SerializeField]
    private Material material_template;

    [SerializeField]
    private int mapScale = 30;
    public int MapScale
    {
        set
        {
            mapScale = value;
        }
        get
        {
            return mapScale;
        }
    }

    private int tickcount = 0;

    public Text TickCount { get; set; }

    public Text AgentStatus { get; set; }

    public Text CureStatus { get; set; }


    private int timeframe = 0;

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

        VariablesController vc = VariablesController.Instance;
        worldTick = vc.WorldTick;
        citiesNumber = vc.CitiesNumber;
        cityRange = vc.CitiesRange;

        GameObject canvas = vc.gameObject;
        TickCount = canvas.transform.Find("WorldTickText").transform.GetComponent<Text>();
        AgentStatus = canvas.transform.Find("AgentText").transform.GetComponent<Text>();
        CureStatus = canvas.transform.Find("CureText").transform.GetComponent<Text>();

        City.MaxRange = cityRange;
        cities = new List<City>();
        trash = new List<GameObject>();

        agentRealm = Instantiate<GameObject>(agentRealm);
    }
    public void Start()
    {
        mc = MovementController.Instance;
        ic = IllnessController.Instance;
        rc = ResearchController.Instance;
        mc.Scale = MapScale;

        CitySpawner();
        AgentSpawner.Instance.Spawn();
        Tick();

        List<City> temp = cities.Where(e => e.Agents > 0).ToList();
        temp[UnityEngine.Random.Range(0, temp.Count)].RandomAgent.Illness = true;
    }

    public void FixedUpdate()
    {
        if(++timeframe == WorldTick)
        {
            Tick();
        }
        LabelUpdate();
    }

    private void Tick()
    {
        tickcount++;

        foreach (City city in cities)
        {
            city.Tick();
        }

        if(rc.Aware == true)
        {
            if(UnityEngine.Random.value > rc.AlertLevel)
            {
                rc.Research();
            }
        }

        if(UnityEngine.Random.value < ic.MutationProbability)
        {
            rc.Drop();
        }

        timeframe = 0;
    }

    private void LabelUpdate()
    {
        int agents = 0;
        int sickagents = 0;

        foreach (City city in cities)
        {
            agents += city.Agents;
            sickagents += city.SickAgents;
        }

        TickCount.text = "World Tick: " + tickcount.ToString();
        AgentStatus.text = "Agents sick: " + sickagents.ToString() + "/" + agents.ToString();
        CureStatus.text = "Cure status: " + ((int)(rc.Cure * 100)).ToString() + "%";
    }

    public City TownTravel(City city)
    {
        var temp = cities.Where(e => e.Access == true && !e.Equals(city)).ToList();
        return (temp.Count == 0 ? null : temp[UnityEngine.Random.Range(0, temp.Count)]);
    }
    public City Town
    {
        get
        {
            return cities[UnityEngine.Random.Range(0, cities.Count)];
        }
    }

    public void CitySpawner()
    {
        for (int i = 0; i < citiesNumber; i++)
        {
            GameObject go = Instantiate(city_template, agentRealm.transform);
            City city = go.transform.GetComponent<City>();

            Vector2 coords =
                new Vector2(
                    UnityEngine.Random.value * MapScale * 0.5f * (UnityEngine.Random.value < 0.5f ? -1 : 1),
                    UnityEngine.Random.value * MapScale * 0.5f * (UnityEngine.Random.value < 0.5f ? -1 : 1)
                );
            go.transform.position = new Vector3(coords.x, city_template.transform.position.y, coords.y);

            city.Coords = coords;

            Material material = new Material(material_template);
            material.color = UnityEngine.Random.ColorHSV();
            go.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;

            cities.Add(city);
        }
    }

    public void Clear(GameObject go)
    {
        trash.Add(go);
    }
}
