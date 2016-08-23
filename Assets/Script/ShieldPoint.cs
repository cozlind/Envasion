using UnityEngine;
using System.Collections;

public class ShieldPoint : MonoBehaviour {

    public float health =100;
    public GameObject centerPlanet;
    
    public void setCenterPlanet(GameObject c)
    {
        centerPlanet = c;
    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            centerPlanet.GetComponent<Planet>().shieldPointList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
