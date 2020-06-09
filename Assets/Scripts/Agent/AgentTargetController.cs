using UnityEngine;

public class AgentTargetController : MonoBehaviour
{
    private AgentController ac;
    public Vector3 Target { set; get; }
    private Vector3 work;

    private static float epsilon;

    public void Start()
    {
        epsilon = MovementController.Instance.Epsilon * 2;
        ac = this.gameObject.transform.GetComponent<AgentController>();
        PickWork();
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

    public void PickWork()
    {
        if(ac == null)
        {
            Debug.Log("yikes");
        }
        Target = work = ac.City.Work(ac.Age);
    }
}
