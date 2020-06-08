﻿using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; set; }

    private List<AgentController> agents;

    private int timeframe = 0;

    public static int WorldTick { get; set; }
    private int saveCount = 0;

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
        agents = new List<AgentController>();
    }
    public void FixedUpdate()
    {
        //Debug.Log(timeframe);
        if(++timeframe == WorldTick)
        {
            foreach (AgentController agent in agents)
            {
                agent.Freeze();
            }

            foreach (AgentController agent in agents)
            {
                agent.Tick();
            }


            foreach (AgentController agent in agents)
            {
                agent.Unfreeze();
            }

            timeframe = 0;
            Debug.Log(agents.Count);
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
