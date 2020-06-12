using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float Epsilon = 0.1f;
    public float YOffset = 1.5f;
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
    private float speed = 5.0f;
    public float Speed
    {
        get
        {
            return speed;
        }
    }

    [SerializeField]
    private float travelProbability = 0.10f;
    public float TravelProbability
    {
        get
        {
            return travelProbability;
        }
        set
        {
            travelProbability = value;
        }
    }

    private Vector3 m = Vector3.zero;

    private const float planeRatio = 0.1f;

    private int scale = 0;
    public int Scale
    {
        set
        {
            if (scale < value)
            {
                scale = value;
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
}
