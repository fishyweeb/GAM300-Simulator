using StarterAssets;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float baseDamage = 10;
    Animator animator;
    public Skill skill;
    public GameObject skillPrefab;

    bool hasBeenHit = false;
    Vector3 hitDir;
    float knockBackAmount = 0;
    float hitDuration = 0.1f;
    float hitTimer = 0;

    [Header("Body")]
    public Transform rightHand;
    public Transform leftHand;
    public Transform neck;

    private Rigidbody _rigidbody;
    private Coroutine knockbackCoroutine;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetHealthFraction()
    {
        return currentHealth / maxHealth;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        if (skillPrefab)
            skill = Instantiate(skillPrefab).GetComponent<Skill>();
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float value, Vector3 hitDirection, float kb)
    {
        hitDir = hitDirection;
        knockBackAmount = kb;
        currentHealth -= value;
        hasBeenHit = true;

        // Trigger hurt animation
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
            StartCoroutine(ResetTriggerAfterDelay("Hurt", 0.1f));  // Adjust delay as needed
        }

        // Handle knockback if the enemy is still alive
        if (currentHealth > 0)
        {
            if (knockbackCoroutine != null)
            {
                StopCoroutine(knockbackCoroutine);
            }
            knockbackCoroutine = StartCoroutine(ApplyKnockback(hitDir, knockBackAmount, hitDuration));
        }
        else
        {
            Die();
        }
    }

    private IEnumerator ApplyKnockback(Vector3 direction, float knockbackForce, float duration)
    {
        float timer = 0;
        Vector3 knockbackVelocity = direction * knockbackForce;

        //// Temporarily stop movement or AI control here if needed
        //if (GetComponent<EnemyControllerRB>())
        //{
        //    GetComponent<EnemyControllerRB>().disableMovement = true;
        //}

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // Smoothly apply knockback over time
            _rigidbody.velocity = new Vector3(knockbackVelocity.x, _rigidbody.velocity.y, knockbackVelocity.z);

            yield return null; // Wait until the next frame
        }

        //// Re-enable movement after knockback is done
        //if (GetComponent<EnemyControllerRB>())
        //{
        //    GetComponent<EnemyControllerRB>().disableMovement = false;
        //}

        // Reset velocity after knockback
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
    }

    public void Die()
    {
        currentHealth = 0;
        if (animator != null)
        {
            animator.SetTrigger("Death");
            animator.SetBool("Dead", true);
        }

        // Additional cleanup logic on death
        if (_rigidbody)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        DisableComponentsOnDeath();
        StartCoroutine(DestroyOverTime(3f));
    }

    private void DisableComponentsOnDeath()
    {
        if (GetComponent<EnemyControllerRB>())
        {
            Destroy(GetComponent<EnemyControllerRB>());
        }
        if (GetComponent<PathfindingScript>())
        {
            Destroy(GetComponent<PathfindingScript>());
        }
        if (GetComponent<EnemyAI>())
        {
            Destroy(GetComponent<EnemyAI>());
        }
        if (GetComponent<ThirdPersonControllerRB>())
        {
            Destroy(GetComponent<ThirdPersonControllerRB>());
        }
        if (GetComponent<Rigidbody>()) 
        {
            GetComponent<Rigidbody>().velocity= Vector3.zero;
            GetComponent<Rigidbody>().useGravity = true;
        }
        if (GetComponent<EnemyHackable>())
        {
            Destroy(GetComponent<EnemyHackable>());
        }
        if (GetComponent<CapsuleCollider>())
        {
            Destroy(GetComponent<CapsuleCollider>());
        }
        gameObject.AddComponent<BoxCollider>().size = new Vector3(0.01f, 0.01f, 0.01f);
        gameObject.layer = 0;
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
        // Other logic, such as health checks or movement, goes here.
    }

    // Coroutine to reset the trigger after a short delay
    IEnumerator ResetTriggerAfterDelay(string triggerName, float delay)
    {
    yield return new WaitForSeconds(delay);
    animator.ResetTrigger(triggerName);
    }

    private IEnumerator DestroyOverTime(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
