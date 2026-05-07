using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("Hitbox")]
    public GameObject hitboxPrefab;
    public float attackDistance = 2.5f;
    public float attackDuration = 0.25f;
    public float attackDmg = 25f;

    [Header("Cooldown")]
    public float attackCooldown = 0.1f;

    private float attackTimer = 0f;
    private float attack2Timer = 0f;
    private float flurryTimer = 0f;

    [Header("References")]
    [SerializeField] private Animator anim;
    public PlayerController playerController;

    [Header("UI")]
    public Image attackOverlay;
    public Text attackText;

    public Image attack2Overlay;
    public Text attack2Text;

    public Image flurryOverlay;
    public Text flurryText;

    void Update()
    {
        // timers
        attackTimer += Time.deltaTime;
        attack2Timer += Time.deltaTime;
        flurryTimer += Time.deltaTime;

        Vector2 facingDirection = playerController.isFacingRight ? Vector2.right : Vector2.left;

        // INPUTS
        if (Input.GetButton("Attack") && attackTimer >= attackCooldown)
        {
            SpawnAttack(facingDirection);
            attackTimer = 0f;

            int rand = Random.Range(0, 2);
            anim.SetTrigger(rand == 0 ? "PlayerAttack2" : "PlayerAttack3");
        }

        if (Input.GetButton("Attack2") && attack2Timer >= attackCooldown)

            int primaryAttack = Random.Range(0, 2);


            if (primaryAttack == 0)
            {
                anim.SetTrigger("PlayerAttack2");
            }
            else
            {
                anim.SetTrigger("PlayerAttack3");
            }
            // primaryAttack = !primaryAttack;

        }
        if (Input.GetButton("Attack2") && canAttack)
        {
            SpawnAttack(facingDirection);
            attack2Timer = 0f;
            anim.SetTrigger("PlayerAttack");
        }

        if (Input.GetButton("AttackFlurry") && flurryTimer >= attackCooldown)
        {
            SpawnAttack(facingDirection);
            flurryTimer = 0f;
            anim.SetTrigger("PlayerAttackFlurry");
        }

        // UI updates (same system as shuriken)
        UpdateCooldownUI(attackOverlay, attackText, attackTimer);
        UpdateCooldownUI(attack2Overlay, attack2Text, attack2Timer);
        UpdateCooldownUI(flurryOverlay, flurryText, flurryTimer);
    }

    void SpawnAttack(Vector2 facingDirection)
    {
        Vector3 localOffset = new Vector3(facingDirection.x * attackDistance, 0f, 0f);
        Vector3 spawnPos = transform.position + localOffset;

        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);
        hitbox.transform.SetParent(transform);

        if (!playerController.isFacingRight)
        {
            Vector3 scale = hitbox.transform.localScale;
            scale.x *= -1f;
            hitbox.transform.localScale = scale;
        }

        Rigidbody2D rb = hitbox.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 playerVel = playerController.GetPlayerVector();
            rb.linearVelocity = new Vector2(playerVel.x, 0);
        }

        Destroy(hitbox, attackDuration);
    }

    void UpdateCooldownUI(Image overlay, Text text, float timer)
    {
        if (overlay == null) return;

        if (timer < attackCooldown)
        {
            float percent = 1 - (timer / attackCooldown);

            overlay.fillAmount = percent;
            overlay.color = new Color(0, 0, 0, 0.6f);

            if (text != null)
            {
                text.gameObject.SetActive(true);
                text.text = Mathf.Ceil(attackCooldown - timer).ToString();
            }
        }
        else
        {
            overlay.fillAmount = 0f;

            if (text != null)
                text.gameObject.SetActive(false);
        }
    }
}