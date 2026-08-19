using Unity.MLAgents.Actuators;
using UnityEngine;

public class SoccerAgentMovement : MonoBehaviour
{
    [Header("---Components---")]
    private Rigidbody _rb;
    private SoccerAgentRewards _agentRewards;

    [Header("---Movement Settings---")]
    [SerializeField] private float _speed = 10f;

    [Header("---Components---")]
    private SoccerAgentController _agentController;
    private SoccerAgentScore _agentScore;
    private float _timeOnWall;
    [SerializeField] private Transform _startPoint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _agentController = GetComponent<SoccerAgentController>();
        _agentScore = GetComponent<SoccerAgentScore>();
        _agentRewards = GetComponent<SoccerAgentRewards>();
    }

    public void Movement(ActionBuffers actions)
    {
        float moveY = actions.ContinuousActions[0];
        float moveX = actions.ContinuousActions[1];

        Vector3 direction = new Vector3(moveX, 0f, moveY);

        _rb.linearVelocity = direction * (_speed * 100f) * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            _agentController.AddReward(SoccerAgentRewards.wallHitReward);
            GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            _agentController.AddReward((SoccerAgentRewards.wallHitContinousReward * Time.fixedDeltaTime));
            _timeOnWall += Time.deltaTime;
        }

        if (_timeOnWall >= 1)
        {
            transform.position = _startPoint.position;
            _timeOnWall = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            _agentRewards._cumulativeReward = _agentController.GetCumulativeReward();
            GetComponentInChildren<Renderer>().material.color = Color.gray;
        }
    }
}
