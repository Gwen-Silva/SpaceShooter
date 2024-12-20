﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Header("Wave Configuration")]
    public GameObject enemyPrefab;
    public GameObject pathPrefab;
    [Space]
    [Tooltip("How much time in seconds each Enemy will be spawned.")]
    [Range(0.1f, 1f)]
    public float timeBetweenSpawns = 0.5f;
    // public float spawnRandomFactor = 0.3f;
    [Tooltip("How many enemies will be spawned.")]
    public int numberOfEnemies = 5;
    [Tooltip("Movement Speed of each Enemy from this Wave.")]
    [Range(1f, 6f)]
    public float moveSpeed = 2f;

    public List<Transform> GetWaypoints()
    {
        List<Transform> waveWaypoints = new List<Transform>();

        foreach (Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }

        return waveWaypoints;
    }


}
