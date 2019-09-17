/* Tile.cs
 * Represents a tile in mahjongg; implements OnTouch3D to allow user interaction
 * for tile selection and checking pairs of tiles against each other
 *
 * Melody Mao
 * Homework 2
 * Fall 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, OnTouch3D
{
    //time to wait before letting the tile be tapped again
    public float debounceTime = 0.3f;
    public GameState gameState;
    public int id;
    
    // Stores a counter for the current remaining wait time.
    private float remainingDebounceTime;
    //whether this tile is currently selected (and highlighted)
    private bool selected;

    void Start()
    {
        this.remainingDebounceTime = 0;
        this.selected = false;
    }

    void Update()
    {
        // Time.deltaTime stores the time since the last update.
        // So all we need to do here is subtract this from the remaining
        // time at each update.
        if (remainingDebounceTime > 0)
            remainingDebounceTime -= Time.deltaTime;
    }

    /* set tile to selected and move cursor over it */
    void Select()
    {
        selected = true;

        //update GameState's stored tile
        if (gameState.selectedTile != null)
            gameState.selectedTile.Deselect();
        gameState.selectedTile = this;

        //move cursor
        gameState.cursor.SetActive(true);
        gameState.cursor.transform.position = this.gameObject.transform.position + new Vector3(0.0f, 0.01f, 0.0f);
    }

    /* set tile to not selected and turn off cursor */
    void Deselect()
    {
        selected = false;

        gameState.selectedTile = null;

        gameState.cursor.SetActive(false);
    }

    /* returns whether this tile is the same color as the given tile */
    private bool doesColorMatch(Tile otherTile)
    {
        Color color1 = otherTile.gameObject.GetComponent<Renderer>().material.color;
        Color color2 = this.gameObject.GetComponent<Renderer>().material.color;
        return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a;
    }
    
    public void OnTouch()
    {
        // If a touch is found and we are not waiting,
        if (remainingDebounceTime <= 0)
        {
            //toggle whether selected
            if (selected)
            {
                this.Deselect();
            }
            else
            {
                //if id matches previous selected tile, both disappear
                if (gameState.selectedTile != null && this.id == gameState.selectedTile.id)
                {
                    this.gameObject.SetActive(false);
                    gameState.selectedTile.gameObject.SetActive(false);
                    gameState.selectedTile.Deselect();

                    gameState.tilesLeft -= 2;
                    gameState.checkWin();
                }
                else //otherwise just select this one
                {
                    this.Select();
                }
            }

            remainingDebounceTime = debounceTime;
        }
    }
}
