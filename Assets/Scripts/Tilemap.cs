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
    private readonly List<Tile> _selection = new List<Tile>();
    List<GameObject> selectedObjects = new List<GameObject>();
    string spriteName = "TripleM_PlayObject1";

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
                //Sprite icon = icons[Random.Range(0, icons.Length)];
                //go.GetComponent<SpriteRenderer>().sprite = icon;
                tilemap[j, i] = go.GetComponent<Tile>();
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
                selectedObjects.Add(selectedObject);

                //Debug.Log("Selected object: " + selectedObject.name);

            }

            List<GameObject> objectsToDestroy = new List<GameObject>();
            int destroyCount = 0;

            foreach (GameObject obj in selectedObjects)
            {
                if (obj.GetComponent<SpriteRenderer>().sprite.name == spriteName)
                {
                    objectsToDestroy.Add(obj);
                    destroyCount++;
                }
            }

            if (destroyCount == 3)
            {
                //foreach (GameObject obj in objectsToDestroy)
                //{
                //    obj.SetActive(false);
                //}
                //objectsToDestroy.RemoveAll(obj => !obj.activeSelf);

                foreach (GameObject obj in objectsToDestroy)
                {
                    Destroy(obj);
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
            //DestroyGameObject();
            selectedObjects.Clear();
           
        }
        
    }
        


}
