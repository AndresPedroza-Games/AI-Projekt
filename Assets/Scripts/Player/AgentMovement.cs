using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [Header("---Components---")]
    private Rigidbody _rb;

    [Header("---Movement Settings---")]
    [SerializeField] private float _speed = 10;


    private new void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();

    }

    private void Movement()
    {
        float moveY = Input.GetAxisRaw("Horizontal");
        float moveX = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(-moveX, 0f, moveY);

        _rb.linearVelocity = direction * (_speed * 100f) * Time.deltaTime;
    }
}
