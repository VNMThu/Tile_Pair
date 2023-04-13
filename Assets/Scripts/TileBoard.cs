﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;


public class TileBoard : MonoBehaviour
{
    float size = 0.76f;
    int row = 10;
    int col = 6;
    string spriteName;
    public float increaseValue = 0.01f;
    public float increaseDuration = 0.5f;
    string sortingLayerName = "Normal";

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
                    selectedObject.GetComponent<Tile>().IncreaseScale(increaseValue, increaseDuration);
                    if (selectedObjects.Count == 3)
                    {
                        HashSet<GameObject> uniqueObjects = new HashSet<GameObject>(selectedObjects);
                        if (uniqueObjects.Count == 3 )
                        {
                            DestroyObject();
                            ResetLayerOrder();
                            selectedObjects.Clear();
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

    /*
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
                        tilemap[emptyY - 1, x].name = x + "-" + (emptyY - 1);
                    }
                }
            }
            if (emptyY != -1)
            {
                GameObject newtile = Instantiate(tilePrefab, transform);
                newtile.GetComponent<Tile>().SetPosition(x, emptyY);
                tilemap[emptyY, x] = newtile.GetComponent<Tile>();
                newtile.name = x + "-" + emptyY;
                Vector3 pos = newtile.transform.position;
                pos.x = x * size - (size * col) / 2f + size / 2;
                pos.y = emptyY * size - (size * row) / 2f + size / 2;
                pos.z = 0;
                newtile.transform.position = pos;
            }
        }
    }
    
    */

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
                        tilemap[emptyY - 1, x].name = x + "-" + (emptyY - 1);

                        // Set sortingOrder for the tile renderer
                        Renderer tileRenderer = tilemap[emptyY - 1, x].GetComponent<Renderer>();
                        if (tileRenderer != null)
                        {
                            tileRenderer.sortingOrder = (row - emptyY) * col + x;
                        }

                        // Set sortingOrder for all icon renderers
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
                newtile.name = x + "-" + emptyY;
                Vector3 pos = newtile.transform.position;
                pos.x = x * size - (size * col) / 2f + size / 2;
                pos.y = emptyY * size - (size * row) / 2f + size / 2;
                pos.z = 0;
                newtile.transform.position = pos;

                // Set sortingOrder for the new tile renderer
                Renderer newTileRenderer = newtile.GetComponent<Renderer>();
                if (newTileRenderer != null)
                {
                    newTileRenderer.sortingOrder = (row - emptyY) * col + x;
                }

                // Set sortingOrder for all icon renderers in the new tile
                Renderer renderer = newtile.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sortingLayerID = SortingLayer.NameToID("Normal");
                    renderer.sortingOrder = (row - emptyY - 1) * col + x;
                }

                Renderer[] iconRenderers = newtile.GetComponentsInChildren<Renderer>();
                foreach (Renderer ren in iconRenderers)
                {
                    ren.sortingLayerID = SortingLayer.NameToID("Normal");
                    ren.sortingOrder = (row - emptyY - 1) * col + x;
                }
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
                    renderer.sortingLayerID = SortingLayer.NameToID(sortingLayerName);
                    renderer.sortingOrder = (row - j - 1) * col + i;
                }
                
                Renderer[] iconRenderers = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer ren in iconRenderers)
                {
                    ren.sortingLayerID = SortingLayer.NameToID(sortingLayerName);
                    ren.sortingOrder = (row - j - 1) * col + i;
                }
                
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

        List<GameObject> objectsToScaleAndDestroy = new List<GameObject>();
        List<Vector3> originalScales = new List<Vector3>();
        foreach (GameObject obj in objectsToScaleAndDestroy)
        {
            originalScales.Add(obj.transform.localScale);
        }
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
            Vector3 targetScale = originalScale * 1.5f;
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
    void ResetLayerOrder()
    {
        for (int x = 0; x < col; x++)
        {
            for (int y = row - 1; y >= 0; y--)
            {
                if (tilemap[y, x] != null)
                {
                    tilemap[y, x].transform.SetSiblingIndex(y * col + x);
                    tilemap[y, x].GetComponent<SpriteRenderer>().sortingOrder = (row - y - 1) * col + x;
                    tilemap[y, x].GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID(sortingLayerName);
                    foreach (Transform child in tilemap[y, x].transform)
                    {
                        child.GetComponent<SpriteRenderer>().sortingOrder = tilemap[y, x].GetComponent<SpriteRenderer>().sortingOrder + 1;
                    }
                }
            }
        }
    }
}




