using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startHealth=100;
    public int currentHealth;
    public Vector3 StartPosition;
    public void GetShot(int damage){
        applyDamage(damage);
    }
    ShootingAgent sa;
    public void applyDamage(int damage){
        currentHealth-=damage;
        if(currentHealth<=0){
            Die();
        }
    }

    public void Die(){
        // Debug.Log("I Died");
        sa.iDie();
        Respawn();
    }

    public void Respawn(){
        currentHealth=startHealth;
        transform.position=new Vector3(StartPosition.x+Random.Range(-2, 2),StartPosition.y,StartPosition.z+Random.Range(-2, 2));
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth=startHealth;
        StartPosition=transform.position;
        sa=GetComponent<ShootingAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //testing
    private void OnMouseDown() {
        GetShot(100);
    }
}
