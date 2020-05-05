using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private enum Gender
    {
        Male,
        Female,
        None
    }

    private AgentMovementController amc = null;
    public void Awake()
    {
        amc = this.gameObject.GetComponent<AgentMovementController>();
    }

    private Gender gender = Gender.None;
    private bool illness = false;
    private int age = 1;
    public AgentVariables AgentVariables
    {
        set
        {
            amc.Velocity = new Vector2(value.x, value.y);
            gender = value.gender == 'M' ? Gender.Male : value.gender == 'F' ? Gender.Female : Gender.None;
            illness = value.illness;
            age = value.age;
        }
    }
}
