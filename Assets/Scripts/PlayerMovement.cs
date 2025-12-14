using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Side Movement")]
    [SerializeField] private float sideMovementForce;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [Header("Sprint")]
    [SerializeField] private float sprintingSpeedMult = 1f;
    private bool isSprinting => Input.GetKey(KeyCode.LeftShift);

    [Header("Slide")]
    [SerializeField] private float slidingSpeedMult;
    private bool isSliding;

    [Header("Vertical Movement")]
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private float mass;
    [SerializeField] private float forceOfGravity;
    [SerializeField] private float onGroundRaycastDistance;
    [SerializeField] private float minY;
    private float upwardsVelocity;
    private int curNumJumps;
    private bool onGround => transform.position.y <= minY;

    private Vector3 amountToMove;

    [Header("Hold to Increase Jump System")]
    [SerializeField] private Timer holdToIncreaseJumpHeightTimer;
    [SerializeField] private float onHoldAddedJumpForce;

    private void Awake()
    {
        // Setup curNumJumps
        curNumJumps = numJumps;
    }

    private void SidewaysMovement()
    {
        // Reset variables from last frame
        amountToMove.x = 0;

        // Add the direction to the players position
        // Take into account whether the player is sprinting or not 
        transform.position += amountToMove 
            * (Input.GetKey(KeyCode.LeftShift) ? sprintingSpeedMult : 1) 
            * (isSliding ? slidingSpeedMult : 1)
            * Time.deltaTime;

        // Clamp the transform to bounds
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        // Slide
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (amountToMove.x !=0)
            {
                isSliding = true;
            }
        } else
        {
            isSliding = false;
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

                // Begin hold to increase jump height timer
                holdToIncreaseJumpHeightTimer.Reset();
            }
        }

        // Update hold to increase jump height timer
        if (!holdToIncreaseJumpHeightTimer.TimesUp)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                upwardsVelocity += onHoldAddedJumpForce * Time.deltaTime;
            }
            holdToIncreaseJumpHeightTimer.Update();
        }

        // if you are not on the ground, gravity applies
        if (!onGround)
        {
            // Gravity is a funciton of forceOfGravity and Mass
            upwardsVelocity -= forceOfGravity * mass * Time.deltaTime;
        }

        // Apply vertical movement
        transform.position += new Vector3(0, upwardsVelocity, 0) * Time.deltaTime;

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
