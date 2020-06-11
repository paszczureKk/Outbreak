using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; set; }
    private MovementController mc;

    private List<City> cities;

    [SerializeField]
    [Range(1,10)]
    private int citiesNumber = 5;

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
    [Range(10, 50)]
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
    [SerializeField]
    private Text TickCount;

    [SerializeField]
    private Text AgentStatus;


    private int timeframe = 0;

    public static int WorldTick { get; set; }

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
        cities = new List<City>();

        agentRealm = Instantiate<GameObject>(agentRealm);
    }
    public void Start()
    {
        mc = MovementController.Instance;
        mc.Scale = MapScale;

        CitySpawner();
        AgentSpawner.Instance.Spawn();
        Tick();
        cities[UnityEngine.Random.Range(0, cities.Count)].RandomAgent.Illness = true;
    }

    public void FixedUpdate()
    {
        if(++timeframe == WorldTick)
        {
            Tick();
        }
        LabelUpdate();
    }

    private int agents;

    private void Tick()
    {
        tickcount++;

        agents = 0;

        foreach (City city in cities)
        {
            city.Tick();
            agents += city.Agents;
        }

        timeframe = 0;
    }

    public int Sick { set; get; }

    private void LabelUpdate()
    {
        TickCount.text = "World Tick: " + tickcount.ToString();
        AgentStatus.text = "Agents sick: " + Sick.ToString() + "/" + agents.ToString();
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
}
