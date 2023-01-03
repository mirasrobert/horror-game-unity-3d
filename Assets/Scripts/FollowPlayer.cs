using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public Transform raycast; // Raycast
    public Transform raycast2; // Raycast
    public Transform raycast3; // Raycast
    public Transform raycast4; // Raycast
    public Transform raycast5; // Raycast

    public float attackRange = 2.5f;

    // Objects enemy transform
    Transform transform;

    // Agent
    NavMeshAgent nav;

    // Animator
    Animator animator;

    // Array of waypoints
    public Transform[] waypoints;

    Vector3 target;

    int waypointIndex;

    public bool needToChangeRoute = false;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if ((Physics.Raycast(raycast.position, raycast.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) ||
            (Physics.Raycast(raycast2.position, raycast2.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) ||
            (Physics.Raycast(raycast3.position, raycast3.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) || 
            (Physics.Raycast(raycast4.position, raycast4.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) ||
            (Physics.Raycast(raycast5.position, raycast5.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) )
        {
            if (hit.transform.CompareTag("Player"))
            {

                // Player object
                Transform player = hit.transform;
                ChasePlayer(player);

                Debug.Log("Chase Player");
                Debug.DrawRay(raycast.position, raycast.TransformDirection(Vector3.forward) * 100f, Color.green);
                Debug.DrawRay(raycast2.position, raycast2.TransformDirection(Vector3.forward) * 100f, Color.green);
                Debug.DrawRay(raycast3.position, raycast3.TransformDirection(Vector3.forward) * 100f, Color.green);
                Debug.DrawRay(raycast4.position, raycast4.TransformDirection(Vector3.forward) * 100f, Color.green);
                Debug.DrawRay(raycast5.position, raycast5.TransformDirection(Vector3.forward) * 100f, Color.green);

            }
            else
            {
                Debug.Log("No Hit Player");
                if (Vector3.Distance(transform.position, target) < 1)
                {
                    IterateWaypointIndex();
                    UpdateDestination();
                } else
                {
                    UpdateDestination();
                }
 
                Debug.DrawRay(raycast.position, raycast.TransformDirection(Vector3.forward) * 100f, Color.red);
                Debug.DrawRay(raycast2.position, raycast2.TransformDirection(Vector3.forward) * 100f, Color.red);
                Debug.DrawRay(raycast3.position, raycast3.TransformDirection(Vector3.forward) * 100f, Color.red);
                Debug.DrawRay(raycast4.position, raycast4.TransformDirection(Vector3.forward) * 100f, Color.red);
                Debug.DrawRay(raycast5.position, raycast5.TransformDirection(Vector3.forward) * 100f, Color.red);


            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target) < 1)
            {
                IterateWaypointIndex();
                UpdateDestination();
            } else
            {
                UpdateDestination();
                Debug.Log("No Hit");
            }

            Debug.DrawRay(raycast.position, raycast.TransformDirection(Vector3.forward) * 100, Color.red);
            Debug.DrawRay(raycast2.position, raycast2.TransformDirection(Vector3.forward) * 100, Color.red);
            Debug.DrawRay(raycast3.position, raycast3.TransformDirection(Vector3.forward) * 100, Color.red);
            Debug.DrawRay(raycast4.position, raycast4.TransformDirection(Vector3.forward) * 100, Color.red);
            Debug.DrawRay(raycast5.position, raycast5.TransformDirection(Vector3.forward) * 100, Color.red);

        }
    }

    public void ChasePlayer(Transform player)
    {
        nav.isStopped = false;
        nav.SetDestination(player.position);

        float distance = Vector3.Distance(player.position, transform.position);

        // If in attack range
        if (distance < attackRange)
        {
            Debug.Log("Attack Range");
            animator.SetTrigger("Attack");
        }
    }

    void StopChasingPlayer()
    {
        nav.isStopped = true;
    }

    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        nav.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        // Generate Random Number
        int randomWaypoints = Random.Range(0, waypoints.Length);

        // Avoid same waypoint
        if (randomWaypoints != waypointIndex)
        {
            waypointIndex = randomWaypoints;
        }
        else
        { 
            // Generate Random Again
            IterateWaypointIndex();
        }
    }
}
