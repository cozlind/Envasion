using UnityEngine;
using System.Collections;

public class Satelite : MonoBehaviour {

    public Transform centerPlanet;
    public float rotateSpeed;

    void Start()
    {
        //register in center planet
        if (centerPlanet.GetComponent<Planet>() != null)
        {
            centerPlanet.GetComponent<Planet>().satelites.Add(transform);
        }
        rotateSpeed = Random.Range(8, 50);
    }

    void Update()
    {
        transform.RotateAround(centerPlanet.position, Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
