using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Tilemap : MonoBehaviour
{
    float size = 0.76f;
    int row = 10;
    int col = 6;
    string spriteName;

    public IconInfo[] iconInfoList;
    public Tile[,] tilemap;
    public GameObject tilePrefab;

    List<GameObject> selectedObjects = new List<GameObject>();
    List<Vector2Int> destroyedPositions = new List<Vector2Int>();


    // Start is called before the first frame update
    void Start()
    {
        CreateTile();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
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
                            DestroyObject();
                            selectedObjects.Clear();
                        }
                        else
                        {
                            selectedObjects.Remove(selectedObject);
                        }
                    }
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
        DropTile();

    }


    void DropTile()
    {
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
                GameObject newtile = Instantiate(tilePrefab, transform);
                newtile.GetComponent<Tile>().SetPosition(x, emptyY);
                tilemap[emptyY, x] = newtile.GetComponent<Tile>();
                Vector3 pos = newtile.transform.position;
                pos.x = x * size - (size * col) / 2f + size / 2;
                pos.y = emptyY * size - (size * row) / 2f + size / 2;
                pos.z = 0;
                newtile.transform.position = pos;
            }
        }
    }

    void CreateTile()
    {
        tilemap = new Tile[row, col];
        for (int i = 0; i < col; i++)
        {
            for (int j = row - 1; j >= 0; j--)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(i * size - (size * col) / 2f + size / 2, j * size - (size * row) / 2f + size / 2, 0), Quaternion.identity, this.transform);
                go.name = i + "-" + j;
                Tile tile = go.GetComponent<Tile>();
                tile.SetPosition(i, j);
                tilemap[j, i] = tile;
                IconInfo selectedIconInfo = iconInfoList[Random.Range(0, iconInfoList.Length)];
                tile.SetIcon(selectedIconInfo, selectedIconInfo.index);
                if (iconInfoList.Length > 0)
                {
                    selectedIconInfo = iconInfoList[Random.Range(0, iconInfoList.Length)];
                }
                else
                {
                    Debug.LogError("The iconInfoList array is empty!");
                }
                tile.SetIcon(selectedIconInfo, selectedIconInfo.index);
            }
        }
    }
    void DestroyObject()
    {
        int selectedIconIndex = -1;
        if (selectedObjects.Count > 0)
        {
            Tile tile = selectedObjects[0].GetComponent<Tile>();
            if (tile != null)
            {
                selectedIconIndex = tile.selectedIcon.index;
            }
        }
        int destroyCount = 0;
        List<Vector2Int> positiontoDestroy = new List<Vector2Int>();
        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in selectedObjects)
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile != null && tile.selectedIcon.index == selectedIconIndex)
            {
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
    }

}


