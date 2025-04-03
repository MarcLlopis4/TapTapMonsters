using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Salud máxima del monstruo
    public float CurrentHealth { get; private set; } // Salud actual del monstruo

    void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        CurrentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
    }
}