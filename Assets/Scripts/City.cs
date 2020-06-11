using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class City : MonoBehaviour
{
    public float Range { set; get; }
    public Vector2 Coords { set; get; }

    private static MovementController mc;

    private List<AgentController> agents;
    private List<AgentController> trash;
    private List<AgentController> created;

    private List<Vector3> works;
    private Vector3 school;
    private Vector3 shop;
    private Vector3 hospital;
    private Vector3 church;

    public AgentController RandomAgent
    {
        get
        {
            return agents[UnityEngine.Random.Range(0, agents.Count)];
        }
    }
    public int Agents
    {
        get
        {
            return agents.Count;
        }
    }


    public bool Access { get; set; }

    public void Awake()
    {
        works = new List<Vector3>();

        agents = new List<AgentController>();
        trash = new List<AgentController>();
        created = new List<AgentController>();
        
        Access = true;

        if (mc == null)
        {
            mc = MovementController.Instance;
        }

        Range = UnityEngine.Random.Range(0, Math.Min(mc.Bounds.x, mc.Bounds.y));
        this.gameObject.transform.localScale = new Vector3(Range * 0.1f, 1.0f, Range * 0.1f);

        works.Add(RandomPos);

        school = RandomPos;
        works.Add(school);

        hospital = RandomPos;
        works.Add(hospital);

        shop = RandomPos;
        works.Add(hospital);

        church = RandomPos;
    }

    public void Tick()
    {
        foreach (AgentController agent in agents)
        {
            agent.Tick();
        }
        foreach (AgentController item in trash)
        {
            agents.Remove(item);
        }
        foreach (AgentController item in created)
        {
            agents.Add(item);
        }
        trash.Clear();
        created.Clear();
    }

    public void KeepTrack(AgentController agent)
    {
        created.Add(agent);
    }

    public void DropTrack(AgentController agent)
    {
        trash.Add(agent);
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

    public Vector3 Work(int age)
    {
        if(age < 13)
        {
            return school;
        }
        else if(age < 50)
        {
            return works[UnityEngine.Random.Range(0, works.Count)];
        }
        else
        {
            return RandomPos;
        }
    }
}
