using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool callGamePause;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        callGamePause = PauseMenu.gameIsPaused;
        if (Input.GetKeyUp(KeyCode.F) && !callGamePause)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //Shooting logic
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
