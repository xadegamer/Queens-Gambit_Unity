using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Retreat }

    [SerializeField] State state;

    [Header("Properties")]
    [SerializeField] bool flying;
    [SerializeField] float normalSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float attackRate;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundOffset;
    [SerializeField] float groundRange;

    [Header("Range")]
    [SerializeField] float detectRange;
    [SerializeField] float attackRange;
    [SerializeField] float stopChaseDistance;

    [Header("AI")]
    [SerializeField] Vector2 startPosition;
    [SerializeField] Vector2[] patrolPositions;
    [SerializeField] Vector2 currentWayPoint;
    [SerializeField] int wayPointIndex = -1;

    float timer;
    bool isFlipped = false;
    float moveSpeed;

    Animator animator;
    Transform player;
    Rigidbody2D rb;

    Enemy enemy;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        player = Player.Instance.transform;
        startPosition = transform.position;
        moveSpeed = normalSpeed;
        currentWayPoint = GetMidPos(patrolPositions[0]);
    }

    void Update()
    {
        if (enemy.ISDead() || Player.Instance.IsDead()) return;

        switch (state)
        {
            case State.Patrol:
                Patrol();
                ScanPlayer();
                break;
            case State.Chase:
                ChasePlayer();
                GroundCheck();
                break;
            case State.Retreat:
                Retreat();
                break;
            default:
                break;
        }
    }

    void Patrol()
    {
        animator.SetBool("Walk", true);

        float distance = Vector2.Distance(transform.position, currentWayPoint);

        if (distance == 0f)
        {
            if (wayPointIndex == patrolPositions.Length - 1)
            {
                wayPointIndex = -1;
                currentWayPoint = GetMidPos(startPosition);
            }
            else
            {
                wayPointIndex++;
                currentWayPoint = GetMidPos(patrolPositions[wayPointIndex]);
            }
        }

        GoToPosition(currentWayPoint);
        LookAtPosition(currentWayPoint);
    }

    void ChasePlayer()
    {
        LookAtPosition(player.position);
        GoToPosition(player.position);

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetBool("Chase", false);
            moveSpeed = 0;
            Attack();
        }
        else
        {
            animator.SetBool("Chase", true);
            moveSpeed = runSpeed;
        }

        if (Vector3.Distance(transform.position, player.position) > stopChaseDistance)
        {
            animator.SetBool("Chase", false);

            moveSpeed = normalSpeed;
            animator.SetTrigger("TargetLost");
            state = State.Patrol;
        }
    }

    void Retreat()
    {
        moveSpeed = runSpeed;
        animator.SetBool("Chase", true);
        LookAtPosition(startPosition);
        GoToPosition(startPosition);

        float reachedPosition = 3.5f;

        if (Vector2.Distance(rb.position, startPosition) <= reachedPosition)
        {
            animator.SetBool("Chase", false);

            moveSpeed = normalSpeed;
            animator.SetTrigger("TargetLost");
            state = State.Patrol;
        }
    }

    void ScanPlayer()
    {
        if (Vector2.Distance(player.position, rb.position) <= detectRange)
        {
            animator.SetTrigger("Spotted");
            animator.SetBool("Walk", false);
            moveSpeed = runSpeed;
            state = State.Chase;
        }
    }

    void Attack()
    {
        if (timer < 0)
        {
            animator.SetTrigger("Attack");
            timer = attackRate;
        }
        else timer -= Time.deltaTime;
    }

    public void LookAtPosition(Vector2 Target)
    {
        Vector3 flipped = transform.localScale;
        flipped.x *= -1f;

        if (transform.position.x > Target.x && !isFlipped)
        {
            transform.localScale = flipped;
            isFlipped = true;
        }
        else if (transform.position.x < Target.x && isFlipped)
        {
            transform.localScale = flipped;
            isFlipped = false;
        }
    }

    void GoToPosition(Vector2 Target)
    {
        Vector2 positionToGo;

        if (flying)
        {
            positionToGo = Vector2.MoveTowards(rb.position, Target, moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 correctPosition = new Vector2(Target.x, rb.position.y);
            positionToGo = Vector2.MoveTowards(rb.position, correctPosition, moveSpeed * Time.fixedDeltaTime);
        }

        rb.MovePosition(positionToGo);
    }

    public void GroundCheck()
    {
        if (flying) return;

        Vector2 currentOffset = new Vector2(groundOffset.x * transform.localScale.x, groundOffset.y);
        Collider2D ground = Physics2D.OverlapCircle((Vector2)transform.position + currentOffset, groundRange, groundLayer);

        if(ground == null) state = State.Retreat;
    }

    public Vector2 GetMidPos(Vector2 point)
    {
        Vector2 newPos = new Vector2(point.x, point.y);
        newPos.x -= transform.GetComponent<Collider2D>().bounds.extents.x;
        return newPos;
    }

    public IEnumerator KnockBack(float knockDownDuration, float knockDownPower, Transform obj)
    {
        float timer = 0;

        while (knockDownDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (obj.position - transform.position).normalized;
            rb.AddForce(-direction * knockDownPower, ForceMode2D.Impulse);
        }

        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopChaseDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        for (int i = 0; i < patrolPositions.Length; i++)
        {
            Gizmos.DrawWireSphere(patrolPositions[i], .2f);
            if (!flying) patrolPositions[i].y = transform.position.y;
        }

        Gizmos.color = Color.blue;
        Vector2 currentOffset = new Vector2(groundOffset.x * transform.localScale.x, groundOffset.y);
        Gizmos.DrawWireSphere((Vector2)transform.position + currentOffset, groundRange);
    }
}
