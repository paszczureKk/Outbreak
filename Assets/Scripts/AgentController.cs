using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private AgentMovementController amc = null;
    public void Awake()
    {
        amc = this.gameObject.GetComponent<AgentMovementController>();
    }
}
