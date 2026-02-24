using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class Shuriken : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public float damage = 25f;

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyDefault enemy = other.GetComponent<EnemyDefault>();

        if (enemy != null)
        {
            enemy.EnemyTakeDamage(damage);
        }
        Destroy(gameObject);
    }
}