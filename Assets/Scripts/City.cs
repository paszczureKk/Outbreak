using System;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public float Range { set; get; }
    public Vector2 Coords { set; get; }

    private static MovementController mc;

    private List<AgentController> agents;
    private List<AgentController> trash;

    private List<Vector3> works;
    private List<Vector3> schools;


    public bool Access { get; set; }

    public void Awake()
    {
        works = new List<Vector3>();
        schools = new List<Vector3>();
        agents = new List<AgentController>();
        trash = new List<AgentController>();
        Access = true;

        if (mc == null)
        {
            mc = MovementController.Instance;
        }

        Range = UnityEngine.Random.Range(0, Math.Min(mc.Bounds.x, mc.Bounds.y));
        this.gameObject.transform.localScale = new Vector3(Range * 0.1f, 1.0f, Range * 0.1f);

        for (int i = 0; i < Mathf.CeilToInt(Range); i++)
        {
            works.Add(RandomPos);
        }
        for (int i = 0; i < Mathf.Max(1, Mathf.CeilToInt(Range / 4)); i++)
        {
            schools.Add(RandomPos);
        }
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
        trash.Clear();
    }

    public void KeepTrack(AgentController agent)
    {
        agents.Add(agent);
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
            return schools[UnityEngine.Random.Range(0, schools.Count)];
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
