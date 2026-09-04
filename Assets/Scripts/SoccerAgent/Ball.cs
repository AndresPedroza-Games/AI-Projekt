using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("---Agents---")]
    [SerializeField] private SoccerAgentController _AI1;
    [SerializeField] private SoccerAgentController _AI2;

    [Header("---Goals---")]
    [SerializeField] private GameObject _AI1Goal;
    [SerializeField] private GameObject _AI2Goal;

    [SerializeField] private Transform _startPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _AI1Goal)
        {
            MatchManager.Instance.GoalAgent2();
            RestartPos();
        }

        if (other.gameObject == _AI2Goal)
        {
            MatchManager.Instance.GoalAgent1();
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
            _AI1.AddReward(SoccerAgentRewards.ballTouchWallReward);
            _AI2.AddReward(SoccerAgentRewards.ballTouchWallReward);
        }
    }
}
