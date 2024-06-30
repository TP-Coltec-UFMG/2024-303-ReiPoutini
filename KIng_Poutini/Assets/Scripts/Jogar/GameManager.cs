using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    private Vector3 checkpointPosition;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void UpdateCheckpoint(Vector3 newCheckpoint) {
        checkpointPosition = newCheckpoint;
        SaveCheckpoint();
    }

    public Vector3 GetCheckpointPosition() {
        return checkpointPosition;
    }

    public void SaveCheckpoint() {
        PlayerPrefs.SetFloat("checkpointX", checkpointPosition.x);
        PlayerPrefs.SetFloat("checkpointY", checkpointPosition.y);
        PlayerPrefs.SetFloat("checkpointZ", checkpointPosition.z);
    }

    public void LoadCheckpoint() {
        if (PlayerPrefs.HasKey("checkpointX")) {
            float x = PlayerPrefs.GetFloat("checkpointX");
            float y = PlayerPrefs.GetFloat("checkpointY");
            float z = PlayerPrefs.GetFloat("checkpointZ");
            checkpointPosition = new Vector3(x, y, z);
        } else {
            checkpointPosition = Vector3.zero;
        }
    }
}