using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float lifeTime;
    public bool callGamePause;
    public float coolDown = 0.2f;
    float _nextFireTime = 0f;


    [SerializeField] private AudioSource gunSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
    }
    

    // Update is called once per frame
    void Update()
    {
        callGamePause = PauseMenu.gameIsPaused;
        if (Input.GetKeyUp(KeyCode.F) && Time.time > _nextFireTime && !callGamePause)
        {
            _nextFireTime = Time.time + coolDown;
            gunSoundEffect.Play();
            Shoot();
        }
    }

    void Shoot()
    {
        //Shooting logic
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
