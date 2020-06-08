using System;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public float Range { set; get; }
    public Vector2 Coords { set; get; }

    private static MovementController mc;

    private List<AgentController> agents;

    public bool Access { get; set; }

    public void Awake()
    {
        if(mc == null)
        {
            mc = MovementController.Instance;
        }
        Range = UnityEngine.Random.Range(1, Math.Min(mc.Bounds.x, mc.Bounds.y));
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

    public Vector3 RandomPos
    {
        get
        {
            Vector2 random = UnityEngine.Random.insideUnitCircle * Range;
            random += Coords;

            if (Mathf.Abs(random.x) > mc.Bounds.x)
            {
                random.x = (random.x < 0 ? -mc.Bounds.x : mc.Bounds.x);
            }
            if (Mathf.Abs(random.y) > mc.Bounds.y)
            {
                random.y = (random.y < 0 ? -mc.Bounds.y : mc.Bounds.y);
            }

            return new Vector3(random.x, mc.YOffset, random.y);
        }
    }
}
