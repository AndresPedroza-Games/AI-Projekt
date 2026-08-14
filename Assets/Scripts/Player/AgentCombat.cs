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

    private void Awake()
    {
        _canAttack = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
    }

    private void Attack()
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
