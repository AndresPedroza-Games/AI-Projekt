using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _minDistance = 0.01f;

    [SerializeField] private SoccerAgentController _agentController;

    private Transform _currentWaypoint;

    private void Start()
    {
        _currentWaypoint = RandomWayPoint();
    }

    private void Update()
    {
        MoveObstacle();
    }

    private void MoveObstacle()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, (_speed * 10) * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentWaypoint.position) <= _minDistance)
            _currentWaypoint = RandomWayPoint();
    }

    private Transform RandomWayPoint()
    {
        int index = Random.Range(0, _wayPoints.Count);

        Transform wayPoint = _wayPoints[index];

        return wayPoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.TryGetComponent<Ball>(out Ball _ball))
        {
            _agentController.AddReward(SoccerAgentRewards.touchObstacle);
        }
    }
}
