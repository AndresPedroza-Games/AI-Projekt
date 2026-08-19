using UnityEngine;

public class SoccerAgentRewards : MonoBehaviour
{
    [Header("---Components---")]
    private SoccerAgentController _agentController;
     

    [Header("---TrainingValues---")]
    public float _cumulativeReward;
    public float _currentEpisode;

    [Header("---Rewards---")]

    public static float goalReward = 10f;
    public static float autoGoalReward = -5f;
    public static float winReward = 15f;
    public static float loseReward = -10f;

    public static float distanceBallReward = 0.01f;
    public static float distanceGoalReward = 0.1f;

    public static float wallHitReward = -0.1f;
    public static float wallHitContinousReward = -0.001f;

    public static float touchBallReward = 0.05f;
    public static float notTouchBallReward = -0.1f;
    public static float notScoreReward = -0.1f;

    public static float touchObstacle = -0.2f;
    public static float dashObstacle = 0.05f;

    private void Awake()
    {
        _agentController = GetComponent<SoccerAgentController>();
    }

    private void Update()
    {
        _currentEpisode = _agentController.CompletedEpisodes;
        CheckDistanceToBall();
        CheckDistanceToGoal();

    }

    private void CheckDistanceToBall()
    {
        float currentDistance = Vector3.Distance(transform.position, _agentController.ball.transform.position);
        float progess = _agentController.previousDistanceToBall - currentDistance;

        _agentController.AddReward(progess * distanceBallReward * Time.deltaTime);
        _agentController.previousDistanceToBall = currentDistance;

        _cumulativeReward = _agentController.GetCumulativeReward();
    }

    private void CheckDistanceToGoal()
    {
        float currentDistance = Vector3.Distance(_agentController.ball.transform.position, _agentController.enemyGoal.transform.position);
        float progess = _agentController.previousDistanceToGoal - currentDistance;

        _agentController.AddReward(progess * distanceGoalReward * Time.deltaTime);
        _agentController.previousDistanceToGoal = currentDistance;

        _cumulativeReward = _agentController.GetCumulativeReward();
    }

    public void OnGoal()
    {
        _agentController.AddReward(goalReward);
        _cumulativeReward = _agentController.GetCumulativeReward();
        _agentController.EndEpisode();
    }

    public void OnGoalReceived()
    {
        _agentController.AddReward(autoGoalReward);
        _cumulativeReward = _agentController.GetCumulativeReward();
        _agentController.EndEpisode();
    }

    public void OnWinGame()
    {
        _agentController.AddReward(winReward);
        _cumulativeReward = _agentController.GetCumulativeReward();
        _agentController.EndEpisode();
        _currentEpisode = _agentController.CompletedEpisodes;

    }

    public void OnLoseGame()
    {
        _agentController.AddReward(loseReward);
        _cumulativeReward = _agentController.GetCumulativeReward();
        _agentController.EndEpisode();
        _currentEpisode = _agentController.CompletedEpisodes;
    }
}
