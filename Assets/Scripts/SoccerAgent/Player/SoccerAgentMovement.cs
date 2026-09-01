using UnityEngine;

public class SoccerAgentMovement : MonoBehaviour
{
    [Header("---Components---")]
    private Rigidbody _rb;
    private SoccerAgentRewards _agentRewards;

    [Header("---Movement Settings---")]
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _normalSpeed = 1f;
    [SerializeField] private float _sprintSpeed = 3f;

    public float maxStamina = 3f;
    public float currentStamina;

    public bool isSprinting;

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

    public void Init()
    {
        currentStamina = maxStamina;
    }

    public void Movement(float moveX, float moveY) 
    {
        Vector3 direction = new Vector3(moveX, 0f, moveY);

        _rb.linearVelocity = direction * _currentSpeed;
    }

    public void Sprint(bool wantToSprint)
    {
        _currentSpeed = wantToSprint ? _sprintSpeed : _normalSpeed;     
    }

    public void ResetStamina()
    {
        currentStamina = maxStamina;
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
