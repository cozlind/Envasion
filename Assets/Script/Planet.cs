using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoBehaviour
{

    [Header("Basic Attribute")]
    public int groupID;
    public string name;
    public int maxOrbit;
    public int productivity;
    public float health;

    [Header("Second Attribute")]
    public int maxSight;
    public float strength;
    public float agility;
    public float vitality;

    [Header("Round Attribute")]
    public List<Transform> satelites;
    public List<Transform> ships;
    [NonSerialized]
    public GameObject hover;
    GameObject shieldPfb;
    GameObject spaceLadderPfb;
    GameObject laserSatelitePfb;
    GameObject spySatelitePfb;
    float shieldNumFactor = 2.1f;
    float shieldRadiusFactor = 1.7f;
    float laserRadiusFactor = 1.5f;
    float spyRadiusFactor = 2f;
    public int maxShip = 100;
    public int maxSpaceLadder = 5;
    int maxLaser = 2;
    int maxSpy = 1;
    public List<GameObject> spaceLadderList;
    public List<GameObject> laserList;
    public List<GameObject> shieldPointList;
    public GameObject spySatelite;
    float radius;

    //temp use
    Sprite sprite0, sprite1, sprite2, sprite3;

    
    void Update()
    {
        if (laserList.Count == 0 && shieldPointList.Count == 0 && spaceLadderList.Count == 0)
        {
            groupID = 0;//clear to a blank planet
            GetComponent<SpriteRenderer>().sprite = sprite0;
        }
        if (groupID != 1 && getShipNum(1, maxSight) == 0)
        {
            disableSee();
        }
        else
        {
            enableSee();
        }
    }
    public int getShipNum(int id, float radius = float.PositiveInfinity)
    {
        int myShipNum = 0;
        foreach (var ship in ships)
        {
            if (ship.GetComponent<Ship>().groupID == id && Vector3.Distance(ship.position, transform.position) < radius)
            {
                myShipNum++;
            }
        }
        return myShipNum;
    }
    public int getShipNum(int id, out int enemyShipNum, float radius = float.PositiveInfinity)
    {
        int myShipNum = 0;
        foreach (var ship in ships)
        {
            if (ship.GetComponent<Ship>().groupID == id && Vector3.Distance(ship.position, transform.position) < radius)
            {
                myShipNum++;
            }
        }
        enemyShipNum = ships.Count - myShipNum;
        return myShipNum;
    }
    void Awake()
    {
        sprite0 = Resources.Load<Sprite>("Sprites/planet5");
        sprite1 = Resources.Load<Sprite>("Sprites/planet1");
        sprite2 = Resources.Load<Sprite>("Sprites/planet6");
        sprite3 = Resources.Load<Sprite>("Sprites/planet2");




        spaceLadderList = new List<GameObject>();
        laserList = new List<GameObject>();
        satelites = new List<Transform>();
        shieldPointList = new List<GameObject>();
        ships = new List<Transform>();
        hover = transform.FindChild("hover").gameObject;
        hover.SetActive(false);
        shieldPfb = Resources.Load<GameObject>("Prefabs/ShieldPoint");
        spaceLadderPfb = Resources.Load<GameObject>("Prefabs/SpaceLadder");
        laserSatelitePfb = Resources.Load<GameObject>("Prefabs/LaserSatelite");
        spySatelitePfb = Resources.Load<GameObject>("Prefabs/SpySatelite");

        radius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;

    }

    public void generateShield()
    {
        if (shieldPointList.Count > 0) return;
        int num = Mathf.CeilToInt(radius * shieldNumFactor);
        for (int i = 0; i < num; i++)
        {
            float theta = i * 2 * Mathf.PI / num;
            float x = radius * shieldRadiusFactor * Mathf.Cos(theta);
            float y = radius * shieldRadiusFactor * Mathf.Sin(theta);
            GameObject shieldPoint = Instantiate(shieldPfb, transform.position + new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            shieldPoint.GetComponent<ShieldPoint>().setCenterPlanet(gameObject);
            shieldPoint.transform.SetParent(transform);
            shieldPointList.Add(shieldPoint);
        }
    }
    public void buildSpaceLadder(int id,bool skip=false)
    {
        if (!skip)
        {
            if (getShipNum(id, 10) < 5) return;
            if (groupID == 0)
            {
                groupID = id;
                switch (id)
                {
                    case 1:
                        GetComponent<SpriteRenderer>().sprite = sprite1;
                        break;
                    case 2:
                        GetComponent<SpriteRenderer>().sprite = sprite2;
                        break;
                    case 3:
                        GetComponent<SpriteRenderer>().sprite = sprite3;
                        break;
                }
                if (id == 1) { GetComponent<PlanetSight>().OpenSight(maxSight); }
            }
            //use the ship to exchange for space ladder
            int num = 5;
            for (int i = 0; i < ships.Count;)
            {
                if (ships[i].GetComponent<Ship>().groupID == id)
                {
                    Destroy(ships[i].gameObject);
                    ships.RemoveAt(i);
                    num--;
                    if (num <= 0) break;
                }
                else
                {
                    i++;
                }
            }
        }
        if (spaceLadderList.Count < maxSpaceLadder)
        {
            Vector3 dir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            Vector3 pos = dir * radius + transform.position;
            GameObject spaceLadder = Instantiate(spaceLadderPfb, pos, Quaternion.identity) as GameObject;
            spaceLadder.transform.up = dir;
            spaceLadder.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().sprite.texture.GetPixel(256, 256);
            spaceLadder.GetComponent<SpaceLadder>().centerPlanet = gameObject.transform;
            spaceLadder.transform.SetParent(transform);
            spaceLadderList.Add(spaceLadder);
        }
    }
    public void levelUpSpaceLadder()
    {
        foreach (var sl in spaceLadderList)
        {
            if (sl.GetComponent<SpaceLadder>().level == 0)
            {
                sl.transform.localScale += Vector3.up * 0.5f;
                sl.GetComponent<SpaceLadder>().level = 1;
                return;
            }
        }
    }
    public void buildLaserSatelite()
    {

        if (laserList.Count < maxLaser)
        {
            Vector3 dir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            Vector3 pos = dir * radius * laserRadiusFactor + transform.position;
            GameObject laserSatelite = Instantiate(laserSatelitePfb, pos, Quaternion.identity) as GameObject;
            laserSatelite.transform.up = dir;
            laserSatelite.GetComponent<LaserSatelite>().centerPlanet = gameObject.transform;
            laserSatelite.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().sprite.texture.GetPixel(256, 256);
            laserSatelite.transform.SetParent(transform);
            laserList.Add(laserSatelite);
        }
    }
    public void buildSpySatelite()
    {
        if (spySatelite == null)
        {
            Vector3 dir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            Vector3 pos = dir * radius * spyRadiusFactor + transform.position;
            spySatelite = Instantiate(spySatelitePfb, pos, Quaternion.identity) as GameObject;
            spySatelite.transform.up = dir;
            spySatelite.GetComponent<SpySatelite>().centerPlanet = gameObject.transform;
            spySatelite.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().sprite.texture.GetPixel(256, 256);
            spySatelite.transform.SetParent(transform);
        }
    }

    public void enableSee()
    {
        GetComponent<PlanetSight>().OpenSight(maxSight);
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().enabled = true;
            //transform.GetChild(i).SendMessage("enableAttack");
        }
        foreach (var ship in ships)
        {
            if (ship.GetComponent<Ship>().groupID == groupID)
            {
                ship.GetComponent<Renderer>().enabled = true;
                // ship.SendMessage("enableAttack");
            }
        }
    }
    public void disableSee()
    {
        GetComponent<PlanetSight>().CloseSight();
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            //transform.GetChild(i).SendMessage("disableAttack");
        }
        foreach (var ship in ships)
        {
            if (ship.GetComponent<Ship>().groupID == groupID)
            {
                ship.GetComponent<Renderer>().enabled = false;
                //ship.SendMessage("disableAttack");
            }
        }
    }
}
