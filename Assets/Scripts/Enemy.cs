using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform cube;
    public Transform grabPoint;
    
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3f;
    public float waypointThreshold = 0.5f;
    
    [Header("Keep Away Settings")]
    public float detectionRange = 8f;
    public float fleeSpeed = 5f;
    public float preferredDistance = 6f;
    public float catchDistance = 1.5f;
    
    [Header("Pickup Settings")]
    public float grabRange = 1.5f;
    
    enum State { Patrol, GoToCube, KeepAway }
    State state = State.Patrol;
    
    NavMeshAgent agent;
    Transform player;
    int currentPatrolIndex = 0;
    bool hasCube = false;
    Rigidbody cubeRb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (cube)
            cubeRb = cube.GetComponent<Rigidbody>();
        
        agent.speed = patrolSpeed;
    }

    void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerNearby = distToPlayer < detectionRange;

        switch (state)
        {
            case State.Patrol:
                Patrol();
                if (playerNearby && cube != null && !hasCube)
                    state = State.GoToCube;
                break;
                
            case State.GoToCube:
                GoToCube();
                if (!playerNearby && !hasCube)
                    state = State.Patrol;
                break;
                
            case State.KeepAway:
                KeepAway();
                if (!playerNearby)
                {
                    agent.speed = patrolSpeed;
                    state = State.Patrol;
                }
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        
        if (agent.remainingDistance <= waypointThreshold && !agent.pathPending)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void GoToCube()
    {
        if (cube == null)
        {
            state = State.Patrol;
            return;
        }
        
        agent.SetDestination(cube.position);
        
        if (Vector3.Distance(transform.position, cube.position) <= grabRange)
        {
            GrabCube();
            agent.speed = fleeSpeed;
            state = State.KeepAway;
        }
    }

    void KeepAway()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Player caught us
        if (distToPlayer <= catchDistance)
        {
            DropCube();
            agent.speed = patrolSpeed;
            state = State.Patrol;
            return;
        }
        
        // Only move if player is too close
        if (distToPlayer < preferredDistance)
        {
            Vector3 dirFromPlayer = (transform.position - player.position).normalized;
            Vector3 fleeTarget = transform.position + dirFromPlayer * (preferredDistance - distToPlayer + 2f);
            
            if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, preferredDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            // Stop and taunt from safe distance
            agent.ResetPath();
        }
    }

    void GrabCube()
    {
        if (hasCube || cube == null) return;
        
        hasCube = true;
        
        if (cubeRb)
            cubeRb.isKinematic = true;
        
        cube.SetParent(grabPoint ? grabPoint : transform);
        cube.localPosition = Vector3.forward * 1.5f + Vector3.up * 0.5f;
        cube.localRotation = Quaternion.identity;
    }

    public void DropCube()
    {
        if (!hasCube || cube == null) return;
        
        hasCube = false;
        cube.SetParent(null);
        
        if (cubeRb)
            cubeRb.isKinematic = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, preferredDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRange);
        
        if (patrolPoints == null) return;
        Gizmos.color = Color.blue;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null) continue;
            Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);
            if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
        }
    }
}