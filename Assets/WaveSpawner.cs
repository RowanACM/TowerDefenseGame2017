using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    [System.Serializable]
    public class WavePiece
    {
        public GameObject typeOfObject;
        public int quantity;
        public float spawnDelay;
        public float nextPieceDelay;
    }

    public Path spawnPath;
    public float startWait;
    public WavePiece[] wavePieces;
    private int currentWavePiece;
    private CountdownTextAnimation countdown;
    private IEnumerator waveSpawnCoroutine;
	// Use this for initialization
	void Start () {
        countdown = GameObject.FindObjectOfType<CountdownTextAnimation>();
	}

    public bool CanStartWave()
    {
        return waveSpawnCoroutine == null;
    }

    public void StartWave()
    {
        if (waveSpawnCoroutine == null)
        {
            waveSpawnCoroutine = SpawnWave();
            StartCoroutine(waveSpawnCoroutine);
        }
    }

    IEnumerator SpawnWave()
    {
        countdown.Countdown((int)startWait, "Incoming!");
        yield return new WaitForSeconds(startWait);
        for (int i = 0; i < wavePieces.Length; i++)
        {
            for (int j = 0; j < wavePieces[i].quantity; j++)
            {
                //spawn the object for the wave piece
                Vector3 spawnPosition = this.transform.position;
                Quaternion spawnRotation = Quaternion.identity;
                GameObject spawnedObject = Instantiate(wavePieces[i].typeOfObject, spawnPosition, spawnRotation);
                if (spawnedObject.GetComponent<PathMovement>())
                {
                    spawnedObject.GetComponent<PathMovement>().setPath(spawnPath);
                }
                yield return new WaitForSeconds(wavePieces[i].spawnDelay);
                
            }
            yield return new WaitForSeconds(wavePieces[i].nextPieceDelay);
        }
        waveSpawnCoroutine = null;
    }
}
