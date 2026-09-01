
using TMPro;
using UnityEngine;

public class SoccerAgentUI : MonoBehaviour
{
    [Header("---Components---")]
    private SoccerAgentScore _agentScore;
    private SoccerAgentRewards _agentRewards;

    [Header("---UI---")]
    [SerializeField] private TMP_Text _playerScoreUI;
    [SerializeField] private TMP_Text _AIScoreUI;
    [SerializeField] private TMP_Text _cumulativeRewardUI;
    [SerializeField] private TMP_Text _currentEpisodeUI;

    private void Awake()
    {
        _agentScore = GetComponent<SoccerAgentScore>();
        _agentRewards = GetComponent<SoccerAgentRewards>();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _playerScoreUI.text = $"Player goals: {_agentScore._enemyScore}";
        _AIScoreUI.text = $"AI goals: {_agentScore._teamScore}";

        _cumulativeRewardUI.text = $"Cumulative Reward: {_agentRewards._cumulativeReward}";
        _currentEpisodeUI.text = $"Episode: {_agentRewards._currentEpisode}";
    }
}
