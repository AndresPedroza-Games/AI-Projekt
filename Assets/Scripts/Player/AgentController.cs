using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentController : Agent
{
    [Header("---Components---")]
    private AgentMovement _agentMovement;
    private AgentHealth _agentHealth;
    private AgentCombat _agentCombat;

    public GameObject enemy;

    public override void Initialize()
    {
        _agentMovement = GetComponent<AgentMovement>();
        _agentHealth = GetComponent<AgentHealth>();
        _agentCombat = GetComponent<AgentCombat>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 directionToEnemy = enemy.transform.position - transform.position;

        sensor.AddObservation(directionToEnemy.normalized);
        sensor.AddObservation(directionToEnemy.magnitude);

        sensor.AddObservation(_agentHealth.currentHealth);
        sensor.AddObservation(enemy.GetComponent<AgentHealth>().currentHealth);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        _agentMovement.Movement(actions);
        _agentMovement.Rotate(actions, enemy.transform);

        int attack = actions.DiscreteActions[0];

        if (attack == 1)
        {
            _agentCombat.Attack();
        }
    }

    public override void OnEpisodeBegin()
    {
        _agentHealth.ResetPlayer();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        var discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        continuousActions[2] = Input.GetAxisRaw("Mouse X");

        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;

    }


}
