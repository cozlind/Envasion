using UnityEngine;
using System.Collections;
using System;
public class SpaceLadder : MonoBehaviour
{

    [NonSerialized]
    public int level = 0;
    int groupID;
    public Transform centerPlanet;
    public float health = 300;
    Transform spawnPoint;
    GameObject shipPfb;
    GameObject laserShipPfb;
    void Awake()
    {
        shipPfb = Resources.Load<GameObject>("Prefabs/Ship");
        laserShipPfb = Resources.Load<GameObject>("Prefabs/LaserShip");
        spawnPoint = transform.FindChild("SpawnPoint");
        InvokeRepeating("spawn", 4, 4);
    }
    void Start()
    {
        groupID = centerPlanet.GetComponent<Planet>().groupID;
    }
    void spawn()
    {
        if (centerPlanet.GetComponent<Planet>().getShipNum(groupID) < 40)
        {
            switch (level)
            {
                case 0:
                    {
                        GameObject ship = Instantiate(shipPfb, spawnPoint.position, Quaternion.identity) as GameObject;
                        if (GetComponent<Renderer>().enabled == false)//when it's hidden
                        {
                            ship.GetComponent<Renderer>().enabled = false;
                            ship.SendMessage("disableAttack");
                        }
                        Ship s = ship.GetComponent<Ship>();
                        s.centerPlanet = centerPlanet;
                        s.groupID = groupID;
                        s.health = centerPlanet.GetComponent<Planet>().vitality / 10;
                        s.strength = centerPlanet.GetComponent<Planet>().strength / 100;
                        s.agility = Mathf.Lerp(2f, 6f - 1.5f, centerPlanet.GetComponent<Planet>().agility / 1000);
                        break;
                    }
                case 1:
                    {
                        GameObject ship = Instantiate(laserShipPfb, spawnPoint.position, Quaternion.identity) as GameObject;
                        if (GetComponent<Renderer>().enabled == false)//when it's hidden
                        {
                            ship.GetComponent<Renderer>().enabled = false;
                            ship.SendMessage("disableAttack");
                        }
                        Ship s = ship.GetComponent<Ship>();
                        s.centerPlanet = centerPlanet;
                        s.groupID = groupID;
                        s.health = centerPlanet.GetComponent<Planet>().vitality / 10f;
                        s.strength = centerPlanet.GetComponent<Planet>().strength / 1000f;
                        s.agility = Mathf.Lerp(2f, 6f - 1.5f, centerPlanet.GetComponent<Planet>().agility / 1000f);
                        break;
                    }
            }
        }
    }
    void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            centerPlanet.GetComponent<Planet>().spaceLadderList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
