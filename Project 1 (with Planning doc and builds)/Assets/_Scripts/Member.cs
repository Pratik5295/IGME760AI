using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour {

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Level level;
    public MemberConfig conf;

    Vector3 wanderTarget;

    void Start() {
        level = FindObjectOfType<Level>();
        conf = FindObjectOfType<MemberConfig>();

        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }

    void Update() {
        if (Input.GetKey(KeyCode.C)) {
            conf.cohesionPriority = 100 - conf.cohesionPriority;
            //conf.alignmentPriority = 0;
            //conf.separationPriority = 0;
        }
        if (Input.GetKey(KeyCode.L))
        {
            //conf.cohesionPriority = 0;
            conf.alignmentPriority = 100 - conf.alignmentPriority;
            //conf.separationPriority = 0;
        }
        if (Input.GetKey(KeyCode.P))
        {
            //conf.cohesionPriority = 0;
            //conf.alignmentPriority = 0;
            conf.separationPriority = 100 - conf.separationPriority;
        }
        acceleration = Combine();
        acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);
        position = position + velocity * Time.deltaTime;
        WrapAround(ref position, -level.bounds, level.bounds);
        transform.position = position;
    }

    // 
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

    Vector3 Cohesion() {
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

    Vector3 Alignment() {
        Vector3 alignVector = new Vector3();
        var members = level.GetNeighbors(this, conf.alignmentRadius);
        if (members.Count == 0)
            return alignVector;

        foreach (var member in members) {
            if (isInFOV(member.position))
                alignVector += member.velocity;
        }

        return alignVector.normalized;
    }

    Vector3 Separation() {
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
    virtual protected Vector3 Combine() {

        Vector3 finalVec = conf.cohesionPriority * Cohesion() + conf.wanderPriority * Wander()
            + conf.alignmentPriority * Alignment() + conf.separationPriority * Separation();
        return finalVec;
    }

    // If an object is moving too far, make it back 
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

    float RandomBinomial() {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    bool isInFOV(Vector3 vec) {
        return Vector3.Angle(this.velocity, vec - this.position) <= conf.maxFOV;
    }
}
