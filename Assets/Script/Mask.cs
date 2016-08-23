using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Mask : MonoBehaviour
{
    public Transform background;
    public Transform startPlanet;
    
    void Awake()
    {
        transform.position = background.position;
        Vector2 size = background.GetComponent<Renderer>().bounds.size;
        Vector2 previousSize = GetComponent<Renderer>().bounds.size;
        transform.localScale = new Vector3(size.x / previousSize.x, size.y / previousSize.y, 1);
        startPlanet.SendMessage("OpenSight", 10);
    }
}
