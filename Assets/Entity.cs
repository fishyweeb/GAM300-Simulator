using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth = 100;
    [SerializeField]
    protected float currentHealth;
    [SerializeField]
    protected float baseDamage = 10;
    Animator animator;

    bool hasBeenHit = false;
    Vector3 hitDir;
    float knockBackAmount = 0;
    float hitDuration = 0.1f;
    float hitTimer = 0;
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
        if (currentHealth < 0 )
        {
            currentHealth = 0;
        }
    }

    public void TakeDamage(float value,Vector3 hitDirection, float kb)
    {
        hitDir = hitDirection;
        Debug.LogError("Take Hit "+ hitDirection);
        knockBackAmount = kb;
        currentHealth -= value;
        hitTimer = 0;
        hasBeenHit = true;
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void Heal(float value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public float GetBaseDamage()
    {
        return baseDamage;
    }

    private void Update()
    {
        if (hasBeenHit)
        {
            hitTimer += Time.deltaTime;
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.Move(hitDir * knockBackAmount * Time.deltaTime);
            }
            if (hitTimer >= hitDuration)
            {
                hitTimer = 0;
                hasBeenHit = false;
            }
        }
    }
}
