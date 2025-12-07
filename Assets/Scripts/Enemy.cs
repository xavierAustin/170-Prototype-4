using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : Pickup
{
    [Header("References")]
    public Transform cube;
    //public Transform grabPoint;
    public GameObject particles;
    
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3f;
    public float waypointThreshold = 0.5f;
    
    [Header("Keep Away Settings")]
    public float detectionRange = 8f;
    public float fleeSpeed = 5f;
    public float preferredDistance = 6f;
    //public float catchDistance = 1.5f;
    
    [Header("Pickup Settings")]
    public float grabRange = 1.5f;

    [Header("Stats")]
    public int damage = 1;
    public int health = 6;
    public float windupTime = 2.3f;
    
    enum State { 
        Patrol,
        Pursue,
        Attack,
        GoToCube, 
        KeepAway 
    }
    State state = State.GoToCube;
    
    NavMeshAgent agent;
    Transform player;
    int currentPatrolIndex = 0;
    bool hasCube = false;
    Rigidbody cubeRb;
    bool runningCoroutine = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (cube)
            cubeRb = cube.GetComponent<Rigidbody>();
        
        isHeavy = true;
        agent.speed = patrolSpeed;
    }

    void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerNearby = distToPlayer < detectionRange;

        switch (state)
        {
            case State.Patrol:
                runningCoroutine = false;
                if (patrolPoints[currentPatrolIndex] == null || patrolPoints.Length == 0) return;
        
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                
                if (agent.remainingDistance <= waypointThreshold && !agent.pathPending)
                {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                }
                if (playerNearby && hasCube){
                    state = State.KeepAway;
                    agent.speed = fleeSpeed;
                }else if (playerNearby)
                    state = State.Pursue;
                break;
                
            case State.GoToCube:
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
                if (hasCube)
                    state = State.Patrol;
                break;
                
            case State.KeepAway:
                // Player caught us
                /*
                if (distToPlayer <= catchDistance)
                {
                    DropCube();
                    agent.speed = patrolSpeed;
                    state = State.Patrol;
                    return;
                }
                */
                
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
                if (!playerNearby)
                {
                    agent.speed = patrolSpeed;
                    state = State.Patrol;
                }
                break;
            case State.Pursue:
                runningCoroutine = false;
                agent.SetDestination(player.position);
                if (distToPlayer < grabRange)
                    state = State.Attack;
                if (!playerNearby)
                    state = State.Patrol;
            break;
            case State.Attack:
                StartCoroutine(Attack());
            break;
        }

        if (isHeld){
            health --;
            isHeld = false;
            player.gameObject.GetComponent<Player>().ForceDrop();
            var temp = Instantiate(particles);
            temp.transform.position = transform.position;
            state = State.Pursue;
            agent.speed = patrolSpeed;
        }
        if (health == 0){
            DropCube();
            Destroy(gameObject);
        }
    }

    IEnumerator Attack(){
        if (runningCoroutine)
            yield break;
        runningCoroutine = true;
        yield return new WaitForSeconds(windupTime);
        var distToPlayer = Vector3.Distance(transform.position,player.position);
        if (distToPlayer < grabRange)
            player.gameObject.GetComponent<Player>().Damage(damage);
        if (distToPlayer < detectionRange)
            state = State.Pursue;
        else 
            state = State.Patrol;
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

    void OnDrawGizmos()
    {
        for (int i = 0; i < patrolPoints.Length; i++){
            int j = (i + 1) % patrolPoints.Length;
            if (patrolPoints[i] == null || patrolPoints[j] == null)
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(patrolPoints[i].position,patrolPoints[j].position);
            Gizmos.color = Color.purple;
            Gizmos.DrawLine(patrolPoints[i].position,transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, preferredDistance);
        
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, catchDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}