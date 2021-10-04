using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int startingHealth;
    public int currentHealth;
    public UnityEvent OnHealthZero;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            OnHealthZero.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
