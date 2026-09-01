using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("---Components---")]
    private Rigidbody _rb;

    [Header("---Movement Settings---")]
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _normalSpeed = 1f;
    [SerializeField] private float _sprintSpeed = 3f;

    private bool _wantToSprint;

    [Header("---Shoot Settings---")]
    [SerializeField] private Rigidbody _ballRb;
    [SerializeField] private float _shootForce = 5f;

    private bool _canShoot;

    [Header("---Goals---")]
    [SerializeField] private GameObject _teamGoal;
    [SerializeField] private GameObject _enemyGoal;
    [SerializeField] private Transform _startPoint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        FindAnyObjectByType<SoccerAgentController>().onGoal += () => transform.position = _startPoint.position;
        FindAnyObjectByType<SoccerAgentController>().onGoalReceived += () => transform.position = _startPoint.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _wantToSprint = true;
        else
            _wantToSprint = false;

        Sprint(_wantToSprint);
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && _canShoot)
            ShootBall();
    }

    private void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(moveX, 0f, moveY);

        _rb.linearVelocity = direction * _currentSpeed;
    }

    private void Sprint(bool wantToSprint)
    {
        _currentSpeed = wantToSprint ? _sprintSpeed : _normalSpeed;
    }

    private void ShootBall()
    {
        Vector3 direction = (_ballRb.transform.position - transform.position).normalized;

        _ballRb.AddForce(direction * _shootForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
            GetComponentInChildren<Renderer>().material.color = Color.red;

        if (collision.gameObject.TryGetComponent(out Ball ball))
            _canShoot = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
            _canShoot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _teamGoal || other.gameObject == _enemyGoal)
        {
            transform.position = _startPoint.position;
        }
    }
}
