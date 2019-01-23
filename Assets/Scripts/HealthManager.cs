using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public UnityEvent OnLoseHealth;
    public UnityEvent OnDeath;

    public ProgressBar healthBar;
	public ProgressBarCircle healthCircle;
	public int health = 100;

	void Start()
    {
		if (healthBar != null)
		{
			healthBar.BarValue = health;
		}

		if (healthCircle != null)
        {
            healthCircle.BarValue = health;
        }
    }

	public void LoseHealth (int damage)
    {
		if (healthBar != null)
		{
			health += -damage;
			healthBar.BarValue = health;
		}

		if (healthCircle != null)
        {
            health += -damage;
			healthCircle.BarValue = health;
        }
        OnLoseHealth.Invoke();

        if(health <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
