using UnityEngine;

public class AgentMovementController : MonoBehaviour
{
    private static MovementController movementController = null;
    private static Vector2 bounds = Vector2.zero;
    private static float maxSpeedChange = 0.0f;

    private Rigidbody rb;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    public Vector2 Velocity
    {
        set
        {
            velocity = new Vector3(value.x, 0.0f, value.y);
        }
        get
        {
            return new Vector2(velocity.x, velocity.z);
        }
    }

    public void Start()
    {
        if (movementController == null)
        {
            movementController = MovementController.Instance;
            maxSpeedChange = movementController.MaxAcceleration * Time.deltaTime;
        }
        movementController.Velocity = new Vector2(velocity.x, velocity.z);
        bounds = movementController.Bounds;

        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        currentVelocity.x =
            Mathf.MoveTowards(currentVelocity.x, velocity.x, maxSpeedChange);
        currentVelocity.z =
            Mathf.MoveTowards(currentVelocity.z, velocity.z, maxSpeedChange);

        Vector3 displacement = currentVelocity * Time.deltaTime;

        Vector3 newPosition = transform.localPosition + displacement;

        float OutrangeX = Mathf.Abs(newPosition.x) - bounds.x;
        float OutrangeY = Mathf.Abs(newPosition.z) - bounds.y;

        if (OutrangeX >= 0.0f)
        {
            velocity.x = -velocity.x;
            currentVelocity.x = -currentVelocity.x;
            displacement.x = -displacement.x;
        }
        if (OutrangeY >= 0.0f)
        {
            velocity.z = -velocity.z;
            currentVelocity.z = -currentVelocity.z;
            displacement.z = -displacement.z;
        }

        newPosition = transform.localPosition + displacement;
        newPosition.y = -1.5f;

        this.gameObject.transform.localPosition = newPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
