using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

    public int groupID = 2;
    public Transform startPlanet;
    public List<Transform> planets;
    public List<Transform> myPlanets;
    Planet self;
    float startTime;

    void Awake()
    {
        self = GetComponent<Planet>();
        myPlanets = new List<Transform>();
        planets = new List<Transform>();
        Transform p = GameObject.Find("Planets").transform;
        for(int i = 0; i < p.childCount; i++)
        {
            planets.Add(p.GetChild(i));
        }
        
    }
    
    void Start()
    {
        self.buildSpaceLadder(groupID,true);
        myPlanets.Add(transform);
        self.generateShield();
        self.buildLaserSatelite();
        StartCoroutine("thinking");
    } 
    Transform getClosestPlanet()
    {
        Transform closeP=planets[0];
        float dist=1000;
        foreach(var p in planets)
        {
            if (p.GetComponent<Planet>().groupID != groupID)
            {
                foreach(var myP in myPlanets)
                {
                    if (Vector3.Distance(p.position, myP.position) < dist)
                    {
                        dist = Vector3.Distance(p.position, myP.position);
                        closeP = p;
                    }
                }
            }
        }
        return closeP;
    }
    IEnumerator thinking()
    {
        startTime = Time.time;
        while (true)
        {
            if (self.spaceLadderList.Count < self.maxSpaceLadder-2)
            {
                if (self.getShipNum(groupID, 10) < 15) yield return new WaitForSeconds(1);
                self.buildSpaceLadder(groupID);
                yield return new WaitForSeconds(1);
            }
            if (self.getShipNum(groupID) > 20)
            {
                GameManager.Instance.moveTo(self.groupID, transform, getClosestPlanet());
            }
            yield return null;
        }
    }
}
