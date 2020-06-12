using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class City : MonoBehaviour
{
    public static float MaxRange;
    public float Range { set; get; }
    public Vector2 Coords { set; get; }

    private static MovementController mc;
    private static ResearchController rc;

    private List<AgentController> agents;
    private List<AgentController> trash;
    private List<AgentController> created;

    private List<Vector3> works;
    public Vector3 School;
    public Vector3 Shop;
    public Vector3 Hospital;
    public Vector3 Church;

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
    public int SickAgents
    {
        get
        {
            if (Agents == 0)
            {
                return 0;
            }

            int count = agents.Where(e => e.Illness == true).ToList().Count;

            if (Access == true)
            {
                if (count / Agents > rc.AlertLevel)
                {
                    rc.Alert(this);
                }
            }
            else
            {
                if (count / Agents < rc.CuredLevel)
                {
                    rc.Cured(this);
                }
            }

            return count;
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
        if (rc == null)
        {
            rc = ResearchController.Instance;
        }

        Range = UnityEngine.Random.Range(0, MaxRange);
        this.gameObject.transform.localScale = new Vector3(Range * 0.1f, 1.0f, Range * 0.1f);

        works.Add(RandomPos);

        School = RandomPos;
        works.Add(School);

        Hospital = RandomPos;
        works.Add(Hospital);

        Shop = RandomPos;
        works.Add(Shop);

        Church = RandomPos;
    }

    public void Tick()
    {
        agents = agents.Where(e => e != null).ToList();
        foreach (AgentController agent in agents)
        {
            agent.Tick();
        }
        for (int i = 0; i < trash.Count; i++)
        {
            if (trash[i] == null)
                continue;
            Destroy(trash[i].gameObject);
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
            return School;
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
