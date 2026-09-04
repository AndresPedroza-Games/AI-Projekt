using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _minDistance = 0.01f;

    [SerializeField] private SoccerAgentController _agentController;

    private Transform _currentWaypoint;
}
