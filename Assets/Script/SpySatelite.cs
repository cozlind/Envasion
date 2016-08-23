using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpySatelite : MonoBehaviour
{
    public float health = 300;
    public Transform centerPlanet;
    public float rotateSpeed;


    void Awake()
    {
        rotateSpeed = Random.Range(8, 50);
    }
    void Start()
    {
        //register in center planet
        if (centerPlanet.GetComponent<Planet>() != null)
        {
            centerPlanet.GetComponent<Planet>().satelites.Add(transform);
        }
    }
    void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            centerPlanet.GetComponent<Planet>().spySatelite = null;
            Destroy(gameObject);
        }
    }


    void FixedUpdate()
    {
        transform.RotateAround(centerPlanet.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        
    }
}
