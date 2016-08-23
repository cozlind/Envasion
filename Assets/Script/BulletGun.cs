using UnityEngine;
using System.Collections;

public class BulletGun : Gun {

    public float damage;
    public float attackThreshold = 2;
    float lastAttacktime1;
    float lastAttacktime2;
    float groupID;
    Transform gun1;
    Transform gun2;
    GameObject bulletPref;
    void Awake()
    {
        bulletPref = Resources.Load<GameObject>("Prefabs/Bullet");
        gun1 = transform.FindChild("Gun1");
        gun2 = transform.FindChild("Gun2");
    }
    void Start()
    {
        groupID = GetComponent<Ship>().groupID;
        damage = Mathf.Lerp(4f,8f, GetComponent<Ship>().strength);
    }
    public override void attack()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + transform.up * 0.3f, transform.GetComponent<EdgeCollider2D>().bounds.extents.y * 2f, transform.up, 4, 1 << LayerMask.NameToLayer("Attackable"));
        if (hits.Length > 0 && (lastAttacktime1 + attackThreshold < Time.time || lastAttacktime2 + attackThreshold < Time.time))
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
                    ||(hit.collider.tag == "Building" && hit.collider.GetComponent<SpaceLadder>().centerPlanet.GetComponent<Planet>().groupID != groupID))
                {
                    //Debug.Log(hit.collider.name + ":" + hit.collider.GetComponent<Ship>().groupID + ":" + gameObject.name + ":" + groupID);
                    attackTarget = hit.collider.gameObject;
                }
            }
            if (attackTarget != null)
            {
                if (lastAttacktime1 + attackThreshold < Time.time)
                {
                    GameObject bullet = Instantiate(bulletPref, gun1.transform.position, transform.rotation) as GameObject;
                    bullet.GetComponent<Bullet>().SetData(attackTarget, damage);
                    lastAttacktime1 = Time.time;
                    lastAttacktime2 = Random.Range(Time.time - attackThreshold, Time.time - attackThreshold / 2);
                }
                if (lastAttacktime2 + attackThreshold < Time.time)
                {
                    GameObject bullet = Instantiate(bulletPref, gun2.transform.position, transform.rotation) as GameObject;
                    bullet.GetComponent<Bullet>().SetData(attackTarget, damage);
                    lastAttacktime2 = Time.time;
                }
            }
        }
    }
}
