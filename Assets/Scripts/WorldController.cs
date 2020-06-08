using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; set; }

    private List<City> cities;

    [SerializeField]
    private int citiesNumber;

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

    public City Town
    {
        get
        {
            var temp = cities.Where(e => e.Access == true).ToList();
            return (temp.Count == 0 ? null : temp[UnityEngine.Random.Range(0, temp.Count)]);
        }
    }
}
