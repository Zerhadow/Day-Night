using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject meleeProjectile = null;
    [SerializeField] Transform firePoint = null;
    [SerializeField] float startTime = 0.1f;
    [SerializeField] float recoveryTime = 0.1f;

    bool canSwing = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canSwing)
        {
            StartCoroutine(swing());
        }

    }

    void OnEnable()
    {
        canSwing = true;
    }
    IEnumerator swing()
    {
        canSwing = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(startTime);
        if (!isActiveAndEnabled)
        {
            canSwing = true;
            animator.ResetTrigger("Attack");
        }
        else
        {
            Instantiate(meleeProjectile, firePoint);
            yield return new WaitForSeconds(recoveryTime);
            canSwing = true;
            animator.ResetTrigger("Attack");
        }
    }
}
