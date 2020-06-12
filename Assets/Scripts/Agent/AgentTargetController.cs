using UnityEngine;

public class AgentTargetController : MonoBehaviour
{
    private static ResearchController rc;
    private AgentController ac;
    public Vector3 Target { set; get; }
    private Vector3 work;

    private static float epsilon;
    private bool hospital = false;

    public void Awake()
    {
        if (rc == null)
        {
            rc = ResearchController.Instance;
        }
    }

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
            if (hospital == true)
            {
                hospital = false;
                if(UnityEngine.Random.value < rc.CureProbablity)
                {
                    ac.Illness = false;
                }
            }
        }
    }
    private Vector3 PickTarget()
    {
        float rand = UnityEngine.Random.value;

        if(rand < 0.30f)
        {
            if (work == null)
            {
                PickWork();
            }
            return work;
        }
        else if (rand < 0.45f)
        {
            return ac.City.Shop;
        }
        else if (rand < 0.50f)
        {
            return ac.City.Church;
        }
        else
        {
            if(ac.Illness == true)
            {
                if (UnityEngine.Random.value < 0.20f)
                {
                    hospital = true;
                    return ac.City.Hospital;
                }
            }

            return ac.City.RandomPos;
        }
    }

    public void PickWork()
    {
        if(ac == null || ac.City == null)
        {
            return;
        }
        work = ac.City.Work(ac.Age);
    }
}
