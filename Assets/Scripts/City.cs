using System;
using System.Collections.Generic;
using System.Numerics;

public class City
{
    public float Range { set; get; }
    public Vector2 Coords { set; get; }

    private List<AgentController> agents;

    public bool Access { get; set; }

    public City()
    {
        Range = UnityEngine.Random.Range(1, Math.Min(MovementController.Instance.Bounds.x, MovementController.Instance.Bounds.y));
    }
    public City(float x, float y)
    {
        Coords = new Vector2(x, y);
        Range = UnityEngine.Random.Range(1, Math.Min(MovementController.Instance.Bounds.x, MovementController.Instance.Bounds.y));
    }
    public void Tick()
    {
        foreach (AgentController agent in agents)
        {
            agent.Tick();
        }
    }

    public void KeepTrack(AgentController agent)
    {
        agents.Add(agent);
    }

    public void DropTrack(AgentController agent)
    {
        agents.Remove(agent);
    }
}
