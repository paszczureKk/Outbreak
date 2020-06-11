using UnityEngine;

public class AgentTargetController : MonoBehaviour
{
    private AgentController ac;
    public Vector3 Target { set; get; }

    private static float epsilon;

    public void Start()
    {
        epsilon = MovementController.Instance.Epsilon * 2;
        ac = this.gameObject.transform.GetComponent<AgentController>();
    }
    public void FixedUpdate()
    {
        if (Target == Vector3.zero || Mathf.Abs(gameObject.transform.position.x - Target.x) + Mathf.Abs(gameObject.transform.position.z - Target.z) < epsilon)
        {
            Target = PickTarget();
        }
    }
    private Vector3 PickTarget()
    {
        return ac.City.RandomPos;
    }
}
