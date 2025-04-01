using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject roadSegmentPrefab;
    public int numberOfSegments = 5;
    public float segmentLength = 20.0f;
    private List<GameObject> roadSegments = new List<GameObject>();
    private Transform playerTransform;
    
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        
        // Create initial road segments along Z axis
        for (int i = 0; i < numberOfSegments; i++)
        {
            CreateRoadSegment(i * segmentLength);
        }
    }

    void Update()
    {
        if (playerTransform != null && roadSegments.Count > 0 && 
            playerTransform.position.z > roadSegments[0].transform.position.z + segmentLength)
        {
            RecycleSegment();
        }
    }

    void CreateRoadSegment(float zPosition)
    {
        Vector3 spawnPosition = new Vector3(0, 0, zPosition);
        
        // Explicitly set rotation to align with Z-axis (looking down Z)
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        
        GameObject segment = Instantiate(roadSegmentPrefab, spawnPosition, rotation);
        
        // Set parent to keep hierarchy clean
        segment.transform.parent = this.transform;
        
        roadSegments.Add(segment);
    }

    void RecycleSegment()
    {
        GameObject oldSegment = roadSegments[0];
        roadSegments.RemoveAt(0);
        Destroy(oldSegment);

        float newZPosition = roadSegments[roadSegments.Count - 1].transform.position.z + segmentLength;
        CreateRoadSegment(newZPosition);
    }
}