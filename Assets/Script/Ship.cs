using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    [Header("Basic Attribute")]
    public int groupID;//the group belonged to 
    public float strength;
    public float agility;
    public float health;
    public float speed;
    Gun gun;

    [Header("Dynamic Attribute")]
    public Transform centerPlanet;
    public Transform moveTarget;
    void Awake()
    {
        gun = GetComponent<Gun>();
    }
    void Start()
    {
        minSpeed = agility;
        maxSpeed = agility + 1.5f;
        newPlanet();
        if (centerPlanet.GetComponent<Planet>() != null)
        {
            GetComponent<SpriteRenderer>().color = centerPlanet.GetComponent<SpriteRenderer>().sprite.texture.GetPixel(256, 256);
        }
        InvokeRepeating("speedChange", 0, 2);
    }
    public void newPlanet()
    {
        //register in center planet
        if (centerPlanet.GetComponent<Planet>() != null)
        {
            centerPlanet.GetComponent<Planet>().ships.Add(transform);
        }
        minOrbit = centerPlanet.GetComponent<CircleCollider2D>().radius * centerPlanet.localScale.x * 1.32f;
        maxOrbit = centerPlanet.GetComponent<CircleCollider2D>().radius * centerPlanet.localScale.x * 2.34f;
    }
    void speedChange()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }
    public Vector3 velocity;
    Vector3 initialVelocity;
    Vector3 targetVelocity;
    //datum point 3.5~4.5 extend range 2~6
    float minSpeed = 2f;
    float maxSpeed = 6f;
    float gravity = 0.08f;
    float minOrbit;
    float maxOrbit;
    float lowAcceleration = 0.2f;
    float highAcceleration = -0.15f;
    float shearAcceleration = 0.3f;
    float targetAcceleration = 0.1f;

    bool attackPermit = true;
    void enableAttack()
    {
        attackPermit = true;
    }
    void disableAttack()
    {
        attackPermit = false;
    }
    void Update()
    {
        move();
        if (attackPermit)  gun.attack();
    }
   
    void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            if (centerPlanet.GetComponent<Planet>() != null)
            {
                centerPlanet.GetComponent<Planet>().ships.Remove(transform);
            }
        }
    }

    void move()
    {
        Vector3 radialDir = (transform.position - centerPlanet.position).normalized;
        initialVelocity = transform.up * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, centerPlanet.position) < minOrbit)//close the planet then bounce
        {
            velocity = initialVelocity + lowAcceleration * radialDir * Time.deltaTime;
        }
        else if (Vector3.Distance(transform.position, centerPlanet.position) > maxOrbit)//far away from planet then attracked
        {
            velocity = initialVelocity + highAcceleration * radialDir * Time.deltaTime;
        }
        else//affected by gravity
        {
            velocity = initialVelocity - gravity * radialDir * Time.deltaTime;
        }
        //when rush from the far point
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1, 1 << LayerMask.NameToLayer("Planet"));
        if (hit.collider != null)
        {
            radialDir = (transform.position - hit.transform.position).normalized;
            velocity = initialVelocity + lowAcceleration * radialDir * Time.deltaTime + (radialDir + transform.up) * shearAcceleration;
        }

        if (moveTarget != null)
        {
            Vector3 targetDir = (moveTarget.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, centerPlanet.position) > 3)//far away from the target then attracked
            {
                velocity += targetDir * Time.deltaTime * targetAcceleration;
            }
        }
        transform.up = velocity;
        transform.Translate(velocity, Space.World);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.3f, transform.GetComponent<EdgeCollider2D>().bounds.extents.y * 2f);
        Gizmos.DrawWireSphere(transform.position + transform.up * 4.3f, transform.GetComponent<EdgeCollider2D>().bounds.extents.y * 2f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.up * 1 + transform.position);
    }
}
