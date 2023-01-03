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

    // Objects transform
    Transform transform;

    NavMeshAgent nav;

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
                Debug.DrawRay(raycast.position, raycast.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                Debug.DrawRay(raycast2.position, raycast2.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                Debug.DrawRay(raycast3.position, raycast3.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                Debug.DrawRay(raycast4.position, raycast4.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                Debug.DrawRay(raycast5.position, raycast5.TransformDirection(Vector3.forward) * hit.distance, Color.green);

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
 
                Debug.DrawRay(raycast.position, raycast.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.DrawRay(raycast2.position, raycast2.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.DrawRay(raycast3.position, raycast3.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.DrawRay(raycast4.position, raycast4.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.DrawRay(raycast5.position, raycast5.TransformDirection(Vector3.forward) * hit.distance, Color.red);


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

            Debug.DrawRay(raycast.position, raycast.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.DrawRay(raycast2.position, raycast2.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.DrawRay(raycast3.position, raycast3.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.DrawRay(raycast4.position, raycast4.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.DrawRay(raycast5.position, raycast5.TransformDirection(Vector3.forward) * 1000, Color.red);

        }
    }

    public void ChasePlayer(Transform player)
    {
        nav.isStopped = false;
        nav.SetDestination(player.position);
    }

    void StopChasingPlayer()
    {
        nav.isStopped = true;
    }

    void GoToWaypoints()
    {
        // Generate Random Number
        int randomWaypoints = Random.Range(0, (waypoints.Length - 1));

        // Generate Random Waypoint To Go
        Transform waypointToGo = waypoints[randomWaypoints];

        if(needToChangeRoute)
        {
            // Go to the destination
            nav.isStopped = false;
            nav.SetDestination(waypointToGo.position);
        } else
        {
            nav.isStopped = false;
            nav.SetDestination(waypoints[2].position);
        }

        CheckIfReachedDestination();
    }

    void CheckIfReachedDestination()
    {
        // Check if we've reached the destination
        if (!nav.pathPending)
        {
            if (nav.remainingDistance <= nav.stoppingDistance)
            {
                if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
                {
                    needToChangeRoute = true;
                    Debug.Log("has reached");
                }
            }
        }

        Debug.Log("Not reached yet");
        needToChangeRoute = false;
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

        /* 
            // check if last way point 
            if (waypointIndex == waypoints.Length)
            {
                // Restart waypoint
                waypointIndex = 0;
            }
         */

    }
}
