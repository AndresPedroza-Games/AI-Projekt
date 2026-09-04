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

   

}
