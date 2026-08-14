using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentHealth : MonoBehaviour, IDamageable
{
    [Header("---Health Settings---")]
    [SerializeField] private int _maxHealth = 10;
    public int currentHealth;

    [Header("---UI Settings---")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;

    [Header("---Components---")]
    private AgentController _agentController;

    private void Awake()
    {
        currentHealth = _maxHealth;
        UpdateUI();
        _agentController = GetComponent<AgentController>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Die();
        UpdateUI();
        _agentController.AddReward(-0.1f);

        _agentController.enemy.GetComponent<AgentController>().AddReward(0.1f);

        Debug.Log($"{gameObject.name}+{_agentController.GetCumulativeReward()}");
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            ResetPlayer();
            _agentController.AddReward(-1f);

            _agentController.enemy.GetComponent<AgentController>().AddReward(1f);

            Debug.Log($"{gameObject.name}+{_agentController.GetCumulativeReward()}");
        }
    }

    private void UpdateUI()
    {
        _slider.value = currentHealth;
        _text.text = $"{currentHealth} / {_maxHealth}";
    }

    public void ResetPlayer()
    {
        currentHealth = _maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable isDamageable))
            isDamageable.TakeDamage(1);
    }
}
