using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestractionBlock : MonoBehaviour
{
    public SpriteRenderer DestractBlockImage;
    public Sprite DestractBlockHit1;
    public Sprite DestractBlockHit2;  
    public GameObject destractSound;
    public GameObject destractionEffect;
    public int health = 3;

    public void Update()
    {
        //смена спрайта при получении урона до его полного уничтожения 
        if(health == 2)
        {
            DestractBlockImage.GetComponent<SpriteRenderer>().sprite = DestractBlockHit1;
        }
        else if (health == 1)
        {
            DestractBlockImage.GetComponent<SpriteRenderer>().sprite = DestractBlockHit2;
        }
        else if (health <= 0)
        {
            Instantiate(destractSound, transform.position, Quaternion.identity);
            Instantiate(destractionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int healthValue)
    {
        health += healthValue;
    }
}