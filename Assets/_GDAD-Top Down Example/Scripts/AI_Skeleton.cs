using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Skeleton : MonoBehaviour
{
    public Animator _anim;

    [SerializeField] private Transform _target;
    [SerializeField] [Range(0.5f, 2.5f)] private float _attackDistance;
    private NavMeshAgent _navMeshAgent;
    private AI_Skeleton_StateMC _stateMachine;
    private float _previousXpos;

    private const string _iswalking = "isWalking";
    private const string _isAttacking = "isAttacking";

    private void Awake()
    {
        _stateMachine = GetComponentInChildren<AI_Skeleton_StateMC>();
        _anim = GetComponent<Animator>();
    }


    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _previousXpos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMovingRight()) GetComponent<SpriteRenderer>().flipX = false;
        else GetComponent<SpriteRenderer>().flipX = true;

        // check current _stateMachine animation state
        AnimatorStateInfo stateInfo = _stateMachine.anim.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Can see player: " + _stateMachine.CanSeePlayer());
        if (_stateMachine.CanSeePlayer())
        {
            bool isAngry = stateInfo.IsName("Angry");
            if (AttackDistCheck() && isAngry)
            {
                _anim.SetBool(_isAttacking, true);
                _navMeshAgent.isStopped = true;
                _anim.SetBool(_iswalking, false);
            }
            else
            {
                _anim.SetBool(_isAttacking, false);
                _navMeshAgent.isStopped = false;
                _anim.SetBool(_iswalking, true);
                _navMeshAgent.SetDestination(_target.position);
            }
    
        }
        else
        {
            _navMeshAgent.isStopped = true;
            _anim.SetBool(_iswalking, false);
        }
        _previousXpos = transform.position.x;
    }

    // check direction of movement in the x axis
    public bool IsMovingRight()
    {
        return transform.position.x >= _previousXpos;
    }

    public bool AttackDistCheck()
    {
        return ((_target.position - transform.position).magnitude < _attackDistance);
    }
}
