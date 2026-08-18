using Unity.MLAgents.Actuators;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [Header("---Components---")]
    private Rigidbody _rb;

    [Header("---Movement Settings---")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("---Components---")]
    private AgentController _agentController;
    private float _timeOnWall;
    private Transform _startPoint;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _agentController = GetComponent<AgentController>();
        _startPoint = transform;
    }

    public void Movement(ActionBuffers actions)
    {
        float moveY = actions.ContinuousActions[0];
        float moveX = actions.ContinuousActions[1];

        Vector3 direction = new Vector3(-moveX, 0f, moveY);

        _rb.linearVelocity = direction * (_speed * 100f) * Time.deltaTime;
    }

    public void Rotate(ActionBuffers actions, Transform enemy)
    {
        float rotation = actions.ContinuousActions[2];

        transform.Rotate(0f, rotation * (_rotationSpeed * 100f) * Time.deltaTime ,0f);
    }

    private void PushBack(Collision collision)
    {
        Vector3 direction = transform.position - collision.transform.position;
        direction.y = 0f;
        direction.Normalize();

        _rb.AddForce(direction * 50f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable isDamageable))
            PushBack(collision);

        if (collision.gameObject.tag == "Wall")
        {
            _agentController.AddReward(-0.01f);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            _agentController.AddReward(-0.01f * Time.fixedDeltaTime);
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
            _agentController._cumulativeReward = _agentController.GetCumulativeReward();

            _agentController._cumulativeText.text = $"Cumulative Reward: {_agentController._cumulativeReward}";
            _agentController._episodeText.text = $"Current Episode: {_agentController._currentEpisode}";
        }
    }
} 
