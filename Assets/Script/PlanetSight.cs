using UnityEngine;
using System.Collections;

public class PlanetSight : MonoBehaviour
{

    public Transform MaskPlane;
    public int Number = 1;

    // Update is called once per frame
    public void OpenSight(int radius)
    {
        MaskPlane.GetComponent<Renderer>().material.SetVector("Planet" + Number.ToString() + "Pos", transform.position);
    }
    public void CloseSight()
    {
        MaskPlane.GetComponent<Renderer>().material.SetVector("Planet" + Number.ToString() + "Pos", new Vector3(10000,0,0));
    }
}