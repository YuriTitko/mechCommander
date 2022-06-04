using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //VARIABLES
    [SerializeField] Transform firingPoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float firingSpeed;

    public static Weapon Instance;

    private float lastTimeShot = 0;

    void Awake()
    {
        Instance = GetComponent<Weapon>();
    }

    void Update()
    {
        
    }

    public void Shoot()
    {
        if(lastTimeShot + firingSpeed < Time.time)
        {
            lastTimeShot = Time.time;
            Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
        }
    }
}
