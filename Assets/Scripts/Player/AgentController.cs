using Unity.MLAgents;
using UnityEngine;

public class AgentController : Agent
{
    [Header("---Components---")]
    private AgentMovement _agentMovement;
    private AgentHealth _agentHealth;
    private AgentCombat _agentCombat;

    private new void Awake()
    {
        _agentMovement = GetComponent<AgentMovement>();
        _agentHealth = GetComponent<AgentHealth>();
        _agentCombat = GetComponent<AgentCombat>();
    }
}
