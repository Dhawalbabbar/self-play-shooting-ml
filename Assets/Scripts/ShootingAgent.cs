using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShootingAgent : Agent
{
    public Transform shootingPoint;
    public int minStepsBetweenShoots=50;
    public float rotationSpeed = 3f;
    public int damage=100;
    public bool shootAvailable;
    private int StepsUntilShootAvailable=0;
    private Vector3 startingPosition;
    private Rigidbody rb;
    public float speed = 3f;
    public void Shoot(){
        var layerMask=1<<LayerMask.NameToLayer("Agent");
        var direction=-transform.right;
        // Debug.Log("Shoot");
        Debug.DrawRay(shootingPoint.position,direction,Color.green,1f);
        // var spawnedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.Euler(0f, -90f, 0f));
        // spawnedProjectile.SetDirection(direction);
        if(Physics.Raycast(shootingPoint.position,direction,out var hit)){
            // Debug.Log("here");
            // Debug.Log(hit.collider.gameObject.tag);
            if (hit.collider.gameObject.tag=="Agent"){
                hit.transform.GetComponent<Enemy>().GetShot(damage);
                RegisterKill();
            }
            else if(hit.collider.gameObject.tag=="wall"){
                // Debug.Log("hit wall");
                AddReward(-0.033f);
            }
            
        }
        shootAvailable=false;
        StepsUntilShootAvailable=minStepsBetweenShoots;
    }

    public void iDie(){
        print("iDie");
        AddReward(-2f);
    }

    private void FixedUpdate() {
        // var direction=-transform.forward;
        // Debug.Log("Shoot");
        // Debug.DrawRay(transform.position,direction,Color.green,20f);
        if(!shootAvailable){
            StepsUntilShootAvailable--;
            if (StepsUntilShootAvailable<=0){
                shootAvailable=true;
            }
        }
    }

    // private void OnMouseEnter() {
    //     Shoot();
    // }
    public override void OnActionReceived(float[] vectorAction){
        // Debug.Log(vectorAction);
        if(Mathf.RoundToInt(vectorAction[0])>=1){
            // Debug.Log(vectorAction);
            if(shootAvailable) Shoot();
        }
        rb.velocity = new Vector3(vectorAction[2] * speed, 0f, vectorAction[1] * speed);
        transform.Rotate(Vector3.up,vectorAction[3]*rotationSpeed);
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(shootAvailable);
    }

    public override void Heuristic(float[] actionsOut){
        if (Input.GetKey(KeyCode.P)) actionsOut[0]=1f;
        else actionsOut[0]=0f;
        actionsOut[1] = Input.GetAxis("Horizontal");
        actionsOut[2] = Input.GetAxis("Vertical");
        // actionsOut[0]=1f;
        // actionsOut[0]=Input.GetKey(KeyCode.P)?1f:0f;
    }

    public override void Initialize(){
        startingPosition=transform.position;
        rb=GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin(){
        // Debug.Log("ep begin");
        transform.position=startingPosition;
        shootAvailable=true;
        // rb.velocity = Vector3.zero;
    }

    public void RegisterKill(){
        AddReward(1f);
    }
}
