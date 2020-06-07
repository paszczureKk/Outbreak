[System.Serializable]
public class AgentsWrapper
{
    public AgentVariables[] agents;
    public IllnessWrapper[] illness;

    public float Infectiousness
    {
        get
        {
            return illness[0].infectiousness;
        }
    }
    public float Fatality
    {
        get
        {
            return illness[0].fatality;
        }
    }
}
