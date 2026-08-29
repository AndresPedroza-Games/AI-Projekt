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
    public SoccerAgentActions _agentActions;
    private SoccerAgentRewards _agentRewards;

    [Header("---Goals---")]
    [SerializeField] private GameObject _teamGoal;
    [SerializeField] private GameObject _goalKeaper;
    public GameObject enemyGoal;
    public GameObject ball;

    public Action onGoal;
    public Action onGoalReceived;

    public float previousDistanceToBall;
    public float previousDistanceToGoal;

    public override void Initialize()
    {
        _agentMovement = GetComponent<SoccerAgentMovement>();
        _agentScore = GetComponent<SoccerAgentScore>();
        _agentActions = GetComponent<SoccerAgentActions>();
        _agentRewards = GetComponent<SoccerAgentRewards>();

        _agentMovement.Init();
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
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        float moveY = actions.ContinuousActions[0];
        float moveX = actions.ContinuousActions[1];

        int sprint = actions.DiscreteActions[0];

        bool wantsToSprint = sprint == 1;

        _agentMovement.Sprint(wantsToSprint);

        _agentMovement.Movement(moveX, moveY);

        int shoot = actions.DiscreteActions[1];
        float forceInput = actions.ContinuousActions[2];

        bool wantsToShoot = shoot == 1;

        _agentActions.Shoot(wantsToShoot, forceInput);

        _agentRewards.CalculateRewards();
    }

    public override void OnEpisodeBegin()
    {
        _agentScore.Restart();

        previousDistanceToBall = Vector3.Distance(transform.position, ball.transform.position);
        previousDistanceToGoal = Vector3.Distance(ball.transform.position, enemyGoal.transform.position);

        _agentMovement.ResetStamina();
        _agentRewards.hasTouchedBall = false;
    }
}
