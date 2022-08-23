using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CubeSpawner : MonoBehaviour
{
    [SerializeField] CubeMover cubePrefab;
    [SerializeField] MoveDirection moveDirection;
    float x , z;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);
        if(moveDirection == MoveDirection.X)
        {
            x = transform.position.x;
            z = CubeMover.LastCube.transform.position.z;
        }
        else
        {
            x = CubeMover.LastCube.transform.position.x;
            z = transform.position.z;
        }

        // last cube control
        if (CubeMover.LastCube != null && CubeMover.LastCube.gameObject != GameObject.Find("Start"))
        {
            cube.transform.position = new Vector3(x,
                                    CubeMover.LastCube.transform.position.y + cubePrefab.transform.localScale.y, z);
        }
        else
        {
            cube.transform.position = transform.position;
        }
        cube.moveDirection = moveDirection;
        // CubeMover.LastCube = cube.GetComponent<CubeMover>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}
