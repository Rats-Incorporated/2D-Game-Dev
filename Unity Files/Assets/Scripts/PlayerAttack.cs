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

    public Material mat;
    private bool isCharging = false;

    //private SpriteRenderer sr;
    //private Material mat;

    private float TotalChargeTime = 1f;
    private float ChargeTime = 0f;

    void Awake()
    {
        // sr = GetComponent<SpriteRenderer>();
        // mat = sr.material;
        mat.SetFloat("_Charge", 0f);
        mat.SetFloat("_FullyCharged", 0f);

    }

    void Update()
    {
        // timers
        attackTimer += Time.deltaTime;
        attack2Timer += Time.deltaTime;
        flurryTimer += Time.deltaTime;

        Vector2 facingDirection = playerController.isFacingRight ? Vector2.right : Vector2.left;

        // ======================
        // PRIMARY ATTACK (random anim) (Also downwards attack)
        // ======================
        if (Input.GetButton("Attack") && attackTimer >= attackCooldown)
        {
            //DOWWNWARDS ATTACK
            if (Input.GetAxisRaw("UpDown") < -0.5f)
            {
                SpawnAttack(facingDirection);
                attackTimer = 0f;
                anim.SetTrigger("PlayerAttackDown");
            }
            //NORMAL ATTACK
            else
            {
                SpawnAttack(facingDirection);
                attackTimer = 0f;

                int rand = Random.Range(0, 2);

                if (rand == 0)
                    anim.SetTrigger("PlayerAttack2");
                else
                    anim.SetTrigger("PlayerAttack3");
            }
        }

        // ======================
        // ATTACK 2 (single anim)
        // ======================
        if (Input.GetButton("Attack2") && attack2Timer >= attackCooldown)
        {
            SpawnAttack(facingDirection);
            attack2Timer = 0f;
            anim.SetTrigger("PlayerAttack");
        }


        // ======================
        // FLURRY
        // ======================
        if (Input.GetButton("AttackFlurry") && flurryTimer >= attackCooldown)
        {
            mat.SetFloat("_Charge", 1f);
            ChargeTime += Time.deltaTime;


            //SpawnAttack(facingDirection);
            //flurryTimer = 0f;
            //anim.SetTrigger("PlayerAttackFlurry");

            if(ChargeTime > TotalChargeTime)
            {
                mat.SetFloat("_FullyCharged", 1f);
            }

        }

        if (Input.GetButtonUp("AttackFlurry") && ChargeTime >= TotalChargeTime && flurryTimer >= attackCooldown)
        {
            mat.SetFloat("_Charge", 1f);

            SpawnAttack(facingDirection);
            flurryTimer = 0f;
            anim.SetTrigger("PlayerAttackFlurry");

            ChargeTime = 0f;
        }
        if (Input.GetButtonUp("AttackFlurry") && ChargeTime < TotalChargeTime)
        {
            mat.SetFloat("_Charge", 0f);
            mat.SetFloat("_FullyCharged", 0f);
            ChargeTime = 0f;
        }

        // ======================
        // UI UPDATES (shared system)
        // ======================
        UpdateCooldownUI(attackOverlay, attackText, attackTimer);
        UpdateCooldownUI(attack2Overlay, attack2Text, attack2Timer);
        UpdateCooldownUI(flurryOverlay, flurryText, flurryTimer);
    }

    void SpawnAttack(Vector2 facingDirection)
    {
        Vector3 spawnPos = transform.position + (Vector3)(facingDirection * attackDistance);

        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);
        hitbox.transform.SetParent(transform);

        // flip hitbox if facing left
        if (!playerController.isFacingRight)
        {
            Vector3 scale = hitbox.transform.localScale;
            scale.x *= -1f;
            hitbox.transform.localScale = scale;
        }

        // match player velocity
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