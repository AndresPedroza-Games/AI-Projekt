using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentHealth : MonoBehaviour, IDamageable
{
    [Header("---Health Settings---")]
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;

    [Header("---UI Settings---")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Die();
        UpdateUI();
    }

    public void Die()
    {
        if (_currentHealth <= 0)
            Debug.Log("Dead");
    }

    private void UpdateUI()
    {
        _slider.value = _currentHealth;
        _text.text = $"{_currentHealth} / {_maxHealth}";
    }
}
