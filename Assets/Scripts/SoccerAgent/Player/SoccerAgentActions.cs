using Unity.MLAgents.Actuators;
using UnityEngine;

public class SoccerAgentActions : MonoBehaviour
{
    [Header("---Settings---")]
    public float minShootForce = 3f;
    public float maxShootForce = 10f;

    [Header("---Components---")]
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Rigidbody _ballRb;

    private SoccerAgentController _agentController;

    private bool _canShoot;

    private void Awake()
    {
        _agentController = GetComponent<SoccerAgentController>();
    }

    public void Shoot(bool wantToShoot, float forceInput)
    {
        if (!wantToShoot)
            return;

        if (!_canShoot)
        {
            _agentController.AddReward(SoccerAgentRewards.shootAwayReward);
            return;
        }

        float normalizedForce = (forceInput + 1f) / 2f;

        float force = Mathf.Lerp(minShootForce, maxShootForce ,normalizedForce);

        if (wantToShoot && _ballRb != null)
            ShootBall(force);
    }

    private void ShootBall(float force)
    {
        Vector3 direction = (_ballRb.transform.position - transform.position).normalized;

        _ballRb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _agentController.ball)
            _canShoot = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == _agentController.ball)
            _canShoot = false;
    }
}
