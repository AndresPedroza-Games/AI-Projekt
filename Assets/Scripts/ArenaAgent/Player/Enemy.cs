using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.MLAgents;

public class Enemy : MonoBehaviour
{
    [Header("---Health Settings---")]
    [SerializeField] private int _maxHealth = 10;
    public int currentHealth;

    [Header("---UI Settings---")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;

    [Header("---Components---")]
    [SerializeField] private AgentController _agentController;

    [Header("---Attack Settings---")]
    [SerializeField] private float _coolDown = 1f;

    private bool _canAttack;

    [Header("---Proyectile Settings---")]
    [SerializeField] private GameObject _proyectilePrefab;
    [SerializeField] private Transform _proyectileSpawn;

    private GameObject _currentProyectile;

    [Header("---Components---")]
    private Rigidbody _rb;

    [Header("---Movement Settings---")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();

    [Header("---Components---")]
    private Vector3 _startPos;
    private NavMeshAgent _agent;
    private int currentWaypoint = 0;

    private void Awake()
    {
        ResetPlayer();
        _canAttack = true;

        _rb = GetComponent<Rigidbody>();
        _startPos = transform.position;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_wayPoints.Count > 0)
        {
            _agent.SetDestination(_wayPoints[currentWaypoint].position);
        }
    }

    private void Update()
    {
        MoveBetweenWaypoints();
        RotateTowardsPlayer();
        Attack();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Die();
        UpdateUI();
        _agentController.AddReward(0.1f);
        _agentController.EndEpisode();
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            ResetPlayer();
            _agentController.AddReward(1f);
            _agentController.EndEpisode();
        }
    }

    private void UpdateUI()
    {
        _slider.value = currentHealth;
        _text.text = $"{currentHealth} / {_maxHealth}";
    }

    public void ResetPlayer()
    {
        currentHealth = _maxHealth;
        UpdateUI();
    }

    public void Attack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            Debug.Log("Attack");
            Shoot();
            StartCoroutine(ResetAttack());
        }
    }

    private void Shoot()
    {
        _currentProyectile = Instantiate(_proyectilePrefab, _proyectileSpawn.position, transform.rotation);
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(_coolDown);
        _canAttack = true;
    }

    private void PushBack(Collision collision)
    {
        Vector3 direction = transform.position - collision.transform.position;
        direction.y = 0f;
        direction.Normalize();

        _rb.AddForce(direction * 50f, ForceMode.Impulse);
    }

    private void MoveBetweenWaypoints()
    {
        if (_wayPoints.Count == 0)
            return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            currentWaypoint++;

            if (currentWaypoint >= _wayPoints.Count)
            {
                currentWaypoint = 0;
            }

            _agent.SetDestination(_wayPoints[currentWaypoint].position);
        }
    }

    private void RotateTowardsPlayer()
    {
        if (_agentController.transform == null)
            return;

        Vector3 direction = _agentController.transform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction); 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable isDamageable))
        {
            currentHealth = 0;
            isDamageable.Die();
            PushBack(collision);
        }            

        if (collision.gameObject.tag == "Wall")
        {
            transform.position = _startPos;
        }
    }
}
