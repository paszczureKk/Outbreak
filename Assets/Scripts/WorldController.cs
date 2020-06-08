using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
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
        CitySpawner();
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
        float sizeX = MovementController.Instance.Bounds.x, sizeY = MovementController.Instance.Bounds.y;
        while (cities.Count < citiesNumber)
        {
            float x = UnityEngine.Random.Range(0, sizeX - 1), y = UnityEngine.Random.Range(0, sizeY - 1);
            bool placeIsFree = true;
            foreach (City city in cities)
            {
                if (Math.Abs(x - city.Coords.X) <= city.Range || Math.Abs(y - city.Coords.Y) <= city.Range)
                {
                    placeIsFree = false;
                    break;
                }
            }
            if (placeIsFree)
            {
                cities.Add(new City(x, y));
            }
        }
    }
}
