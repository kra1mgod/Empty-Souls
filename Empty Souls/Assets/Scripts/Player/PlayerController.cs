using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (playerStats != null)
                playerStats.UseActiveAbility();
        }
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = new Vector2(h, v);
        if (playerStats != null)
        {
            playerStats.SetMoveDirection(moveDir);
        }
    }

    void FixedUpdate()
    {
        float actualSpeed = playerStats != null ? playerStats.moveSpeed : speed;
        rb.MovePosition(rb.position + moveInput.normalized * actualSpeed * Time.fixedDeltaTime);
    }
}