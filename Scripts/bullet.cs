using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float lifeTime;
    public int damage = 1;

    public Sprite FlashSprite;
    //[Range(0, 5)]
    public int FramesToFlash = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoFlash());
        rb.velocity = transform.right * speed;
        Invoke("DestroyProjectile", lifeTime);
    }
    
    void DestroyProjectile()
    {
        Destroy(gameObject); //Destroys the bullet
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

    IEnumerator DoFlash()
    {
        var renderer = GetComponent<SpriteRenderer>();
        var originalSprite = renderer.sprite;
        renderer.sprite = FlashSprite;

        var framesFlashed = 0;
        while(framesFlashed < FramesToFlash)
        {
            framesFlashed++;
            yield return null;
        }

        renderer.sprite = originalSprite;
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
