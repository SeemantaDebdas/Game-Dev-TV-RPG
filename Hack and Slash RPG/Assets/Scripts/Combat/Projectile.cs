using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject impactParticlePrefab = null;
    [SerializeField] bool isHoming = false;
    [SerializeField] float speed = 1;
    [SerializeField] float projectleLifeTime = 2f;

    Health target = null;
    float damage = 0;
    GameObject instigator = null;

    private void Start()
    {
        //if this gives null ref exception, set this after set target
        transform.LookAt(GetLookAtTarget());
        Destroy(gameObject, projectleLifeTime);
    }

    public void SetTraget(Health target, float damage, GameObject instigator)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (isHoming && !target.IsDead)
        {
            transform.LookAt(GetLookAtTarget());
        }
        transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
    }

    private Vector3 GetLookAtTarget()
    {
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
        if (collider == null) return target.transform.position;
        return target.transform.position + collider.center;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health targetHealth) && targetHealth == target && !target.IsDead)
        {
            if (impactParticlePrefab != null)
            {
                GameObject impactParticlePrefabSpawn = Instantiate(impactParticlePrefab, transform.position, transform.rotation);
                Destroy(impactParticlePrefabSpawn, 0.5f);
            }

            targetHealth.TakeDamage(damage ,instigator);
            Destroy(gameObject);
        }
    }
}
