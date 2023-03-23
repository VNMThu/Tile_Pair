using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows;

    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength(dimension: 0);
    public int Height => Tiles.GetLength(dimension: 1);

    private readonly List<Tile> _selection = new List<Tile>(); 

    public void Awake() => Instance = this;

    private const float TweenDuration = 0.25f;

    private void Start()
    {
        Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;
                

                tile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];

                Tiles[x, y] = tile;
            }
        }


    }

    public void Select(Tile tile)
    {
        if (!_selection.Contains(tile)) _selection.Add(tile);

        if (_selection.Count < 3) return;

        Debug.Log(message: $"Selected tiles at ({_selection[0].x}, {_selection[0].y}) and ({_selection[1].x}, {_selection[1].y}) and ({_selection[2].x}, {_selection[2].y})");

        _selection.Clear();
    }

    private bool CanPop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (Tiles[x, y].GetConnectedTiles().Count > 2) return true;
            }
        }
        return false;
    }

    private async void Pop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Count < 3) continue;

                var deflatSequence = DOTween.Sequence();
                 foreach (var connectedTile in connectedTiles)
                {
                    
                }

            }
        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.A)) return;
        foreach (var connectedTile in Tiles[0, 0].GetConnectedTiles()) connectedTile.icon.transform.DOScale(1.25f, TweenDuration).Play();
    }
    

}
