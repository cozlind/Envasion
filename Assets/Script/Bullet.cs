using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed = 5;
    public float acceleration = 5;
    public GameObject target;
    public float damage=5;
    public void SetData(GameObject t,float d)
    {
        target = t;
        damage = d;
    }
    void Start()
    {
        Invoke("DestroySelf", 10);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        Vector3 velocity = transform.up * speed * Time.deltaTime;
        if (target != null)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, target.transform.position) > 0.2f)
            {
                velocity += dir * acceleration*Time.deltaTime;
            }
        }
        transform.up = velocity;
        transform.Translate(velocity, Space.World);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.isTrigger == true) return;//deal with the laserSatelite
        if (coll.gameObject==target)
        {
            coll.gameObject.SendMessage("ApplyDamage", damage);
            Destroy(gameObject);
        }
    }
}
