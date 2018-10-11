using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public GameObject memberPrefab;
    public int numberOfMembers;
    public List<Member> members;
    public float bounds;
    public float spawnRadius;

    public GameObject flockingSpawner;

	// Use this for initialization
	void Start () {
        members = new List<Member>();

        // Spawn Members
        Spawn(memberPrefab, numberOfMembers);

        members.AddRange(FindObjectsOfType<Member>());
	}

    // Function to Generate a list of gameObjects randomly
    void Spawn(GameObject prefab, int count) {
        for (int i = 0; i < count; i++) {
            /*
            prefab = Instantiate(prefab, new Vector3(Random.Range(-spawnRadius, spawnRadius), 
                0,
                Random.Range(-spawnRadius, spawnRadius)), 
                Quaternion.identity) as GameObject;
            
            prefab = Instantiate(prefab, new Vector3(Random.Range(61, 67),
                -5.5f,
                Random.Range(91, 97)),
                Quaternion.identity) as GameObject;
                */

            prefab = Instantiate(prefab, flockingSpawner.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius),
                0,
                Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity) as GameObject;
            prefab.transform.parent = flockingSpawner.transform;
        }
    }

    public List<Member> GetNeighbors(Member member, float radius) {
        List<Member> neighborsFound = new List<Member>();

        foreach (var otherMember in members) {
            if (otherMember == member)
                continue;

            if (Vector3.Distance(member.position, otherMember.position) <= radius) {
                neighborsFound.Add(otherMember);
            }
        }

        return neighborsFound;
    }
}
