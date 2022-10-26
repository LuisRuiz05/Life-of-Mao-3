using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This script describes the bullet's behaviour.
/// </summary>
public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;
    public GameObject blood;

    private Rigidbody rb;
    private GameObject bloodParent;

    private float speed = 75f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void Start()
    {
        bloodParent = GameObject.Find("BloodParent");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    ///     Once the bullet colides, it will be destroyes and will instantiate the bullet's decal.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.sqrMagnitude == 0f || rb.IsSleeping())
        {
            Debug.Log("It stopped");
            Destroy(gameObject);
        }

        // Bullet colliding against zombie's body.
        if (collision.collider.CompareTag("Enemy"))
        {
            ContactPoint contact = collision.GetContact(0);

            Destroy(gameObject);
            if (collision.collider.gameObject.GetComponent<ZombieIA>().isAlive)
            {
                GameObject bloodCopy = GameObject.Instantiate(blood, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal), bloodParent.transform);
                Destroy(bloodCopy, 6f);
            }
            collision.collider.gameObject.GetComponent<ZombieIA>().ReceiveDamage(10);
        }
        // Bullet colliding against zombie's head.
        else if (collision.collider.CompareTag("Z_Head"))
        {
            ContactPoint contact = collision.GetContact(0);

            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
            if (collision.collider.gameObject.GetComponent<ZombieHead>().ai.isAlive)
            {
                GameObject bloodCopy = GameObject.Instantiate(blood, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal), bloodParent.transform);
                Destroy(bloodCopy, 6f);
            }
            collision.collider.gameObject.GetComponent<ZombieHead>().ai.ReceiveDamage(100);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
