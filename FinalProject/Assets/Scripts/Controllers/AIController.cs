using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller, IHear
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4f;
    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    public float rotateTime = 2f;

    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;
    [SerializeField] protected bool canFlee = false;
    [SerializeField] protected float enemyDistanceRun = 5.0f;
    [SerializeField] protected int runAttempts = 0;
    [SerializeField] protected int maxRunAttempts = 5;
    




    public Transform[] waypoints;
    protected int currentWayPointIndex;

    protected Vector3 playerLastPosition = Vector3.zero;
    protected Vector3 playerPosition;

    protected float waitTime;
    protected float timeToRotate;
    protected bool playerInRange;
    protected bool playerNear;
    [SerializeField] protected bool isPatrol;
    protected bool caughtPlayer;
    protected float _animationBlend;
    public float SpeedChangeRate = 10.0f;
    public bool notifySpawnerToRespawn = true;
    [SerializeField] protected float timeBetweenRunAttempts = 1.0f;

    //Patroling 
    protected bool walkPointSet;
    [SerializeField] protected float walkPointRange;
    [SerializeField] protected Vector3 walkPoint;
    [SerializeField] protected bool randomPatrol = false;
    [SerializeField] protected LayerMask groundMask;
    protected float viewCheckInterval = 0.5f; // Time interval between checks (in seconds)
    protected float lastViewCheckTime;



    // animation IDs
    protected int _animIDSpeed;
    protected int _animIDGrounded;
    protected int _animIDJump;
    protected int _animIDFreeFall;
    protected int _animIDMotionSpeed;
    protected int _animIDAttack;

    protected Animator _animator;
    protected bool _hasAnimator;
    protected bool heardSound = false;
    protected Vector3 soundLoc;

    protected virtual void Start()
    {
        playerPosition = Vector3.zero;
        caughtPlayer = false;
        playerInRange = false;
        waitTime = startWaitTime;
        timeToRotate = rotateTime;


        currentWayPointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);

        _hasAnimator = TryGetComponent(out _animator);
        _animator = GetComponent<Animator>();

        AssignAnimationIDs();
        GameManager.instance.GetEnemies().Add(this);
    }

    protected void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDAttack = Animator.StringToHash("Attack");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _animator = GetComponent<Animator>();
        EnvironmentView();
        if (!isPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }

        if (heardSound)
        {
            navMeshAgent.SetDestination(soundLoc);
            if (Vector3.Distance(transform.position, soundLoc) <= 2f)
            {
                heardSound = false;
            }
        }

        _animator.SetFloat(_animIDSpeed, navMeshAgent.velocity.magnitude);
        if (!navMeshAgent.updatePosition) GroundCharacter();

    }



    protected void MoveTo(Vector3 pos)
    {
        soundLoc = pos;
        
    }
    public void RespondToSound(Sound sound)
    {
        
        isPatrol = false;
        heardSound = true;
        Debug.Log("CanHear" + sound.pos);
        MoveTo(sound.pos);
    }
    protected void Attack()
    {
        if (_hasAnimator)
        {
            _animator.Play("Attack");
        }


    }
    protected void GroundCharacter()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }



    protected override void AttackStart()
    {
        GameManager.instance.PlaySwordSlash();
        base.AttackStart();

        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();
        navMeshAgent.SetDestination(transform.position);

        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;

    }

    protected void Chasing()
    {
        
        
        playerNear = false;
        playerLastPosition = Vector3.zero;

        if (!caughtPlayer)
        {
            Move(runSpeed);
            navMeshAgent.SetDestination(playerPosition);
        }


        // Check if the NavMeshAgent is active before accessing its properties
        if (navMeshAgent.enabled && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (waitTime <= 0 && !caughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 6f)
            {
                isPatrol = true;
                playerNear = false;
                Move(walkSpeed);
                timeToRotate = rotateTime;
                waitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    waitTime -= Time.deltaTime;

                }
            }

        }
        
        
        

    }



    protected void Patroling()
    {
        if(playerNear)
        {
            if(timeToRotate <= 0)
            {
                Move(walkSpeed);
                LookingPlayer(playerLastPosition);

            }
            else
            {
                Stop();
                timeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            if(!randomPatrol)
            {
                playerNear = false;
                playerLastPosition = Vector3.zero;
                navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    if (waitTime <= 0)
                    {
                        NextPoint();
                        Move(walkSpeed);
                        waitTime = startWaitTime;
                    }
                    else
                    {
                        Stop();
                        waitTime -= Time.deltaTime;
                    }
                }
            }
            else
            {
                RandomPatrol();
            }
           
        }
    }

   protected void RandomPatrol()
    {
        if (!walkPointSet) SearchWalkPoint(); // find a walkpoint
        if (walkPointSet)
        {
            navMeshAgent.SetDestination(walkPoint); // set location to move towards to a random walkpoint

        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint; // did we reach the walkpoint

        if (distanceToWalkPoint.magnitude <= 2f)
        {
            walkPointSet = false; // find another walkpoint
        }
    }

    protected void SearchWalkPoint()
    {
        //calculate random point in range
        float randomRangeZ = Random.Range(-walkPointRange, walkPointRange);
        float randomRangeX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomRangeX, transform.position.y, transform.position.z + randomRangeZ); // set walk point to the result of the random generated position
        
        walkPointSet = true;

    }

    protected void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    protected void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
    public void NextPoint()
    {
        currentWayPointIndex = (currentWayPointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
    }

    protected void CaughtPlayer()
    {
        caughtPlayer = true;
    }
    protected void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        Vector3 direction = (player - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMeshAgent.angularSpeed);

        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (waitTime <= 0)
            {
                playerNear = false;
                Move(walkSpeed);
                navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
                waitTime = startWaitTime;
                timeToRotate = rotateTime;
            }
            else
            {
                Stop();
                waitTime = Time.deltaTime;
            }
        }
    }
    protected void EnvironmentView()
    {
        if (Time.time - lastViewCheckTime < viewCheckInterval)
        {
            return;
        }

        lastViewCheckTime = Time.time;

        Collider[] m_playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < m_playersInRange.Length; i++)
        {
            Transform player = m_playersInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                {
                    playerInRange = true;
                    isPatrol = false;
                }
                else
                {
                    playerInRange = false;
                }
            }

            if (Vector3.Distance(transform.position, playerPosition) > viewRadius)
            {
                playerInRange = false;
                isPatrol = true;
            }

            if (playerInRange)
            {
                if (canFlee && runAttempts < maxRunAttempts && distToPlayer < enemyDistanceRun)
                {
                    Vector3 newPos = transform.position - dirToPlayer * 2f; // Increase the distance for fleeing
                    navMeshAgent.SetDestination(newPos);

                    runAttempts++;
                    break; // Prioritize fleeing before attempting any other actions
                }
                else
                {
                    playerPosition = player.transform.position;
                    heardSound = false;

                    if (Vector3.Distance(transform.position, playerPosition) <= 1f)
                    {
                        Attack();
                    }
                }
            }
        }
        // Reset isPatrol if the AI has finished fleeing
        if (canFlee && runAttempts >= maxRunAttempts)
        {
            isPatrol = true;
        }
    }



}
