using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public enum PickupType {
        Health,
        Shield,
        Exp
    }
    
    public PickupType myType = PickupType.Exp;

    public int AmountToIncrease = 20;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, Time.deltaTime * 30.0f, 0.0f);
        var myPosition = transform.position;
        myPosition.y = Player.instance.transform.position.y + 0.5f + Mathf.Sin(Time.time * 2.0f) * 0.1f;
        transform.position = myPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.instance.gameObject)
        {
            switch (myType) {
                case PickupType.Exp:
                    Player.instance.healthSystem.IncreaseXP(AmountToIncrease);
                    break;

                case PickupType.Shield:
                    Player.instance.healthSystem.RegenerateShield(AmountToIncrease);
                    break;

                case PickupType.Health:
                    Player.instance.healthSystem.Heal(AmountToIncrease);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
