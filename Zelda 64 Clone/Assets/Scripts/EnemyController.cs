using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    public Transform player;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private bool isMoving;

    //public int hurtDamage = -10;
    public float attackRange = 2f;
    private bool takeDamage = true;
    private float damageDelay = 1.5f;

    IEnumerator DamageDelay()
    {
        takeDamage = false;
        yield return new WaitForSeconds(damageDelay);
        takeDamage = true;
    }

    public void damagePlayer() {

      if (takeDamage)
      {
        Managers.Player.ChangeHealth(-5);
        StartCoroutine(DamageDelay());
      }
    }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {

      float distanceToPlayer = Vector3.Distance(transform.position, player.position);
      if (distanceToPlayer <= attackRange)
      {
          damagePlayer();
      }

      if (Managers.Player.isDead == true)
      {
        animator.SetBool("run", false);
      }else
      {
        navMeshAgent.SetDestination(player.position - new Vector3(0,0,1f));
        animator.SetBool("run", isMoving);
      }

        if (navMeshAgent.velocity.magnitude > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //animator.SetBool("run", isMoving);
    }
}
