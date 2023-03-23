using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Tilemap : MonoBehaviour
{

    float size = 0.9f;

    int row = 5;
    int col = 3;
    public Tile[,] tilemap;
    public GameObject tilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = new Tile[row, col];
        for (int i = 0; i<col; i ++)
        {
            for (int j= 0; j<row; j ++)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i*size - ((size*col)/2f)+size/2, j*size - (size*row/2f)+size/2, 0), Quaternion.identity);
                go.name = i + "-" + j;
                tilemap[j, i] = go.GetComponent<Tile>(); 
            }
        }

        /*
        for (float i = 0; i <= 2; i += 0.9f)
        {
            for (float j = 0; j <= 4; j += 0.9f)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                go.name = i + "-" + j;
            }
        }

        for (float i = 0; i >= -2; i -= 0.9f)
        {
            for (float j = 0; j <= 4; j+=0.9f)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                go.name = i + "-" + j;

            }
        }

        for (float i = 0; i <= 2; i += 0.9f)
        {
            for (float j = 0; j >= -4; j -=0.9f)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                go.name = i + "-" + j;

            }
        }

        for (float i = 0; i >= -2; i -= 0.9f)
        {
            for (float j = 0; j >= -4; j -=0.9f)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                go.name = i + "-" + j;

            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Touch Began");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                Vector3 position = transform.position;


                //Matrix4x4 MVPMatrix = mainCamera.projectionMatrix * mainCamera.worldToCameraMatrix * transform.localToWorldMatrix;

                //// Chuyển đổi tọa độ từ tọa độ Unity sang tọa độ ma trận
                //Vector4 transformedPosition = MVPMatrix * new Vector4(objectPosition.x, objectPosition.y, objectPosition.z, 1);

                //// Hiển thị tọa độ trong tọa độ ma trận
                //Debug.Log("Transformed Position: " + transformedPosition);

                
            }
        }

    }

    public void ConverttoMatrixCoordinate(float x, float y)
    {
        transform.position = new Vector3(x / 0.9f, y / 0.9f);
    }
}