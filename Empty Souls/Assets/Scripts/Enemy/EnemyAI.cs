using UnityEngine;
using System.Timers;
using System;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 10;
    private Transform player;
    private float stunTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            return;
        }
        if (player == null) return;
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }
    private static void Tick(System.Object source, ElapsedEventArgs e)
    {
        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
    }
    public void ApplyStun(float duration)
    {
        stunTimer = Mathf.Max(stunTimer, duration);
    }
    //OnTriggerEnter2D, если у врага или игрока стоит Collider2D с isTrigger = true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                int baseDamage = damage; // damage врага
                float strengthBonus = stats.GetStrengthTouchDamageBonus();
                stats.TakeDamage(baseDamage + Mathf.RoundToInt(strengthBonus));
            }
        }
    }
}