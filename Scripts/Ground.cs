/* 
    THIS SCRIPT IS FOR MAKING THE GROUND OBJECT AND
    MAKE IT CONTINUOUSLY MOVING TOWARD THE PLAYER AND
    GENERATE ITSELF
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    
    Player player;
    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D colliderGround;

    bool didGenerateGround = false;

    public Obstacle boxTemplate;
    public Obstacle boxTemplate2;
    public Obstacle boxTemplate3;
    public Obstacle boxTemplate4;
    
    
    public int health = 1;
    public GameObject deathEffect;
    
    
    private void Awake()
    {
        //Find the player
        player = GameObject.Find("Player").GetComponent<Player>();
        
        //Initialize the player's variable
        colliderGround = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + (colliderGround.size.y / 2);
        screenRight = Camera.main.transform.position.x * 2;
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate runs constantly
    private void FixedUpdate()
    {
        //Make the ground move toward the player with the velocity of the player
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + (colliderGround.size.x / 2);

        //If the ground is too far to the left of the screen, destroy it
        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }
        
        //Make sure the ground only generate once
        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }
        //Resize
        transform.position = pos;

    }


    // Generate the ground object
    void generateGround()
    {
        // Initialize the ground object
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;
        
        /* GENERATE RANDOM HEIGHT*/
        // The high of the generated ground is a random value between a minY (lowest the player can jump) and
        // maxY (the highest the player can jump), maxY = current player height + max jump height
        float h1 = player.jumpVelocity * player.maxMaxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.5f;
        maxY += groundHeight;
        float minY = 1;        
        // Create the random height
        float actualY = Random.Range(minY, maxY);
        // Set the y postion of the ground object and make sure it's not too high for player to jump
        pos.y = actualY - goCollider.size.y / 2;
        // Make sure it's not too high
        if (pos.y > -6f) // 6f is the gap between the object and the ceiling of the screen
        {
            pos.y = -6f;
        }

        /* GENERATE RANDOM GAP BETWEEN GROUNDS */
        // Because the x pos depends on the time, we gonna use it the calculate the minX and maxX
        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY) / -player.gravity));
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.5f; //Make it a little easy for the player
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);
        // Update the x pos 
        pos.x = actualX + goCollider.size.x / 2;
        
        // Apply the x and y position
        go.transform.position = pos;

        // Reset the groundHeight since we altered our Y pos after we creating our object
        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        /* GENERATE THE ZOMBIES */
        //Generate random position of the zombie
        float y = goGround.groundHeight;
        float halfWidth = goCollider.size.x / 2 - 1;
        float left = go.transform.position.x - halfWidth;
        float right = go.transform.position.x + halfWidth;
        float x = Random.Range(left, right);
        
        //Create zombie
        int obstacleNum = Random.Range(1, 2);
        int selectZombie = Random.Range(0,4);
        for (int i=0; i < obstacleNum; i++)
        {
            if (selectZombie == 0) {
                GameObject box = Instantiate(boxTemplate.gameObject);
                Vector2 boxPos = new Vector2(x, y);
                box.transform.position = boxPos;
            }
            if (selectZombie == 1) {
                GameObject box = Instantiate(boxTemplate2.gameObject);
                Vector2 boxPos = new Vector2(x, y);
                box.transform.position = boxPos;
            }            
            if (selectZombie == 2) {
                GameObject box = Instantiate(boxTemplate3.gameObject);
                Vector2 boxPos = new Vector2(x, y);
                box.transform.position = boxPos;
            }
            if (selectZombie == 3) {
                GameObject box = Instantiate(boxTemplate4.gameObject);
                Vector2 boxPos = new Vector2(x, y);
                box.transform.position = boxPos;
            }            
        }
    }

    /*public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }*/


   
}
