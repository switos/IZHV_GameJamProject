using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform turRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;

    // Start is called before the first frame update
    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f;

    private Animator anim;
    private Transform target;
    private float timeUntilFire;

    #if UNITY_EDITOR
        // Code that uses Handles
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
        }
    #endif
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if(hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            anim.SetBool("Shooting", false);
            FindTarget();
            return;
        } 
        RotateTowardsTarget();
        if (!CheckRangeForTarget())
        {
            anim.SetBool("Shooting", false);
            target = null;
        } else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= (1f / bps )/2) { anim.SetBool("Shooting", true); }

            if (timeUntilFire >= 1f/ bps)
            {
                Shoot();
                timeUntilFire = 0f;
                anim.SetBool("Shooting", false);
            }
        }
    }

    private void Shoot()
    {
        GameObject projObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
        Projectile projScript = projObj.GetComponent<Projectile>();
        projScript.SetTarget(target);

    }

    private bool CheckRangeForTarget()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turRotationPoint.transform.rotation = Quaternion.RotateTowards(turRotationPoint.transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
    }
}
