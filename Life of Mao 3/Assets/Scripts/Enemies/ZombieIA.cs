using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     This script handles the zombie's AI.
/// </summary>
public class ZombieIA : MonoBehaviour
{
    private NavMeshAgent nav;
    private Animator animator;
    public RewardsLoader rewards;

    public int health = 100;
    public bool isAlive;

    // Zombie's Vision
    public float distance = 10;
    public float angle = 30;
    public float height = 1.7f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> objects = new List<GameObject>();

    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;

    // Wander
    public float wanderDistance = 50f;
    private float wanderTimer;
    public float wanderMaxTime = 6f;

    // Attack
    private float damageTimer;
    private float damageCooldown = 0.45f;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.isStopped = false;
        animator = GetComponent<Animator>();

        wanderTimer = wanderMaxTime;
        scanInterval = 1.0f / scanFrequency;
    }

    void Update()
    {
        if (isAlive)
        {
            // Follow player if it's detected.
            GameObject follow = FindPlayer();
            if (follow != null)
            {
                nav.speed = 1.25f;
                nav.SetDestination(follow.transform.position);
                animator.SetBool("Walking", false);
                animator.SetBool("Running", true);
                animator.SetBool("Attacking", false);
            }
            // Id the player is not detected, wander randomly all over the map.
            else
            {
                // Wander
                nav.speed = 1f;
                animator.SetBool("Walking", true);
                animator.SetBool("Running", false);
                animator.SetBool("Attacking", false);
                wanderTimer += Time.deltaTime;

                if (wanderTimer >= wanderMaxTime)
                {
                    nav.SetDestination(Wander(transform.position, wanderDistance, 7));
                    wanderTimer = 0;
                }
            }

            // Attack
            if (follow != null && (transform.position - follow.transform.position).magnitude <= 1.3)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                animator.SetBool("Attacking", true);
                damageTimer += Time.deltaTime;

                if (damageTimer >= damageCooldown)
                {
                    follow.GetComponent<PlayerState>().currentHealth -= 5;
                    follow.GetComponent<Animator>().Play("Hurt");
                    damageTimer = 0;
                }
            }

            // Regular scan.
            scanTimer -= Time.deltaTime;
            if (scanTimer < 0)
            {
                scanTimer += scanInterval;
                Scan();
            }
        }
    }

    /// <summary>
    ///     Receives damage.
    /// </summary>
    /// <param name="damage"> Quantity of damage received, which will be substracted fromthe total health. </param>
    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && isAlive)
        {
            isAlive = false;
            nav.isStopped = true;
            rewards.zombiesKilled++;
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetBool("Attacking", false);
            animator.Play("Die");

            Destroy(gameObject, 1.5f);
        }
    }

    /// <summary>
    ///     Checks if there's a followable object in the zombie's vision area.
    /// </summary>
    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        objects.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                objects.Add(obj);
            }
        }
    }

    /// <summary>
    ///     Checks if the followable in the zombie's vision area is obstructed by another object, so the zombie doesn't "see" a player behind another object.
    /// </summary>
    /// <param name="obj"> Followable object </param>
    /// <returns></returns>
    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < -1.0f || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if(deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if(Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    ///     Draws a mesh preview of the zombie's vision field in the debug screen.
    /// </summary>
    /// <returns> Zombie's vision field </returns>
    Mesh CreateVisionMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        // Left Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // Right Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            // Far Side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            
            currentAngle += deltaAngle;
        }
       

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateVisionMesh();
    }

    /// <summary>
    ///     If there's one or more players in the zombie's vision, will get the closest one.
    /// </summary>
    /// <returns> Nearest visible player </returns>
    GameObject FindPlayer()
    {
        if (objects.Count > 0)
        {
            return objects[0];
        }
        return null;
    }
    
    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);   
        }

        Gizmos.color = Color.green;
        foreach (var obj in objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }

    public static Vector3 Wander(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}