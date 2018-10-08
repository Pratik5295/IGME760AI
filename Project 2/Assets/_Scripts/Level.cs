using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public Transform memberPrefab;
    public int numberOfMembers;
    public List<Member> members;
    public float bounds;
    public float spawnRadius;

	// Use this for initialization
	void Start () {
        members = new List<Member>();

        // Spawn Members
        Spawn(memberPrefab, numberOfMembers);

        members.AddRange(FindObjectsOfType<Member>());
	}

    // Function to Generate a list of gameObjects randomly
    void Spawn(Transform prefab, int count) {
        for (int i = 0; i < count; i++) {
            Instantiate(prefab, new Vector3(Random.Range(-spawnRadius, spawnRadius), 
                0, 
                Random.Range(-spawnRadius, spawnRadius)), 
                Quaternion.identity);
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
