using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IHittable
{
    public float baseHealth;

    [Range(0,1)]
    public float damageModifier = 1;

    [SerializeField]
    private float health;
    public float Health{
        get{
            return health;
        }
        set{
            health = value;
            health = Mathf.Clamp(health, -1, baseHealth);
        }
    }


    public specialHitPoint[] weakPoints;

    public float regenTimer;
    public float regenPerSecond;

    [HideInInspector]
    public float activeRegenTimer;

    public virtual void GetHit(Vector3 position, Vector3 direction, float damage)
    {
        float fixedDamage = 
            damage * damageModifier;

        foreach(var item in weakPoints){
            // Determine if the hit was inside of the object
            if ((item.transform.position - position).magnitude <= item.transform.lossyScale.x){
                fixedDamage = fixedDamage * item.damageModifier;
                break;
            }
        }

        this.TakeDamage(fixedDamage);
    }

    public virtual void TakeDamage(float damage){
        // Comes through as true damage
        Health -= damage;

        this.StartRegenTimer();
        if(Health <= 0) {
            this.Die();
        }
    }

    public virtual void Die(){
        // Some death behaviour
        Destroy(gameObject);
    }
    public virtual bool CheckRegenTimer(){
        return Time.time >= activeRegenTimer;
    }
    public virtual void StartRegenTimer(){
        activeRegenTimer = Time.time + regenTimer;
    }
    public virtual void Regen(){
        //true regen
        Health += regenPerSecond * Time.deltaTime;
    }

    void Start(){
        Health = baseHealth;
    }
    void FixedUpdate()
    {
        if(CheckRegenTimer()){
            Regen();
        }
    }
}

[System.Serializable]
public struct specialHitPoint{
    public Transform transform;

    [Min(0)]
    public float damageModifier;
}