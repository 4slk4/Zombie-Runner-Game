using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float lifeTime;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        Invoke("DestroyProjectile", lifeTime);
    }
    
    void DestroyProjectile()
    {
        Destroy(gameObject); //Destroys the knife
    }

    /*void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Ground enemy = hitInfo.GetComponent<Ground>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Obstacle obstacle = hitInfo.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            //obstacle.TakeDamage(damage);
            Destroy(obstacle.gameObject);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }

}
