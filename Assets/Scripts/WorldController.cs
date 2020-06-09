using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
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

    private int timeframe = 0;

    public static int WorldTick { get; set; }
    public bool Day { get; set; }

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

        Day = true;
    }
    public void Start()
    {
        mc = MovementController.Instance;
        mc.Scale = MapScale;

        CitySpawner();
        AgentSpawner.Instance.Spawn();
    }

    public void FixedUpdate()
    {
        if(++timeframe == WorldTick)
        {
            Day = !Day;

            foreach (City city in cities)
            {
                city.Tick();
            }

            timeframe = 0;
        }
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
        /*
        float sizeX = mc.Bounds.x, sizeY = mc.Bounds.y;
        while (cities.Count < citiesNumber)
        {
            Vector2 coords = new Vector2(UnityEngine.Random.Range(0, sizeX - 1), UnityEngine.Random.Range(0, sizeY - 1));
            bool placeIsFree = true;
            foreach (City city in cities)
            {
                if (Math.Abs(coords.x - city.Coords.x) <= city.Range || Math.Abs(coords.y - city.Coords.y) <= city.Range)
                {
                    placeIsFree = false;
                    break;
                }
            }
            if (placeIsFree)
            {
                GameObject go = Instantiate(
                    city_template,
                    new Vector3(coords.x, city_template.transform.position.y, coords.y),
                    Quaternion.identity,
                    agentRealm.transform);

                City city = go.transform.GetComponent<City>();
                city.Coords = coords;

                Material material = new Material(material_template);
                material.color = UnityEngine.Random.ColorHSV();

                cities.Add(city);
            }
        }*/
    }
}
