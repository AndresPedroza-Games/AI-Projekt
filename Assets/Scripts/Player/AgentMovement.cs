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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
} 
