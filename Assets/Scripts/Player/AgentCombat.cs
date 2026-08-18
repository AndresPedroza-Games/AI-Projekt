using System.Collections;
using TMPro;
using UnityEngine;

public class AgentCombat : MonoBehaviour
{
    [Header("---Attack Settings---")]
    [SerializeField] private float _coolDown = 1f;

    private bool _canAttack;

    [Header("---Proyectile Settings---")]
    [SerializeField] private GameObject _proyectilePrefab;
    [SerializeField] private Transform _proyectileSpawn;

    private GameObject _currentProyectile;

    [Header("---Components---")]
    private AgentController _agentController;
    private AgentHealth _myHealth;
    [SerializeField] private AgentHealth _enemyHealth;

    [Header("---UI Components---")]
    [SerializeField] private TMP_Text _cumulativeText;
    [SerializeField] private TMP_Text _episodeText;

    private void Awake()
    {
        _canAttack = true;
        _agentController = GetComponent<AgentController>();
        _myHealth = GetComponent<AgentHealth>();
    }

    void OnEnable()
    {
        _enemyHealth.OnDamageTaken += HandleDamageDealt;
        _myHealth.OnDamageTaken += HandleDamageTaken;
        _enemyHealth.OnDeath += HandleKill;
        _myHealth.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        _enemyHealth.OnDamageTaken -= HandleDamageDealt;
        _myHealth.OnDamageTaken -= HandleDamageTaken;
        _enemyHealth.OnDeath -= HandleKill;
        _myHealth.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            Attack();

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

    private void HandleDamageDealt(float damage)
    {
        _agentController.AddReward((damage / _enemyHealth._maxHealth) * 0.5f);

        _agentController._cumulativeReward = _agentController.GetCumulativeReward();

        _cumulativeText.text = $"Cumulative Reward: {_agentController._cumulativeReward}";
        _episodeText.text = $"Current Episode: {_agentController._currentEpisode}";

        Debug.Log("hit");

    }

    private void HandleDamageTaken(float damage)
    {
        _agentController.AddReward(-(damage / _myHealth._maxHealth) * 0.05f);

        _agentController._cumulativeReward = _agentController.GetCumulativeReward();

        _cumulativeText.text = $"Cumulative Reward: {_agentController._cumulativeReward}";
        _episodeText.text = $"Current Episode: {_agentController._currentEpisode}";
    }

    private void HandleKill()
    {
        _agentController.AddReward(1f);
        _agentController.EndEpisode();

        _agentController._cumulativeReward = _agentController.GetCumulativeReward();

        _cumulativeText.text = $"Cumulative Reward: {_agentController._cumulativeReward}";
        _episodeText.text = $"Current Episode: {_agentController._currentEpisode}";
    }

    private void HandleDeath()
    {
        _agentController.AddReward(-1f);
        _agentController.EndEpisode();

        _agentController._cumulativeReward = _agentController.GetCumulativeReward();

        _cumulativeText.text = $"Cumulative Reward: {_agentController._cumulativeReward}";
        _episodeText.text = $"Current Episode: {_agentController._currentEpisode}";
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(_coolDown);
        _canAttack = true;
    }
}
