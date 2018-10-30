﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour {

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Level level;
    public MemberConfig conf;

    //Vector3 wanderTarget;

    public GameObject flockingTarget;
    public GameObject obstacle;

    public bool cohesionState;
    public bool alignmentState;
    public bool separationState;

    void Start() {
        cohesionState = true;
        alignmentState = true;
        separationState = true;

        level = FindObjectOfType<Level>();
        conf = FindObjectOfType<MemberConfig>();

        position = transform.position;

        flockingTarget = GameObject.FindGameObjectWithTag("Player");
        obstacle = GameObject.FindGameObjectWithTag("Obstacle");
        //velocity = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }

    void Update() {

        // keyboard control of COHESION, ALIGNMENT, SEPARATION parameter of the flocking agents
        // disable parameter

        if (Input.GetKey(KeyCode.I))
        {
            if (conf.cohesionPriority != 0)
            {
                conf.cohesionPriority = 0;
            }
        }
        if (Input.GetKey(KeyCode.O))
        {
            //conf.cohesionPriority = 2;
            if (conf.alignmentPriority != 0)
            {
                conf.alignmentPriority = 0;
            }

        }
        if (Input.GetKey(KeyCode.P))
        {
            //conf.cohesionPriority = 2;
            //conf.alignmentPriority = 2;
            if (conf.separationPriority != 0)
            {
                conf.separationPriority = 0;
            }

        }

        // enable parameter
        if (Input.GetKey(KeyCode.J))
        {
            if (conf.cohesionPriority == 0)
            {
                conf.cohesionPriority = 20;
            }
        }
        if (Input.GetKey(KeyCode.K))
        {
            //conf.cohesionPriority = 2;
            if (conf.alignmentPriority == 0)
            {
                conf.alignmentPriority = 10;
            }

        }
        if (Input.GetKey(KeyCode.L))
        {
            //conf.cohesionPriority = 2;
            //conf.alignmentPriority = 2;
            if (conf.separationPriority == 0)
            {
                conf.separationPriority = 15;
            }

        }

        // restore parameters
        if (Input.GetKey(KeyCode.R))
        {
            conf.cohesionPriority = 20;
            conf.alignmentPriority = 10;
            conf.separationPriority = 15;
        }

        /*
        if (Input.GetKey(KeyCode.I)) {
            if (cohesionState == true)
            {
                conf.cohesionPriority = 0;
                cohesionState = false;

                return;
                
            }
            else
            { 
                conf.cohesionPriority = 10;
                cohesionState = true;
                return;
            }
            //conf.alignmentPriority = 2;
            //conf.separationPriority = 2;
        }
        if (Input.GetKey(KeyCode.O))
        {
            //conf.cohesionPriority = 2;
            if (conf.alignmentPriority != 0)
            {
                conf.alignmentPriority = 0;
            }
            if (conf.alignmentPriority == 0)
            {
                conf.alignmentPriority = 10;
            }
            //conf.separationPriority = 2;
        }
        if (Input.GetKey(KeyCode.P))
        {
            //conf.cohesionPriority = 2;
            //conf.alignmentPriority = 2;
            if (conf.separationPriority != 0)
            {
                conf.separationPriority = 0;
            }
            if (conf.separationPriority == 0)
            {
                conf.separationPriority = 10;
            }
            
        }
        if (Input.GetKey(KeyCode.R)) {
            conf.cohesionPriority = 5;
            conf.alignmentPriority = 10;
            conf.separationPriority = 10;
        }
        */

        // compute the total force vector add on the flocking agents 
        acceleration = Combine();
        acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);
        //velocity = velocity.normalized;
        //GetComponent<Rigidbody>().AddForce(velocity * 1f);

        GetComponent<Rigidbody>().AddForce(acceleration * 1f);
        //Debug.Log(velocity);
        //GetComponent<Rigidbody>().MovePosition(transform.localPosition + velocity);
        //position = position + velocity * Time.deltaTime;
        
        //WrapAround(ref position, -level.bounds, level.bounds);
        //transform.localPosition = position;
    }

    /*
    protected Vector3 Wander() {
        float jitter = conf.wanderJitter * Time.deltaTime;
        wanderTarget += new Vector3(RandomBinomial() * jitter, 0, RandomBinomial() * jitter);
        wanderTarget = wanderTarget.normalized;
        wanderTarget *= conf.wanderRadius;
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(conf.wanderDistance, 0, conf.wanderDistance);
        //Debug.Log(conf.wanderDistance);
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        targetInWorldSpace -= this.position;
        return targetInWorldSpace.normalized;
    }
    */


    Vector3 Cohesion() {            // compute cohesion vector
        Vector3 cohesionVector = new Vector3();
        int countMembers = 0;
        var neighbors = level.GetNeighbors(this, conf.cohesionRadius);
        if (neighbors.Count == 0)
            return cohesionVector;
        foreach (var member in neighbors) {
            if (isInFOV(member.position)) {
                cohesionVector += member.position;
                countMembers++;
            }
        }

        if (countMembers == 0) {
            return cohesionVector;
        }

        cohesionVector /= countMembers;
        cohesionVector = cohesionVector - this.position;
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;
    }

    Vector3 Alignment() {            // compute alignment vector
        Vector3 alignVector = new Vector3();
        var members = level.GetNeighbors(this, conf.alignmentRadius);
        if (members.Count == 0) {
            alignVector.x = 0f;
            alignVector.y = 0f;
            alignVector.z = 0f;
        }
            return alignVector;

        foreach (var member in members) {
            if (isInFOV(member.position))
                alignVector += member.velocity;
        }

        return alignVector.normalized;
    }

    Vector3 Separation() {           // compute separation vector
        Vector3 separateVector = new Vector3();
        var members = level.GetNeighbors(this, conf.separationRadius);
        if (members.Count == 0)
            return separateVector;

        foreach (var member in members) {
            if (isInFOV(member.position)) {
                Vector3 movingTowards = this.position - member.position;
                if (movingTowards.magnitude > 0)
                {
                    separateVector += movingTowards.normalized / movingTowards.magnitude;
                }
            }
        }

        return separateVector;
    }

    Vector3 OAvoidance()            // compute avoidance vector related to obstacles
    {
        Vector3 oavoidVector = new Vector3();
        oavoidVector = this.position - obstacle.transform.position;
        //oavoidVector = obstacle.transform.position - this.position;
        return oavoidVector;
    }
    Vector3 Avoidance() {           // compute avoidance vector related to the leader of the flocking agents
        Vector3 avoidVector = new Vector3();
        avoidVector = this.position - flockingTarget.transform.position;
        return avoidVector;
    }
    
    private Vector3 Following() {           // compute the following vector towards the leader of the flocking agents
        Vector3 followVector = new Vector3();
        followVector = flockingTarget.transform.position - transform.position;
        return followVector;
    }
    virtual protected Vector3 Combine() {           // compute the total vector applied to the flocking agents/members
        Vector3 finalVec;
        if (flockingTarget)
        {
            finalVec = conf.cohesionPriority * Cohesion()
            + conf.alignmentPriority * Alignment() + conf.separationPriority * Separation() + conf.followingPriority * Following() + conf.avoidancePriority * Avoidance() + conf.oavoidancePriority * OAvoidance();
        }
        else {
            finalVec = conf.cohesionPriority * Cohesion()
            + conf.alignmentPriority * Alignment() + conf.separationPriority * Separation();
        }

        return finalVec;
    }

   
    /*
     * // If an object is moving too far, make it back 
    void WrapAround(ref Vector3 vector, float min, float max) {
        vector.x = WrapAroundFloat(vector.x, min, max);
        vector.y = WrapAroundFloat(vector.y, min, max);
        vector.z = WrapAroundFloat(vector.z, min, max);

    }

    float WrapAroundFloat(float value, float min, float max) {

        if (value > max)
            value = min;
        else if (value < min)
            value = max;
        return value;

    }
    */

    float RandomBinomial() {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    bool isInFOV(Vector3 vec) {
        return Vector3.Angle(this.velocity, vec - this.position) <= conf.maxFOV;
    }
}
