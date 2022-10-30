using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     This script handles the zombie's AI.
/// </summary>
public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent nav;
    private Animator animator;
    public RewardsLoader rewards;

    public int health = 100;
    public bool isAlive = true;
    public bool follow = false;

    public GameObject player;

    // Wander
    public float wanderDistance = 30f;
    private float wanderTimer;
    public float wanderMaxTime = 6f;

    // Attack
    private float damageTimer;
    private float damageCooldown = 0.45f;

    //Audio
    private float audioCooldown = 0.0f;
    private AudioSource audio;
    public AudioClip growl; //Moan
    public AudioClip growl2; //Hiss 
    public AudioClip hurt; //Grunt
    public AudioClip die; //Hyperchase

    //Instantiate loot
    public GameObject food;
    public GameObject water;
    public GameObject medkit;
    public GameObject pills;
    public GameObject pistolAmmo;
    public GameObject uziAmmo;
    public GameObject riffleAmmo;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.isStopped = false;
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wanderTimer = wanderMaxTime;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isAlive)
        {
            Growl();
            FindPlayer();
            // Follow player if it's detected.
            if (follow)
            {
                FollowPlayer();
            }
            // Id the player is not detected, wander randomly all over the map.
            else
            {
                DoWander();
            }

            // Attack
            if (follow && (transform.position - player.transform.position).magnitude <= 1.3)
            {
                Attack();
            }
        }
    }

    public void Growl()
    {
        audioCooldown += Time.deltaTime;
        if(audioCooldown >= 5.0f && !audio.isPlaying)
        {
            audio.PlayOneShot(Random.Range(0, 2) == 0 ? growl : growl2);
            audioCooldown = 0.0f;
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
            audio.PlayOneShot(die);

            Destroy(gameObject, 1.5f);
        }
        else
            audio.PlayOneShot(hurt);
    }

    /// <summary>
    ///     Will check if the player's closer than the defined range.
    /// </summary>
    void FindPlayer()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (distance <= 25.0f)
            follow = true;
        else
            follow = false;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <returns></returns>
    public static Vector3 Wander(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    void DoWander()
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

    void FollowPlayer()
    {
        nav.speed = 2.75f;
        nav.SetDestination(player.transform.position);
        animator.SetBool("Walking", false);
        animator.SetBool("Running", true);
        animator.SetBool("Attacking", false);
    }

    void Attack()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
        animator.SetBool("Attacking", true);
        damageTimer += Time.deltaTime;

        if (damageTimer >= damageCooldown)
        {
            player.GetComponent<PlayerState>().currentHealth -= 5;
            player.GetComponent<Animator>().Play("Hurt");
            damageTimer = 0;
        }
    }

}