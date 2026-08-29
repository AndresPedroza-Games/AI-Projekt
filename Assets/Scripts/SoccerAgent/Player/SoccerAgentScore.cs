using UnityEngine;

public class SoccerAgentScore : MonoBehaviour
{
    [Header("---Components---")]
    private SoccerAgentRewards _agentRewards;
    private SoccerAgentController _agentController;
    [SerializeField] private Transform _startPoint;

    [Header("---Score---")]
    public int _teamScore = 0;
    public int _enemyScore = 0;

    [Header("---Goals---")]
    [SerializeField] private GameObject _teamGoal;
    [SerializeField] private GameObject _enemyGoal;

    private void Awake()
    {
        _agentRewards = GetComponent<SoccerAgentRewards>();
        _agentController = GetComponent<SoccerAgentController>();

         _teamScore = 0;
        _enemyScore = 0;

    }

    private void OnEnable()
    {
        _agentController.onGoal += OnGoal;
        _agentController.onGoalReceived += OnGoalReceived;
    }

    private void OnDisable()
    {
        _agentController.onGoal -= OnGoal;
        _agentController.onGoalReceived -= OnGoalReceived;
    }

    private void Update()
    {
        if (_teamScore >= 3)
            OnWinGame();
        else if (_enemyScore >= 3)
            OnLoseGame();
    }


    public void Restart()
    {
        _teamScore = 0;
        _enemyScore = 0;
        _agentRewards._cumulativeReward = 0;
        _agentRewards._currentEpisode = 0;
    }

    private void OnWinGame()
    {
        _agentRewards.OnWinGame();
        Restart();
    }

    private void OnLoseGame()
    {
        _agentRewards.OnLoseGame();
        Restart();
    }

    private void OnGoal()
    {
        _agentRewards.OnGoal();
        _teamScore++;
        transform.position = _startPoint.position;
    }

    private void OnGoalReceived()
    {
        _agentRewards.OnGoalReceived();
        _enemyScore++;
        transform.position = _startPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _teamGoal || other.gameObject == _enemyGoal)
        {
            transform.position = _startPoint.position;
        }
    }

}
