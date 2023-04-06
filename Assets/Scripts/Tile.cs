using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    //public Vector3 initialScale;
    //public Vector2Int position;
    private int selectedIconIndex;

    public int x;
    public int y;

    public bool isSelected = false;


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

    internal void IncreaseScale(float inscreaseValue, float increaseDuration)
    {
        //transform.DOScale(transform.localScale + new Vector3(inscreaseValue, inscreaseValue, inscreaseValue), increaseDuration);
        if (transform == null)
        {
            return;
        }
        transform.DOScale(transform.localScale + Vector3.one * 0.05f * inscreaseValue, increaseDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                if (transform != null)
                {
                    transform.DOScale(Vector3.one*0.5f, increaseDuration);
                }
            });
    }

}






