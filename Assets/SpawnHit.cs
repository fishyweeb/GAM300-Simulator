using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHit : StateMachineBehaviour
{
    [SerializeField]
    string stateName;
    [SerializeField]
    float animationDamageMultiplier = 1;
    [SerializeField]
    [Range(0f, 1f)]
    float animationTime;
    [SerializeField]
    float length;
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    [SerializeField]
    float positionForwardOffset;
    [SerializeField]
    float positionHeightOffset;

    [SerializeField]
    float duration;

    bool spawned = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!spawned)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= animationTime && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                GameObject theGameObject = Instantiate(Resources.Load("HitCollider", typeof(GameObject))) as GameObject;
                if (theGameObject != null)
                {
                    HitCollider hitCollider = theGameObject.GetComponent<HitCollider>();
                    if (hitCollider)
                    {
                        hitCollider.parentLayer = animator.gameObject.layer;
                        hitCollider.spawnDuration = duration;
                        hitCollider.hitDirection = animator.transform.forward;
                        Entity entity = animator.GetComponent<Entity>();
                        if (entity != null)
                        {
                            hitCollider.damage = entity.GetBaseDamage() * animationDamageMultiplier;
                        }
                        Vector3 spawnPos = new Vector3(animator.transform.position.x,animator.transform.position.y + positionHeightOffset,animator.transform.position.z);
                        hitCollider.Spawn(spawnPos + animator.transform.forward * positionForwardOffset * 0.5f, width, height, length);
                        hitCollider.transform.rotation = Quaternion.Euler(0.0f, animator.transform.eulerAngles.y, 0.0f);
                        hitCollider.transform.parent = animator.transform;
                        hitCollider.spawnedFrom = animator.gameObject;

                        AudioManager.instance.PlayCachedSound(AudioManager.instance.WhooshSoundsFX,spawnPos,0.2f);
                    }
                    else
                    {
                        Debug.LogError("No Hit");
                    }
                }
                else
                {
                    Debug.LogError("No Hit");
                }
                spawned = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spawned = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
