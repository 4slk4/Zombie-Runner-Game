using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public int life;
    public GameObject[] hearts;

       
    // Start is called before the first frame update
    void Start()
    {
        life = hearts.Length;
    }
    

    public void TakeDamage(int d)
    {
        life -= d;
        Destroy(hearts[life].gameObject);
    }
}
