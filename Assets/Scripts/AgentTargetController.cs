using UnityEngine;

public class AgentTargetController : MonoBehaviour
{
    private static Vector2 bounds;
    private AgentController ac;
    public Vector3 Target { set; get; }
    private Vector3 work;

    private static float epsilon;
    private static float yOffset;


    public void Start()
    {
        epsilon = MovementController.Instance.Epsilon * 2;
        yOffset = MovementController.Instance.YOffset;
        bounds = MovementController.Instance.Bounds;
        ac = this.gameObject.transform.GetComponent<AgentController>();

        Target = work = PickTarget();
        this.enabled = false;
    }
    public void FixedUpdate()
    {
        if (Mathf.Abs(gameObject.transform.position.x - Target.x) + Mathf.Abs(gameObject.transform.position.z - Target.z) < epsilon)
        {
            Target = PickTarget();
        }
    }
    private Vector3 PickTarget()
    {
        return ac.City.RandomPos;
    }

    public void Work()
    {
        Target = work;
    }
}
