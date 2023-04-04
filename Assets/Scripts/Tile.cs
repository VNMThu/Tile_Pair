using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IconInfo
{
    public Sprite sprite;
    public int index;
}

public class Tile : MonoBehaviour
{
    public IconInfo[] icons;
    public IconInfo selectedIcon;
    private int selectedIconIndex;

    public int x;
    public int y;



    //Start is called before the first frame update
    void Start()
    {

        selectedIcon = icons[Random.Range(0, icons.Length)];
        gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = selectedIcon.sprite;
        selectedIconIndex = Random.Range(0, icons.Length);
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

    internal void SetIcon(IconInfo icon, int index)
    {
        selectedIcon = icon;
        selectedIconIndex = index;
        gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = selectedIcon.sprite;
    }
}






