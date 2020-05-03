using UnityEngine;
using System.Collections.Generic;

public class MovementController : MonoBehaviour
{
    private static MovementController instance = null;
    public static MovementController Instance
    {
        get
        {
            return instance;
        }
    }
    
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        plane = Instantiate(plane_template, Vector3.zero, Quaternion.identity);
        plane.transform.localScale = new Vector3(scale, 1, scale);
    }

    [SerializeField]
    [Range(5, 50)]
    private int tempo = 10;
    private Vector2 m = Vector2.zero;

    private int scale = 10;
    public int Scale
    {
        set
        {
            int temp = value * tempo;
            if (scale < temp)
            {
                scale = temp;
            }
        }
        get
        {
            return scale;
        }
    }
    private Vector2 bounds;
    public Vector2 Bounds
    {
        set
        {
            bounds = new Vector2(Scale - value.x, Scale - value.y);
        }
        get
        {
            return bounds;
        }
    }

    [SerializeField]
    private GameObject plane_template;
    private GameObject plane;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float maxAcceleration;
    public float MaxAcceleration
    {
        get
        {
            return maxAcceleration;
        }
    }

    private Queue<Vector2> velocities;
    public Vector2 Velocity
    {
        set
        {
            if (this.m.x < value.x)
            {
                this.m = new Vector2(value.x, m.y);
                this.Bounds = new Vector2(value.x, this.Bounds.y);
                this.Scale = (int)value.x;
            }
            if (this.m.y < value.y)
            {
                this.m = new Vector2(m.x, value.y);
                this.Bounds = new Vector2(this.Bounds.x, value.y);
                this.Scale = (int)value.y;
            }
            velocities.Enqueue(value);
        }
        get
        {
            return velocities.Dequeue();
        }
    }
}
