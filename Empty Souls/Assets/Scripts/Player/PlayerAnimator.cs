using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator; // Drag your Animator here or get via GetComponent
    public SpriteRenderer spriteRenderer; // For flipping character

    private PlayerController movement; // Reference to your movement script

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 1. Set "isMoving" parameter for animation transitions
        bool Move = moveDir.magnitude > 0.01f;
        animator.SetBool("Move", Move);

        // 2. Flip sprite depending on direction (assume right = default)
        if (moveDir.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveDir.x < -0.01f)
            spriteRenderer.flipX = true;
    }
}