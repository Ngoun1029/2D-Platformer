using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 1.0f; // Adjust the duration as needed
    private float invulnerabilityTimer = 0.0f;

    public UIManager uiManager;

    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();   
    }

    private void Update()
    {
        // Update the invulnerability timer if player is invulnerable
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer <= 0.0f)
            {
                isInvulnerable = false;
            }
        }
    }

    public void TakeDamage(float _damage)
    {
        if (!isInvulnerable)
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            if (currentHealth > 0)
            {
                // Hurt animation or other behavior
                anim.SetTrigger("hurt");
            }
            else
            {
                // Die animation or other behavior
                if (!dead)
                {
                    anim.SetTrigger("die");
                    uiManager.LoseScreen();
                    dead = true;
                }
            }

            // Set invulnerability
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
        }
    }
}
