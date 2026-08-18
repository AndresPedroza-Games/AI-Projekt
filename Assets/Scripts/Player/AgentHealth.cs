using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentHealth : MonoBehaviour, IDamageable
{
    [Header("---Health Settings---")]
    public int _maxHealth = 10;
    public int currentHealth;

    [Header("---UI Settings---")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;

    [Header("---Components---")]
    private AgentController _agentController;

    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

    private void Awake()
    {
        ResetPlayer();
        _agentController = GetComponent<AgentController>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Die();
        UpdateUI();
        OnDamageTaken?.Invoke(damage);

        _agentController._cumulativeReward = _agentController.GetCumulativeReward();
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            ResetPlayer();
            OnDeath?.Invoke();

            _agentController._cumulativeReward = _agentController.GetCumulativeReward();
            _agentController._currentEpisode = _agentController.CompletedEpisodes;

            Debug.Log("dead");
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
        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable isDamageable))
        {
            TakeDamage(1);
        }
    }
}
