using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class SoccerAgentController : Agent
{
    [Header("---Components---")]
    private SoccerAgentMovement _agentMovement;
    private SoccerAgentScore _agentScore;

    [Header("---Goals---")]
    [SerializeField] private GameObject _teamGoal;
    public GameObject enemyGoal;
    public GameObject ball;

    [Header("---Obstacle---")]
    [SerializeField] private GameObject _obstacle;

    public Action onGoal;
    public Action onGoalReceived;

    public float previousDistanceToBall;
    public float previousDistanceToGoal;

    public override void Initialize()
    {
        _agentMovement = GetComponent<SoccerAgentMovement>();
        _agentScore = GetComponent<SoccerAgentScore>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 ballPos = ball.transform.position - transform.position;
        sensor.AddObservation(ballPos);

        Vector3 teamGoalPos = _teamGoal.transform.position - transform.position;
        sensor.AddObservation(teamGoalPos);

        Vector3 enemyGoalPos = enemyGoal.transform.position - transform.position;
        sensor.AddObservation(enemyGoalPos);

        Vector3 goalFromBall = enemyGoal.transform.position - ball.transform.position;
        sensor.AddObservation(goalFromBall);

        Vector3 playerFromObstacle = transform.position - _obstacle.transform.position;
        sensor.AddObservation(playerFromObstacle);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        _agentMovement.Movement(actions);
    }

    public override void OnEpisodeBegin()
    {
        _agentScore.Restart();
        previousDistanceToBall = Vector3.Distance(transform.position, ball.transform.position);
        previousDistanceToGoal = Vector3.Distance(ball.transform.position, enemyGoal.transform.position);
    }
}
