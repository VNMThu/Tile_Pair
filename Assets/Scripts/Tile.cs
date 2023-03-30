using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Sprite[] icons;
    private Sprite selectedIcon;

    public int x;
    public int y;



    //Start is called before the first frame update
    void Start()
    {
       
        selectedIcon = icons[Random.Range(0, icons.Length)];
        gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = selectedIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}






