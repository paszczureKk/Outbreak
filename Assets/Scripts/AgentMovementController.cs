using UnityEngine;

public class AgentMovementController : MonoBehaviour
{
    private static MovementController movementController = null;
    private static Vector2 bounds = Vector2.zero;
    private static float maxSpeedChange = 0.0f;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    public Vector2 Velocity
    {
        set
        {
            velocity = new Vector3(value.x, 0.0f, value.y);
        }
    }

    public void Start()
    {
        if (movementController == null)
        {
            movementController = MovementController.Instance;
        }

        maxSpeedChange = movementController.MaxAcceleration * Time.deltaTime;
        bounds = movementController.Bounds;
        movementController.Velocity = velocity;
    }

    // Update is called once per frame
    public void Update()
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
        }
        if (OutrangeY >= 0.0f)
        {
            velocity.z = -velocity.z;
        }

        this.gameObject.transform.localPosition = newPosition;
    }
}
