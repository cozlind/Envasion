using UnityEngine;
using System.Collections;

public class LaserGun : Gun
{
    [Header("Time Control")]
    public float dps = 1;
    int groupID;
    LineRenderer gun1;
    LineRenderer gun2;
    Material laserMat;
    void Awake()
    {
        laserMat = Resources.Load<Material>("Material/LaserMaterial");
        gun1 = transform.FindChild("Gun1").GetComponent<LineRenderer>();
        gun2 = transform.FindChild("Gun2").GetComponent<LineRenderer>();
    }
    void Start()
    {
        groupID = GetComponent<Ship>().groupID;
        dps =Mathf.Lerp(0.5f,2.5f,GetComponent<Ship>().strength);
    }
    public override void attack()
    {
        gun1.SetPosition(1, Vector3.zero);
        gun2.SetPosition(1, Vector3.zero);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + transform.up * 0.3f, transform.GetComponent<EdgeCollider2D>().bounds.extents.y * 2f, transform.up, 4, 1 << LayerMask.NameToLayer("Attackable"));
        if (hits.Length > 0)
        {
            GameObject attackTarget = null;
            foreach (var hit in hits)
            {
                if (hit.collider.tag == "ShieldPoint" && hit.collider.GetComponent<ShieldPoint>().centerPlanet.GetComponent<Planet>().groupID != groupID)
                {
                    attackTarget = hit.collider.gameObject;
                    break;
                }
                else if ((hit.collider.tag == "Ship" && hit.collider.GetComponent<Ship>().groupID != groupID)
                    || (hit.collider.tag == "LaserSatelite" && hit.collider.GetComponent<LaserSatelite>().centerPlanet.GetComponent<Planet>().groupID != groupID)
                    || (hit.collider.tag == "Building" && hit.collider.GetComponent<SpaceLadder>().centerPlanet.GetComponent<Planet>().groupID != groupID))
                {
                    attackTarget = hit.collider.gameObject;
                }
            }
            if (attackTarget != null)
            {
                attackTarget.SendMessage("ApplyDamage", dps);//dps
                gun1.SetPosition(1, transform.worldToLocalMatrix * (attackTarget.transform.position - gun1.transform.position));
                gun2.SetPosition(1, transform.worldToLocalMatrix * (attackTarget.transform.position - gun2.transform.position));
            }
        }
    }
}