using UnityEngine;

public class SoccerAgentRewards : MonoBehaviour
{
    [Header("---Components---")]
    private SoccerAgentController _agentController;

    [Header("---TrainingValues---")]
    public float _cumulativeReward;
    public float _currentEpisode;

    [Header("---Rewards---")]

    public static float goalReward = 15f;
    public static float autoGoalReward = -10f;
    public static float winReward = 20f;
    public static float loseReward = -15f;

    //public static float distanceBallReward = 0.05f;
    //public static float distanceGoalReward = 0.1f;
    //public static float touchBallReward = 0.1f;

    public static float distanceBallReward = 0.1f;
    public static float distanceGoalReward = 0.3f;
    public static float touchBallReward = 0.5f;

    public static float wallHitReward = -0.1f;
    public static float wallHitContinousReward = -0.001f;

    public static float timePenalty = -0.0005f;
    public static float ballTouchWallReward = -0.1f;

    public static float touchObstacle = -0.2f;
    public static float dashObstacle = 0.005f;

    public static float shootAwayReward = -0.001f;

    public bool hasTouchedBall;

    private void Awake()
    {
        _agentController = GetComponent<SoccerAgentController>();
    }

    private void Update()
    {
        _currentEpisode = _agentController.CompletedEpisodes;
    }

    public void CalculateRewards()
    {
        float distanceToBall = Vector3.Distance(transform.position, _agentController.ball.transform.position);

        bool isNearBall = distanceToBall <= 1f;

        CheckDistanceToBall(!isNearBall);
        CheckDistanceToGoal(isNearBall);

        _agentController.AddReward(timePenalty);
    }

    private void CheckDistanceToBall(bool giveReward)
    {
        float currentDistance = Vector3.Distance(transform.position, _agentController.ball.transform.position);

        float progress = _agentController.previousDistanceToBall - currentDistance;

        if (giveReward)
            _agentController.AddReward(progress * distanceBallReward);

        _agentController.previousDistanceToBall = currentDistance;
    }

    private void CheckDistanceToGoal(bool giveReward)
    {
        float currentDistance = Vector3.Distance(_agentController.ball.transform.position,_agentController.enemyGoal.transform.position);

        float progress = _agentController.previousDistanceToGoal - currentDistance;

        if (giveReward)
            _agentController.AddReward(progress * distanceGoalReward);

        _agentController.previousDistanceToGoal = currentDistance;
    }

    public void OnGoal()
    {
        float distanceToGoal = Vector3.Distance(transform.position, _agentController.enemyGoal.transform.position);

        float distanceBonus = Mathf.Clamp(distanceToGoal, 0f, 10f);

        _agentController.AddReward(goalReward + distanceBonus);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball ball) && !hasTouchedBall)
        {
            hasTouchedBall = true;
            _agentController.AddReward(touchBallReward);
            _cumulativeReward = _agentController.GetCumulativeReward();
        }
    }
}
