using UnityEngine;
using System.Collections.Generic;

public class MovementController : MonoBehaviour
{
    public static MovementController Instance { get; set; }
    
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

        plane = Instantiate(plane, Vector3.zero, Quaternion.identity);
        plane.transform.localScale = new Vector3(scale, 1, scale);
    }

    [SerializeField]
    [Range(1, 10)]
    private float maxAcceleration = 5.0f;
    public float MaxAcceleration
    {
        get
        {
            return maxAcceleration;
        }
    }

    [SerializeField]
    [Range(1, 20)]
    private int worldScale = 5;
    private Vector3 m = Vector3.zero;

    private const float planeRatio = 0.1f;
    private const int worldUnitScale = 10;

    private int scale = 10;
    public int Scale
    {
        set
        {
            int temp = value * worldScale;
            if (scale < temp)
            {
                scale = temp;
                float planeScale = scale * planeRatio;
                plane.transform.localScale = new Vector3(planeScale, 1, planeScale);
                this.Bounds = new Vector2(Scale / 2.0f, Scale / 2.0f);
            }
        }
        get
        {
            return scale;
        }
    }
    public Vector2 Bounds { set; get; }

    [SerializeField]
    private GameObject plane;

    public Vector2 Velocity
    {
        set
        {
            float max = (value.x > value.y ? value.x : value.y);
            if (this.m.z < max)
            {
                this.m.z = max;
                this.Scale = Mathf.CeilToInt(max);
            }
        }
    }
}
