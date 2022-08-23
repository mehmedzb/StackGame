using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeMover : MonoBehaviour
{
    public static CubeMover CurrentCube{ get; private set; }
    public static CubeMover LastCube{ get;  set; }
    public MoveDirection moveDirection { get; set;}

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float xBorder = 2f;
    [SerializeField] float zBorder = 2f;
    private bool flag = true;
    private bool flagz = false;
    private GameObject fallCube;
    
    private void OnEnable() 
    {
        if(LastCube == null)
        {
            LastCube = GameObject.Find("Start").GetComponent<CubeMover>();
        }
        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(LastCube.transform.localScale.x , transform.localScale.y,  LastCube.transform.localScale.z);

    }

    public Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f));
    }
    
    
    void Update()
    {
        if(moveDirection == MoveDirection.X )
        {
            MoveCubeX();
        }
        else
        {
            MoveCubeZ();
        }
    }

    void MoveCube(float speed , MoveDirection md)
    {
        if(md == MoveDirection.X)
            transform.position += transform.right * Time.deltaTime * speed;
        else if(md == MoveDirection.Z)
            transform.position += transform.forward * Time.deltaTime * speed;
    }

    void MoveCubeX()
    {
        if(flag)
        {
            MoveCube(moveSpeed , MoveDirection.X);
            if(transform.position.x >= xBorder)
                flag = false;
        }
        else
        {
            MoveCube(-moveSpeed , MoveDirection.X);
            if(transform.position.x <= -xBorder)
                flag = true;
        }
    }
    void MoveCubeZ()
    {
        if(flagz)
        {
            MoveCube(moveSpeed , MoveDirection.Z);
            if(transform.position.z >= zBorder)
                flagz = false;
        }
        else
        {
            MoveCube(-moveSpeed , MoveDirection.Z);
            if(transform.position.z <= -zBorder)
                flagz = true;
        }
    }

    public void StopCube()
    {
        moveSpeed = 0;

        float kalicakOlan = GetKalicakOlan(); // kalicak olan küpü hesaplıyor.
        float max = moveDirection == MoveDirection.X ? LastCube.transform.localScale.x :  LastCube.transform.localScale.z;
        if(Mathf.Abs(kalicakOlan) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }

        float direction = kalicakOlan > 0 ? 1f : -1f;
        if(moveDirection == MoveDirection.X)
            SplitCubeX(kalicakOlan,direction);
        else
            SplitCubeZ(kalicakOlan,direction);
        LastCube = this;
    }

    float GetKalicakOlan()
    {
        if(moveDirection == MoveDirection.X)
            return transform.position.x - LastCube.transform.position.x;
        else
            return transform.position.z - LastCube.transform.position.z;
    }

    void SplitCubeX(float kalicakOlan , float direction)
    {
        float xSize = LastCube.transform.localScale.x - Mathf.Abs(kalicakOlan);
        float dusecekSize = transform.localScale.x - xSize;

        float newCoordinate = LastCube.transform.position.x + (kalicakOlan/2) ;
        transform.localScale = new Vector3(xSize , transform.localScale.y , transform.localScale.z);
        transform.position = new Vector3(newCoordinate , transform.position.y , transform.position.z);

        float cubeEdge = transform.position.x + (xSize /2f  * direction);
        float fallingBlockXPosition =  cubeEdge + dusecekSize / 2f * direction;
        SpawnDropCube(fallingBlockXPosition, dusecekSize);
    }
    void SplitCubeZ(float kalicakOlan , float direction)
    {
        float zSize = LastCube.transform.localScale.z - Mathf.Abs(kalicakOlan);
        float dusecekSize = transform.localScale.z - zSize;

        float newCoordinate = LastCube.transform.position.z + (kalicakOlan/2) ;
        transform.localScale = new Vector3( transform.localScale.x , transform.localScale.y , zSize);
        transform.position = new Vector3( transform.position.x , transform.position.y , newCoordinate);

        float cubeEdge = transform.position.z + (zSize /2f  * direction);
        float fallingBlockZPosition =  cubeEdge + dusecekSize / 2f * direction;
        SpawnDropCube(fallingBlockZPosition, dusecekSize);
    }



    void SpawnDropCube(float fallingBlockZPosition , float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if(moveDirection == MoveDirection.X)
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
        }
        else
        {
            cube.transform.localScale = new Vector3( transform.localScale.x, transform.localScale.y , fallingBlockSize);
            cube.transform.position = new Vector3( transform.position.x, transform.position.y , fallingBlockZPosition);
        }
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>() .material.color;
        Destroy(cube.gameObject, 1f);
    }

}