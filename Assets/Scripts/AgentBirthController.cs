﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AgentBirthController : MonoBehaviour
{
    private static AgentSpawner agentSpawner;
    private AgentController ac;

    private static int birthCooldown = 1;
    private int lastBirth = 0;
    private int timeFrame = 0;
    private int Age { get; set; }

    private float DeathProbability
    {
        get
        {
            return Age * 0.005f + 0.02f + (ac.Illness == true ? IllnessController.Fatality : 0.0f);
        }
    }

    public void Awake()
    {
        this.Age = 0;
    }

    public void Start()
    {
        agentSpawner = AgentSpawner.Instance;
        ac = this.gameObject.GetComponent<AgentController>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if(++timeFrame == WorldController.WorldTick)
        {
            Age++;
            lastBirth--;

            if (UnityEngine.Random.value < DeathProbability)
            {
                Destroy(gameObject);
                Debug.Log("BLAH!");
            }
        }
    }

    public void Birth(AgentController other)
    {
        if(Age < 13 || Age >= 50)
        {
            return;
        }

        float probability;
        if(Age < 20 && Age >= 13)
        {
            probability = 0.85f;
        }
        else if(Age < 30 && Age >= 20)
        {
            probability = 0.69f;
        }
        else if(Age < 40 && Age >= 30)
        {
            probability = 0.31f;
        }
        else
        {
            probability = 0.12f;
        }

        if(UnityEngine.Random.value < probability && lastBirth == 0)
        {
            AgentVariables av = new AgentVariables();
            AgentVariables otherVariables = other.AgentVariables;
            av.x = (ac.AgentVariables.x + otherVariables.x) / 2.0f;
            av.y = (ac.AgentVariables.y + otherVariables.y) / 2.0f;
            av.gender = UnityEngine.Random.value < 0.5f ? "M" : "F";
            av.illness = (UnityEngine.Random.value < IllnessController.Infectiousness ? true : false);

            agentSpawner.Spawn(this.gameObject.transform, av);

            lastBirth += birthCooldown;
        }
    }
}