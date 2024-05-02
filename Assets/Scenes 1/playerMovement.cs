using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D myRigidbody;
    private Vector2 change;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Capture player input
        change = Vector2.zero;
        change.x = Input.GetAxis("Horizontal");
        //change.y = Input.GetAxis("Vertical");
        UpdateAnimation(); // Call the method to update animation
    }

    void UpdateAnimation()
    {
        if (change != Vector2.zero)
        {
            animator.SetFloat("moveX", change.x);
            //animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody
        MoveCharacter(change);
    }

    void MoveCharacter(Vector2 direction)
    {
        // Normalize the direction to ensure consistent speed in all directions
        direction.Normalize();
        myRigidbody.MovePosition(myRigidbody.position + direction * speed * Time.fixedDeltaTime);
    }
}
