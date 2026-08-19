using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("---Agents---")]
    [SerializeField] private SoccerAgentController _agentController;

    [Header("---Goals---")]
    [SerializeField] private GameObject _teamGoal;
    [SerializeField] private GameObject _enemyGoal;

    [SerializeField] private Transform _startPoint;

    private bool _canRestart;

    private void Awake()
    {
        _canRestart = true;
    }

    private void Update()
    {
        if (_canRestart)
        {
            _canRestart = false;
            RestartBall();
        }
    }

    private IEnumerator RestartBall()
    {
        yield return new WaitForSeconds(15f);
        transform.position = _startPoint.position;
        _canRestart = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _teamGoal)
        {
            _agentController.onGoalReceived.Invoke();
            transform.position = _startPoint.position;
        }

        if (other.gameObject == _enemyGoal)
        {
            _agentController.onGoal.Invoke();
            transform.position = _startPoint.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            transform.position = _startPoint.position;
        }
    }
}
