using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 6;
    public GameObject healthmanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0){
            transform.position = new Vector3 (-5.15f, -0.99f, 0f);
            health = 100;
        }
    }

    public void takeDamage(int damage){
        health -= damage;
        healthmanager.GetComponent<Healthmanager>().Damage();
    }
}
