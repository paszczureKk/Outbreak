using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VariablesController : MonoBehaviour
{
    public static VariablesController Instance { get; set; }
    private IllnessController ic = null;

    [SerializeField]
    private GameObject gameEngine;

    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject supportPanel;

    [SerializeField]
    private Slider infectiousnessSlider;
    [SerializeField]
    private Slider fatalitySlider;
    [SerializeField]
    private Slider illnessSlider;
    [SerializeField]
    private Slider recoverySlider;
    [SerializeField]
    private Slider mutationSlider;
    [SerializeField]
    private Slider i_infectiousnessSlider;
    [SerializeField]
    private Slider i_fatalitySlider;
    [SerializeField]
    private Slider i_illnessSlider;
    [SerializeField]
    private Slider i_recoverySlider;
    [SerializeField]
    private Slider i_mutationSlider;

    [SerializeField]
    private Slider worldSlider;
    [SerializeField]
    private Slider citiesSlider;
    [SerializeField]
    private Slider rangeSlider;
    [SerializeField]
    private Slider agentsSlider;

    [SerializeField]
    private Toggle toggle;


    //Illness Controller
    public float Infectiousness { get; set; }
    public float Fatality { get; set; }
    public float IllnessProbability { get; set; }
    public float Recovery { get; set; }
    public float MutationProbability { get; set; }
    public bool Random { get; set; }

    //World Controller
    public int WorldTick { get; set; }
    public float WorldTickFloat
    {
        get
        {
            return (float)WorldTick;
        }
        set
        {
            WorldTick = (int)value;
        }
    }
    public int CitiesNumber { get; set; }
    public float CitiesNumberFloat
    {
        get
        {
            return (float)CitiesNumber;
        }
        set
        {
            CitiesNumber = (int)value;
        }
    }
    public int CitiesRange { get; set; }
    public float CitiesRangeFloat
    {
        get
        {
            return (float)CitiesRange;
        }
        set
        {
            CitiesRange = (int)value;
        }
    }
    public int AgentsNumber { get; set; }
    public float AgentsNumberFloat
    {
        get
        {
            return (float)AgentsNumber;
        }
        set
        {
            AgentsNumber = (int)value;
        }
    }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Run()
    {
        mainPanel.SetActive(false);
        supportPanel.SetActive(true);

        Infectiousness = i_infectiousnessSlider.value;
        Fatality = i_fatalitySlider.value;
        IllnessProbability = i_illnessSlider.value;
        Recovery = i_recoverySlider.value;
        MutationProbability = i_mutationSlider.value;

        infectiousnessSlider.value = Infectiousness;
        fatalitySlider.value = Fatality;
        illnessSlider.value = IllnessProbability;
        recoverySlider.value = Recovery;
        mutationSlider.value = MutationProbability;

        WorldTickFloat = worldSlider.value;
        CitiesNumberFloat = citiesSlider.value;
        CitiesRangeFloat = rangeSlider.value;
        AgentsNumberFloat = agentsSlider.value;

        Random = toggle.isOn;

        gameEngine = Instantiate(gameEngine);
    }

    private bool direction = false;
    [SerializeField]
    private Button moveButton;
    public void MoveSupportPanel()
    {
        moveButton.interactable = false;

        Vector3 destination = supportPanel.transform.localPosition;
        destination.x += (direction == true ? -1 : 1) * (supportPanel.GetComponent<RectTransform>().rect.width - moveButton.GetComponent<RectTransform>().rect.width);

        IEnumerator coroutine = SmoothMove(supportPanel.transform, destination, 1.0f);

        StartCoroutine(coroutine);

        direction = !direction;
        moveButton.interactable = true;
    }

    private IEnumerator SmoothMove(Transform transform, Vector3 destination, float time)
    {
        Vector3 startingPos = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.localPosition = Vector3.Lerp(startingPos, destination, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = destination;
    }
}
