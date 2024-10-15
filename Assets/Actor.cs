using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    CharacterController charControl;
    public float speed = 10.0f;
    public Transform visual;

    public HealthSystem healthSystem = new HealthSystem();
    public Projectile prefabProjectile;

    int lastCheckingForDeath = 0;
    // Start is called before the first frame update
    public virtual void Start()
    {
        healthSystem.ResetGame();
        charControl = gameObject.GetComponent<CharacterController>();
    }

    public void ShootProjectile() {
        var newProjectile = GameObject.Instantiate(prefabProjectile);
        newProjectile.transform.position = transform.position + (Vector3.up * 0.45f) + (ShootDirection() * 0.35f);
        newProjectile.transform.forward = ShootDirection();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Vector3 desiredMovement = GetMovementDirection();

        charControl.Move(desiredMovement.normalized * speed * Time.deltaTime);

        if (Mathf.Abs(desiredMovement.magnitude) > 0.1f) {
            visual.transform.forward = Vector3.Lerp(visual.transform.forward, GetLookDirection(), Time.deltaTime * 24.0f);
        }

        if (WantsToShoot()) {
            ShootProjectile();
        }

        if (healthSystem.health != lastCheckingForDeath)
        {
            lastCheckingForDeath = healthSystem.health;
            if (healthSystem.health <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {

    }

    public virtual Vector3 ShootDirection()
    {
        return visual.transform.forward;
    }

    public virtual bool WantsToShoot()
    {
        return false;
    }

    public virtual Vector3 GetMovementDirection()
    {
        return Vector3.zero;
    }
    public virtual Vector3 GetLookDirection()
    {
        return GetMovementDirection();
    }
}
