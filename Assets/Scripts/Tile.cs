using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public Sprite[] icons;
    private Sprite selectedIcon;
    //float size = 0.9f;

    //int row = 10;
    //int col = 6;
    //public Tile[,] tilemap;
    //public GameObject tilePrefab;

    //Start is called before the first frame update
    void Start()
    {
        //tilemap = new Tile[row, col];
        //for (int i = 0; i < col; i++)
        //{
        //    for (int j = 0; j < row; j++)
        //    {
        //        GameObject go = Instantiate(tilePrefab, new Vector3(i * size - (size * col) / 2f + size / 2, j * size - (size * row) / 2f + size / 2, 0), Quaternion.identity);
        //        //GameObject go = Instantiate(gameObject, new Vector3(i * size - (size * col) / 2f + size / 2, j * size - (size * row) / 2f + size / 2, 0), Quaternion.identity);
        //        go.name = i + "-" + j;
        //        Sprite icon = icons[Random.Range(0, icons.Length)];
        //        go.GetComponent<SpriteRenderer>().sprite = icon;
        //    }
        //}
        selectedIcon = icons[Random.Range(0, icons.Length)];
        gameObject.GetComponent<SpriteRenderer>().sprite = selectedIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}






