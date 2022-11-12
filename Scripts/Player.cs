/*
    THIS SCRIPT IS ALL ABOUT THE PLAYER MECHANISM
    INCLUDING: JUMPING, COLLIDE WITH GROUND AND ZOMBIES
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSound;

    LifeSystem lifeScript;
    public float gravity;
    public Vector2 velocity;
    public float maxAcceleration = 10;
    public float maxXVelocity = 60;
    public float distance = 0;
    public float acceleration = 1;
    public float jumpVelocity = 80;
    public float groundHeight = 10;
    public bool isGrounded = false;

    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.4f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;

    public float jumpGroundThreshold = 1;
    public bool isDead = false;

    public bool isPowerUp = false;
    public float powerupTimer = 0.0f;

    /*Variables for Fixing Bugs */
    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    public LayerMask powerupLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        lifeScript = GameObject.Find("Player").GetComponent<LifeSystem>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpSound.Play();
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }



    }
    /*START FIXEDUPDATE FUNCTION */
    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (isDead)
        {
            return;
        }
        
        if (pos.y < -20)
        {
            isDead = true;
        }

        //Power Up time limit is 1 seconds
        if (isPowerUp)
        {
            powerupTimer += Time.fixedDeltaTime;
            if (powerupTimer >= 1f)
            {
                isPowerUp = false;
                velocity.x = 20;
                maxXVelocity = 60;
            }
            else
            {
                maxXVelocity = 84;
                velocity.x = 84;
            }
        }


        //If the player is floating in the air
        if (!isGrounded)
        {
            //When the player holding jump button, he jumps higher
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            pos.y += velocity.y * Time.fixedDeltaTime;
            

            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            /*GROUND COLLIDING MECHANISM*/
            // This mechanism uses RayCasting, a function in Unity used to identify interaction between object
            // RayCasting emits a ray, and whenever the ray hits any object, it triggers an action of the object
            // We use RayCasting in here to make the player standing on the platform, and it only works
            // when the platform is a BoxCollider2D object
            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up; //Idk why the vector is up but it works
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            // Trigger an action when the ray hit a BoxCollider2D object
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y >= ground.groundHeight) 
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true;
                    }
                }
            }
            // Drawing a red line showing the RayCasting vector, useful for debugging
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

            /* WALL COLLISION MECHANISM */
            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight) // To make sure we never trigger this for the ground
                    {
                        velocity.x = 0;
                    }
                }
            }
        }

        // Generate distance value, we are gonna use this variable to display on screen
        distance += velocity.x * Time.fixedDeltaTime;

        // If the player is on the ground
        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio; // The player can only hold the jump button when he gains speed

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            // RayCasting for when the ground is not beneath the player
            // and he will fall endlessly
            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up; //Idk why the vector is up but it works
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            // Trigger an action when the ray does not hit a BoxCollider2D object
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            // Drawing a yellow line for debugging
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }
        /*END GROUND COLLIDING MECHANISM*/



        /* RAYCAST FOR HITTING OBSTACLES */       
        Vector2 obsOrigin = new Vector2(pos.x, pos.y);
        // Hitting from the left
        RaycastHit2D obstHitX = Physics2D.Raycast(obsOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitX.collider != null)
        {
            Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle, isPowerUp);
            }
        }
        // Hitting from above
        RaycastHit2D obstHitY = Physics2D.Raycast(obsOrigin, Vector2.down, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitY.collider != null)
        {
            Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle, isPowerUp);
            }
        }
        /*END RAYCAST FOR HITTING OBSTACLES */ 

        /* RAYCAST FOR HITTING POWERUP */
        Vector2 puOrigin = new Vector2(pos.x, pos.y);
        // Hitting from the left
        RaycastHit2D puHitX = Physics2D.Raycast(puOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, powerupLayerMask);
        if (puHitX.collider != null)
        {
            PowerUp elixir = puHitX.collider.GetComponent<PowerUp>();
            if (elixir != null)
            {
                hitPowerUp(elixir);
            }
        }
        // Hitting from above
        RaycastHit2D puHitY = Physics2D.Raycast(puOrigin, Vector2.down, velocity.y * Time.fixedDeltaTime, powerupLayerMask);
        if (puHitY.collider != null)
        {
            PowerUp elixir = puHitY.collider.GetComponent<PowerUp>();
            if (elixir != null)
            {
                hitPowerUp(elixir);
            }
        }
        /*END RAYCAST FOR HITTING POWERUP */
        
        
        // Update the postion
        transform.position = pos;
    }
    /*END FIXEDUPDATE FUNCTION */


    // Hitting obstacle zombie mechanism
    void hitObstacle(Obstacle obstacle, bool isPowerUp)
    {        
        if (isPowerUp)
        {
            Destroy(obstacle.gameObject); //Kill zombie
        }
        else
        {
            int life = lifeScript.life;
        
            if (life > 0)
            {
                lifeScript.TakeDamage(1);
                velocity.x *= 0.7f; //Slow down when hit a zombie
                Destroy(obstacle.gameObject); //Kill zombie
            }
            else
            {
                velocity.x = 0;
                isDead = true;
            }
        }        
    }

    //Hitting PowerUp mechanism
    void hitPowerUp(PowerUp elixir)
    {
        Destroy(elixir.gameObject);
        isPowerUp = true;
        powerupTimer = 0.0f;
    }

    

}
