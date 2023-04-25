using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
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

    public Transform[] waypoints;
    int currentWayPointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 playerPosition;

    float waitTime;
    float timeToRotate;
    bool playerInRange;
    bool playerNear;
    bool isPatrol;
    bool caughtPlayer;
    private float _animationBlend;
    public float SpeedChangeRate = 10.0f;
    bool hasPlayedSwooshSound = false;
    [SerializeField] private float slashDuration = 0.5f;


    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDAttack;

    private Animator _animator;
    private bool _hasAnimator;

    void Start()
    {
        playerPosition = Vector3.zero;
        isPatrol = false;
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
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDAttack = Animator.StringToHash("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        _animator = GetComponent<Animator>();
        EnvironmentView();

        if(!isPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
        _animator.SetFloat(_animIDSpeed, navMeshAgent.velocity.magnitude);
        
    }
    private void Attack()
    {
        if (_hasAnimator)
        {


            _animator.Play("Attack");
            StartCoroutine(WaitForAttactDuration());


        }


    }


    protected override void AttackStart()
    {
        GameManager.instance.PlaySwordSlash();
        base.AttackStart();
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();
    }

    private IEnumerator WaitForAttactDuration()
    {
        yield return new WaitForSeconds(slashDuration);
        
        hasPlayedSwooshSound = false;


    }

    private void Chasing()
    {
        playerNear = false;
        playerLastPosition = Vector3.zero;

        if(!caughtPlayer)
        {
            Move(runSpeed);
            navMeshAgent.SetDestination(playerPosition);
        }
        

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if(waitTime <= 0  && !caughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 6f)
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
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    waitTime -= Time.deltaTime;
                    
                }
            }
            
        }
        

    }


    private void Patroling()
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
            playerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
            if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                if(waitTime <= 0)
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
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
    public void NextPoint()
    {
        currentWayPointIndex = (currentWayPointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
    }

    void CaughtPlayer()
    {
        caughtPlayer = true;
    }
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position, player) <= 0.3)
        {
            if(waitTime <= 0)
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
    void EnvironmentView()
    {
        Collider[] m_playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < m_playersInRange.Length; i++)
        {
            Transform player = m_playersInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float distToPlayer = Vector3.Distance(transform.position, player.position);
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

            }
            if (playerInRange)
            {
                playerPosition = player.transform.position;
            }
            if(Vector3.Distance(transform.position, playerPosition) <= 1f && playerInRange)
            {
                
                Attack();
            }
        }
    }
}   
