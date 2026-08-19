using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    [SerializeField] private SoccerAgentController _agentController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            _agentController.AddReward(SoccerAgentRewards.dashObstacle);
    }
}
