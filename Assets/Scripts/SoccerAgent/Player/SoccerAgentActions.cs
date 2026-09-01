using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using UnityEngine;

public class SoccerAgentActions : MonoBehaviour
{
    [Header("---Settings---")]
    public float minShootForce = 3f;
    public float maxShootForce = 10f;
    public float shootDistance = 1.5f;

    [Header("---Components---")]
    public Rigidbody _ballRb;

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

        float distance = Vector3.Distance(transform.position, _ballRb.transform.position);

        if (distance > shootDistance)
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
}
