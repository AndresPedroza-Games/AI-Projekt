using UnityEngine;

public class SoccerAgentRewards : MonoBehaviour
{
    [Header("---Components---")]
    private SoccerAgentController _agentController;

    [Header("---TrainingValues---")]
    public float _cumulativeReward;
    public float _currentEpisode;

    [Header("---Rewards---")]

    //public static float distanceBallReward = 0.05f;
    //public static float distanceGoalReward = 0.1f;
    //public static float touchBallReward = 0.1f;

    public static float distanceBallReward = 0.1f;
    public static float distanceGoalReward = 0.3f;
    public static float touchBallReward = 0.5f;

    public static float wallHitReward = -0.1f;
    public static float wallHitContinousReward = -0.001f;

    public static float timePenalty = -0.001f;
    public static float ballTouchWallReward = -0.01f;

    public static float shootAwayReward = -0.001f;

    public bool hasTouchedBall;

    private float _stuckTimer;


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

        CheckBallStuck();

        _agentController.AddReward(timePenalty);
    }


    private void CheckBallStuck()
    {
        float ballSpeed = _agentController.ball.GetComponent<Rigidbody>().linearVelocity.magnitude;

        if (ballSpeed < 0.2f)
        {
            _stuckTimer += Time.fixedDeltaTime;

            if (_stuckTimer >= 2f)
            {
                _agentController.AddReward(-0.01f);
                _stuckTimer = 0f;
            }
        }
        else
            _stuckTimer = 0f;
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
