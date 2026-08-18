using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _damage = 1;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += transform.forward * (_speed * 10f) * Time.deltaTime;
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable isDamageable))
        {
            isDamageable.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}
