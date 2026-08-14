using System.Collections;
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

    private void Awake()
    {
        _canAttack = true;
        _agentController = GetComponent<AgentController>();
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

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(_coolDown);
        _canAttack = true;
    }
}
