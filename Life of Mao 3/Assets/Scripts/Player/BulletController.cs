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
    public GameObject flesh;

    private float speed = 50f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
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
        Debug.Log(collision.collider.name);
        //ContactPoint contact = collision.GetContact(0);
        //GameObject decal = GameObject.Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
        if (collision.collider.CompareTag("Enemy"))
        {
            ContactPoint contact = collision.GetContact(0);
            GameObject bloodCopy = GameObject.Instantiate(blood, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal), collision.gameObject.transform);
            collision.collider.gameObject.GetComponent<ZombieIA>().ReceiveDamage(10);

            Destroy(gameObject);
            Destroy(bloodCopy, 2f);
        }
        else if (collision.collider.CompareTag("Z_Head")){
            collision.collider.gameObject.GetComponent<ZombieHead>().ai.ReceiveDamage(100);

            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }
        else
        {
            ContactPoint contact = collision.GetContact(0);
            GameObject decal = GameObject.Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject);
            Destroy(decal, 6f);
        }
    }
}
