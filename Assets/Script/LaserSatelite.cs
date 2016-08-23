using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserSatelite : MonoBehaviour
{
    public float health = 300;
    public float dps = 1;
    public Transform centerPlanet;
    public float rotateSpeed;
    float radius;
    Material laserMat;
    List<GameObject> stayList;
    LineRenderer[] gunList;


    void Awake()
    {
        laserMat = Resources.Load<Material>("Material/LaserMaterial");
        rotateSpeed = Random.Range(8, 50);
        radius = GetComponent<CircleCollider2D>().radius;
        stayList = new List<GameObject>();
        gunList = transform.GetComponentsInChildren<LineRenderer>();
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
            centerPlanet.GetComponent<Planet>().laserList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    bool attackPermit = true;
    void enableAttack()
    {
        attackPermit = true;
    }
    void disableAttack()
    {
        attackPermit = false;
    }
    void FixedUpdate()
    {
        transform.RotateAround(centerPlanet.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        if(attackPermit) attack();
    }
    void attack()
    {
        foreach (var gun in gunList)
        {
            gun.SetPosition(1, Vector3.zero);
        }
        int usedIndex = 0;
        foreach (var ship in stayList)
        {
            if (ship == null) { stayList.Remove(ship); break; }
            RaycastHit2D hit = Physics2D.RaycastAll(transform.position, ship.transform.position - transform.position, radius)[1];
            if (hit.collider != null && hit.collider.gameObject.GetInstanceID() == ship.GetInstanceID())
            {
                if (usedIndex < gunList.Length)
                {
                    ship.SendMessage("ApplyDamage", dps);//dps
                    gunList[usedIndex++].SetPosition(1, transform.worldToLocalMatrix * (ship.transform.position - transform.position));
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Ship" && coll.GetComponent<Ship>().groupID != centerPlanet.GetComponent<Planet>().groupID)
        {
            stayList.Add(coll.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Ship" && coll.GetComponent<Ship>().groupID != centerPlanet.GetComponent<Planet>().groupID)
        {
            stayList.Remove(coll.gameObject);
        }
    }
}
