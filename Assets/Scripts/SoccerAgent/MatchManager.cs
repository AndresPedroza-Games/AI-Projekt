using TMPro;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [Header("Agents")]
    [SerializeField] private SoccerAgentController _agent1;
    [SerializeField] private SoccerAgentController _agent2;

    [Header("Match")]
    [SerializeField] private int _maxGoals = 3;

    [Header("Rewards")]
    [SerializeField] private float _goalReward = 20f;
    [SerializeField] private float _goalRecievedReward = -20f;
    [SerializeField] private float _winReward = 30f;
    [SerializeField] private float _loseReward = -30f;

    [SerializeField] private TMP_Text _AI1ScoreUI;
    [SerializeField] private TMP_Text _AI2ScoreUI;

    public int scoreAgent1;
    public int scoreAgent2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void GoalAgent1()
    {
        scoreAgent1++;

        _agent1.AddReward(_goalReward);
        _agent2.AddReward(_goalRecievedReward);

        _agent1.EndEpisode();
        _agent2.EndEpisode();

        CheckMatchEnd();
    }

    public void GoalAgent2()
    {
        scoreAgent2++;

        _agent2.AddReward(_goalReward);
        _agent1.AddReward(_goalRecievedReward);

        _agent1.EndEpisode();
        _agent2.EndEpisode();

        CheckMatchEnd();
    }

    private void CheckMatchEnd()
    {
        if (scoreAgent1 >= _maxGoals)
             EndMatch(_agent1, _agent2);
        else if (scoreAgent2 >= _maxGoals)
            EndMatch(_agent2, _agent1);
        else
            ResetAfterGoal();

    }

    private void EndMatch(SoccerAgentController winner, SoccerAgentController loser)
    {
        winner.AddReward(_winReward);
        loser.AddReward(_loseReward);

        winner.EndEpisode();
        loser.EndEpisode();

        ResetScore();
        ResetAfterGoal();
    }

    private void ResetAfterGoal()
    {
        _agent1.gameObject.transform.position = _agent1.agentMovement.startPoint.position;
        _agent2.gameObject.transform.position = _agent2.agentMovement.startPoint.position;
    }

    public void ResetScore()
    {
        scoreAgent1 = 0;
        scoreAgent2 = 0;
    }

    private void UpdateUI()
    {
        _AI1ScoreUI.text = $"AI 1 goals: {scoreAgent1}";
        _AI2ScoreUI.text = $"AI 2 goals: {scoreAgent2}";
    }
}
