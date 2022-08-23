using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] CubeSpawner[] spawners;
    [SerializeField] TMP_Text text;
    [SerializeField] Camera cam;
    int currentSpawner = 1;
    float cubey = 0.1f;
    int level = 0;
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CubeMover.CurrentCube.StopCube();

            currentSpawner = currentSpawner == 0 ? 1 : 0;
            spawners[currentSpawner].SpawnCube();
            text.text = level.ToString();
            level++;
            cam.transform.position = new Vector3(cam.transform.position.x , cam.transform.position.y + cubey , cam.transform.position.z);
        }    
    }
}
