using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("---Agents---")]
    [SerializeField] private SoccerAgentController _agentController;

    [Header("---Goals---")]
    [SerializeField] private GameObject _AIGoal;
    [SerializeField] private GameObject _playerGoal;

    [SerializeField] private Transform _startPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _AIGoal)
        {
            _agentController.onGoalReceived.Invoke();
            RestartPos();
        }

        if (other.gameObject == _playerGoal)
        {
            _agentController.onGoal.Invoke();
            RestartPos();
        }
    }

    public void RestartPos()
    {
        transform.position = _startPoint.position;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            RestartPos();
            _agentController.AddReward(SoccerAgentRewards.ballTouchWallReward);
        }
    }
}
