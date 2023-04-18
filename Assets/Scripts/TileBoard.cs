using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;


public class TileBoard : MonoBehaviour
{
    float size = 0.76f;
    int row = 10;
    int col = 6;
    float dropSpeed = 0.01f;
    string spriteName;
    public float increaseValue = 0.1f;
    public float increaseDuration = 4f;
    public float decreaseValue = 0.001f;
    public float decreaseDuration = 4f;
    string normalsortingLayerName = "Normal";
    string selectsortingLayerName = "Select";
    


    public IconInfo[] iconInfoList;
    public Tile[,] tilemap;
    public GameObject tilePrefab;

    List<GameObject> selectedObjects = new List<GameObject>();
    List<Vector2Int> destroyedPositions = new List<Vector2Int>();
    List<GameObject> objectsToScaleAndDestroy = new List<GameObject>();
    List<GameObject> aboveTiles = new List<GameObject>();
    List<Vector3> dropPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
        CreateTile();
    }

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject selectedObject = hit.collider.gameObject;
                if (!selectedObjects.Contains(selectedObject))
                {
                    selectedObjects.Add(selectedObject);
                    selectedObject.GetComponent<Tile>().IncreaseScale(increaseValue, increaseDuration);
                }
                else
                {
                    selectedObjects.Remove(selectedObject);
                    selectedObject.GetComponent<Tile>().DecreaseScale(decreaseValue, decreaseDuration);
                    selectedObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                    Renderer[] iconRenderers = selectedObject.GetComponentsInChildren<Renderer>();
                    foreach (Renderer ren in iconRenderers)
                    {
                        ren.sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                    }
                }
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
                        selectedObject.GetComponent<Tile>().DecreaseScale(decreaseValue, decreaseDuration);
                        selectedObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                        Renderer[] iconRenderers = selectedObject.GetComponentsInChildren<Renderer>();
                        foreach (Renderer ren in iconRenderers)
                        {
                            ren.sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                        }
                    }
                }
                else
                {
                    foreach (GameObject obj in selectedObjects)
                    {
                        obj.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID(selectsortingLayerName);
                        Renderer[] iconRenderers = obj.GetComponentsInChildren<Renderer>();
                        foreach (Renderer ren in iconRenderers)
                        {
                            ren.sortingLayerID = SortingLayer.NameToID(selectsortingLayerName);
                            ren.sortingOrder += 1;
                        }
                    }
                }
            }
        }
        DropTile();
    }

    void DropTile()
    {
        float dropSpeed = 5.0f;
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
                        StartCoroutine(DropTileCoroutine(tilemap[emptyY, x], x, emptyY, dropSpeed));
                        tilemap[emptyY, x].SetPosition(x, emptyY);
                        Vector3 pos = tilemap[emptyY, x].transform.position;
                        pos.y = emptyY * size - (size * row) / 2f + size / 2;
                        tilemap[emptyY, x].transform.position = pos;
                        emptyY++;
                        tilemap[emptyY - 1, x].name = x + "-" + (emptyY - 1);

                        Renderer tileRenderer = tilemap[emptyY - 1, x].GetComponent<Renderer>();
                        if (tileRenderer != null)
                        {
                            tileRenderer.sortingOrder = (row - emptyY) * col + x;
                        }

                        Renderer[] iconRenderers = tilemap[emptyY - 1, x].GetComponentsInChildren<Renderer>();
                        foreach (Renderer iconRenderer in iconRenderers)
                        {
                            if (iconRenderer != tileRenderer)
                            {
                                iconRenderer.sortingOrder = tileRenderer.sortingOrder;
                            }
                        }
                    }
                }
            }
            if (emptyY != -1)
            {
                GameObject newtile = Instantiate(tilePrefab, transform);
                newtile.GetComponent<Tile>().SetPosition(x, emptyY);
                tilemap[emptyY, x] = newtile.GetComponent<Tile>();
                StartCoroutine(DropTileCoroutine(tilemap[emptyY, x], x, emptyY, dropSpeed));
                newtile.name = x + "-" + emptyY;
                Vector3 pos = newtile.transform.position;
                pos.x = x * size - (size * col) / 2f + size / 2;
                pos.y = emptyY * size - (size * row) / 2f + size / 2;
                pos.z = 0;
                newtile.transform.position = pos;

                Renderer newTileRenderer = newtile.GetComponent<Renderer>();
                if (newTileRenderer != null)
                {
                    newTileRenderer.sortingOrder = (row - emptyY) * col + x;
                }

                Renderer renderer = newtile.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                    renderer.sortingOrder = (row - emptyY - 1) * col + x;
                }

                Renderer[] iconRenderers = newtile.GetComponentsInChildren<Renderer>();
                foreach (Renderer ren in iconRenderers)
                {
                    ren.sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                    ren.sortingOrder = (row - emptyY - 1) * col + x;
                }
                
            }
        }
    }

    IEnumerator DropTileCoroutine(Tile tile, int x, int y, float dropSpeed)
    {
        float startY = tile.transform.position.y;
        float endY = y * size - (size * row) / 2f + size / 2;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * dropSpeed;
            tile.transform.position = new Vector3(x * size - (size * col) / 2f + size / 2, Mathf.Lerp(startY, endY, t), 0.0f);
            yield return null;
        }
        tile.SetPosition(x, y);

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
                if (iconInfoList.Length > 0)
                {
                    selectedIconInfo = iconInfoList[Random.Range(0, iconInfoList.Length)];
                }
                else
                {
                    Debug.LogError("The iconInfoList array is empty!");
                }
                tile.SetIcon(selectedIconInfo, selectedIconInfo.index);

                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                    renderer.sortingOrder = (row - j - 1) * col + i;
                }

                Renderer[] iconRenderers = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer ren in iconRenderers)
                {
                    ren.sortingLayerID = SortingLayer.NameToID(normalsortingLayerName);
                    ren.sortingOrder = (row - j - 1) * col + i;
                }
            }
        }
    }

    void DestroyObject()
    {
        bool isDifferent = false;
        int firstIndex = -1;
        int selectedIconIndex = -1;
        foreach (GameObject obj in selectedObjects)
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile != null)
            {
                selectedIconIndex = tile.selectedIcon.index;
                if (firstIndex == -1)
                {
                    firstIndex = selectedIconIndex;
                }
                else if (selectedIconIndex != firstIndex)
                {
                    isDifferent = true;
                    break;
                }
            }
        }


        if (isDifferent)
        {
            foreach (GameObject obj in selectedObjects)
            {
                Tile tile = obj.GetComponent<Tile>();
                if (tile != null)
                {
                    obj.transform.localScale = tile.originalScale;

                    SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sortingOrder = tile.initialSortingOrder;
                    }
                    tile.GetComponent<Renderer>().sortingLayerName = "Normal";
                }
            }
            selectedObjects.Clear();
        }

        //List<GameObject> objectsToScaleAndDestroy = new List<GameObject>();
        List<Vector3> originalScales = new List<Vector3>();
        foreach (GameObject obj in objectsToScaleAndDestroy)
        {
            if (obj != null)
            {
                originalScales.Add(obj.transform.localScale);
            }
        }
        objectsToScaleAndDestroy.Clear();

        foreach (GameObject obj in selectedObjects)
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile != null && tile.selectedIcon.index == selectedIconIndex)
            {
                objectsToScaleAndDestroy.Add(obj);
            }
        }

        if (objectsToScaleAndDestroy.Count == 3)
        {
            foreach (GameObject obj in objectsToScaleAndDestroy)
            {
                Tile tile = obj.GetComponent<Tile>();
                if (tile != null)
                {
                    Vector3 dropPosition = new Vector3(tile.transform.position.x, tile.transform.position.y - 1f, tile.transform.position.z);
                    dropPositions.Add(dropPosition);
                }

            }
            StartCoroutine(ScaleAndDestroyObjects(objectsToScaleAndDestroy));
        }
    }
    

    IEnumerator ScaleDownAndReset(GameObject obj, Vector3 originalScale)
    {
        float duration = 0.1f;
        float timer = 0.0f;

        Vector3 targetScale = originalScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, targetScale, timer / duration);
            yield return null;
        }

        obj.transform.localScale = originalScale;
    }


    IEnumerator ScaleAndDestroyObjects(List<GameObject> objectsToScaleAndDestroy)
    {

        foreach (GameObject obj in objectsToScaleAndDestroy)
        {
            Vector3 originalScale = obj.transform.localScale;
            Vector3 targetScale = originalScale * 1.1f;
            float elapsedTime = 0;
            float duration = 0.1f;
            while (elapsedTime < duration)
            {
                obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.localScale = targetScale;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject obj in objectsToScaleAndDestroy)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in objectsToScaleAndDestroy)
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile != null)
            {
                Vector2Int position = new Vector2Int(tile.x, tile.y);
                Debug.Log("Object destroyed at position: " + position);
                destroyedPositions.Add(position);
            }
        }
    }

    

}




