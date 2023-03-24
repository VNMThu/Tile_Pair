using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Tilemap : MonoBehaviour
{

    float size = 0.9f;

    int row = 10;
    int col = 6;
    public Tile[,] tilemap;
    public GameObject tilePrefab;
    public Sprite[] icons;

    public Sprite sprite { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        tilemap = new Tile[row, col];
        for (int i = 0; i<col; i ++)
        {
            for (int j= 0; j<row; j ++)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i*size - (size*col)/2f + size/2, j*size - (size*row)/2f + size/2, 0), Quaternion.identity);
                go.name = i + "-" + j;
                Sprite icon = icons[Random.Range(0, icons.Length)];
                go.GetComponent<SpriteRenderer>().sprite = icon;
                //tilemap[j, i] = go.GetComponent<Tile>();
            }
        }

        
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
            }
        }

    }

    public void ConverttoMatrixCoordinate(float x, float y)
    {
        transform.position = new Vector3(x / 0.9f, y / 0.9f);
    }
}