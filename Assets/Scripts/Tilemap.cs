using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Tilemap : MonoBehaviour
{
    float size = 0.9f;
    int row = 10;
    int col = 6;
    string spriteName;

    public Tile[,] tilemap;
    public GameObject tilePrefab;

    List<GameObject> selectedObjects = new List<GameObject>();
    
    List<Vector2Int> destroyedPositions = new List<Vector2Int>();


    // Start is called before the first frame update
    void Start()
    {
        tilemap = new Tile[row, col];
        for (int i = 0; i<col; i ++)
        {
            for (int j = 0; j < row; j ++)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i*size - (size*col)/2f + size/2, j*size - (size*row)/2f + size/2, 0), Quaternion.identity, this.transform);
                go.name = i + "-" + j;
                Tile tile = go.GetComponent<Tile>();
                tile.SetPosition(i, j);
                tilemap[j, i] = tile;
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            
            //Debug.Log("Touch Began");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                //Debug.Log(hit.collider.gameObject.name);

                //Vector3 position = transform.position;
                GameObject selectedObject = hit.collider.gameObject;
                if (!selectedObjects.Contains(selectedObject))
                {
                    selectedObjects.Add(selectedObject);
                    if (selectedObjects.Count == 3)
                    {
                        HashSet<GameObject> uniqueObjects = new HashSet<GameObject>(selectedObjects);
                        if (uniqueObjects.Count == 3)
                        {
                            int destroyCount = 0;
                            List<Vector2Int> positiontoDestroy = new List<Vector2Int>();
                            List<GameObject> objectsToDestroy = new List<GameObject>();
                            foreach (GameObject obj in selectedObjects)
                            {
                                if (obj.GetComponentsInChildren<SpriteRenderer>()[1].sprite.name == spriteName)
                                {
                                    Tile tile = obj.GetComponent<Tile>();
                                    Vector2Int position = new Vector2Int(tile.x, tile.y);
                                    positiontoDestroy.Add(position);
                                    objectsToDestroy.Add(obj);
                                    destroyCount++;
                                }
                            }

                            if (destroyCount == 3)
                            {
                                foreach (GameObject obj in objectsToDestroy)
                                {
                                    Destroy(obj);
                                }

                                foreach (Vector2Int position in positiontoDestroy)
                                {
                                    Debug.Log("Object destroyed at position: " + position);
                                    destroyedPositions.Add(position);
                                }
                            }
                            selectedObjects.Clear();
                        }
                        else
                        {
                            selectedObjects.Remove(selectedObject);
                        }
                    }
                }
                //selectedObjects.Add(selectedObject);
                //Debug.Log("Selected object: " + selectedObject.name);

            }
            if (selectedObjects.Count > 0)
            {
                SpriteRenderer spriteRenderer = selectedObjects[0].GetComponentsInChildren<SpriteRenderer>()[1];

                if (spriteRenderer != null)
                {
                    spriteName = spriteRenderer.sprite.name;
                }
            }
             
        }
        if (selectedObjects.Count >= 3)
        {
            List<string> selectedObjectNames = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                selectedObjectNames.Add(selectedObjects[i].name);
            }
            Debug.Log("Selected objects: " + string.Join(", ", selectedObjectNames.ToArray()));
            selectedObjects.Clear();
           
        }
        
        for (int x = 0; x < col; x++)
        {
            for (int y = 1; y <row; y++)
            {
                if (tilemap[y, x] != null && tilemap[y-1, x] == null)
                {
                    int currentY = y;
                    while (currentY > 0 && tilemap[currentY -1, x] == null)
                    {
                        currentY--;
                    }
                    //tilemap[currentY, x] = tilemap[y, x];
                    //tilemap[y, x] = null;
                    tilemap[currentY, x] = tilemap[y, x];
                    tilemap[y, x] = null;
                    tilemap[currentY, x].SetPosition(x, currentY);
                    Vector3 pos = tilemap[currentY, x].transform.position;
                    pos.y = currentY * size - (size * row) / 2f + size / 2;
                    tilemap[currentY, x].transform.position = pos;
                }
            }
        }

        for (int x = 0; x < col; x++)
        {
            int emptyY = -1;
            for (int y = 0; y < row; y++)
            {
                if (tilemap[y, x] == null)
                {
                    if (emptyY == -1) 
                    {
                        emptyY = y;
                    }
                }
                else
                {
                    if (emptyY != -1)
                    {
                        tilemap[emptyY, x] = tilemap[y, x];
                        tilemap[y, x] = null;
                        tilemap[emptyY, x].SetPosition(x, emptyY);
                        Vector3 pos = tilemap[emptyY, x].transform.position;
                        pos.y = emptyY * size - (size * row) / 2f + size / 2;
                        tilemap[emptyY, x].transform.position = pos;
                        emptyY++; 
                    }
                }
            }
            if (emptyY != -1) 
            {
                GameObject newTile = Instantiate(tilePrefab, transform);
                newTile.GetComponent<Tile>().SetPosition(x, emptyY);
                tilemap[emptyY, x] = newTile.GetComponent<Tile>();
                Vector3 pos = newTile.transform.position;
                pos.x = x * size - (size * col) / 2f + size / 2;
                pos.y = emptyY * size - (size * row) / 2f + size / 2;
                newTile.transform.position = pos;
            }
        }

    }

}

