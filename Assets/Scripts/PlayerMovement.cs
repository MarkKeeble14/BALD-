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
    private bool onGround => !hasFallen && transform.position.y <= minY;

    [Header("Falling")]
    [SerializeField] private LayerMask checkForPitMask;
    [SerializeField] private LayerMask pitMask;
    [SerializeField] private float isFallingFeelersRange;
    [SerializeField] private float isFallingFeelersArc;
    [SerializeField] private int isFallingNumFeelers;
    private bool hasFallen;

    private Vector3 amountToMove;

    [Header("Parry Strike")]
    [SerializeField] private float parryRange;
    [SerializeField] private float parryPower;
    [SerializeField] private float parryArc;
    [SerializeField] private int parryNumFeelers;
    [SerializeField] private LayerMask hazards;

    [Header("Hold to Increase Jump System")]
    [SerializeField] private Timer holdToIncreaseJumpHeightTimer;
    [SerializeField] private float onHoldAddedJumpForce;

    private void Awake()
    {
        // Setup curNumJumps
        curNumJumps = numJumps;
    }

    // No longer being used
    private void SidewaysMovement()
    {
        // Reset variables from last frame
        amountToMove.x = 0;

        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            amountToMove.x += -sideMovementForce;
        }

        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            amountToMove.x += sideMovementForce;
        }

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

    private void ParryStrike()
    {
        // If the game manager says don't allow input, don't!
        if (GameManager._Instance.BlockInput) return;

        // Parry downwards
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (int i = -parryNumFeelers; i <= parryNumFeelers; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(i * parryArc, -1), parryRange, hazards);
                if (hit.collider != null && hit.collider.CompareTag("Parryable"))
                {
                    upwardsVelocity = parryPower;
                    return;
                }
            }
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
            if (!GameManager._Instance.BlockInput && Input.GetKeyDown(KeyCode.Space))
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
            if (!GameManager._Instance.BlockInput && Input.GetKey(KeyCode.Space))
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

        // Clamp to range, only if player hasn't fallen
        if (hasFallen) return;
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
    }

    private void CheckIfHasFallen()
    {
        if (transform.position.y > minY) return;
        if (hasFallen) return;

        // Probe underneath the player several times
        for (int i = -isFallingNumFeelers; i <= isFallingNumFeelers; i++)
        {
            // For each probe
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(i * isFallingFeelersArc, -1),
                isFallingFeelersRange, checkForPitMask);

            // We check if we hit something, and if so, check if it is not pit
            if (hit.collider != null && !LayerMaskHelper.IsInLayerMask(hit.collider.gameObject, pitMask))
            {
                // As long as we hit anything that isn't a pit, we haven't yet fallen
                return;
            }
        }

        // We went through all probes and hit nothing but pit, therefore we have fallen and should trigger die state
        hasFallen = true;
        GameManager._Instance.DieState();
    }

    private void Update()
    {
        CheckIfHasFallen();
        JumpingMovement();
        ParryStrike();
    }
}
