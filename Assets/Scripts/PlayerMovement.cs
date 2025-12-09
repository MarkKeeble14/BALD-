using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Side Movement")]
    [SerializeField] private float sideMovementForce;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [Header("Vertical Movement")]
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private float mass;
    [SerializeField] private float forceOfGravity;
    [SerializeField] private float onGroundRaycastDistance;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float minY;
    private float upwardsVelocity;
    private int curNumJumps;
    private bool onGround => transform.position.y <= minY;

    private Vector3 amountToMove;

    private void Awake()
    {
        // Setup curNumJumps
        curNumJumps = numJumps;
    }

    private void SidewaysMovement()
    {
        // Reset variables
        amountToMove.x = 0;

        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            amountToMove.x = -sideMovementForce;
        }
        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            amountToMove.x = sideMovementForce;
        }
        transform.position += amountToMove * Time.deltaTime;

        // Clamp the transform to bounds
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
    }

    private void JumpingMovement()
    {
        // if falling and landing on ground, zero out velocity and replenish jumps
        if (onGround && upwardsVelocity < 0) 
        {
            // Reset number of players jumps if they are on the ground
            curNumJumps = numJumps;

            // Reset Velocity
            upwardsVelocity = 0; 
        }

        // If have a jump remaining, allow player to jump
        if (curNumJumps > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Set upwards velocity based on jump force
                upwardsVelocity = jumpForce;

                // Remove a jump
                curNumJumps--;
            }
        }

        // if you are not on the ground, gravity applies
        if (!onGround)
        {
            // Gravity is a funciton of forceOfGravity and Mass
            upwardsVelocity -= forceOfGravity * mass * Time.deltaTime;
        }

        // Apply vertical movement
        transform.position += new Vector3(0, upwardsVelocity, 0);

        // Clamp to range
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
    }

    private void Update()
    {
        SidewaysMovement();

        JumpingMovement();
    }
}
