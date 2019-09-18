/* GameState.cs
 * Handles randomizing colored tile pairs, keeping track of the currently selected tile,
 * and checking whether the user has won yet
 *
 * Melody Mao
 * Homework 2
 * Fall 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public GameObject cursor; //hovers over currently selected tile
    public Tile selectedTile;
    public int tilesLeft;
    public Text winMessage; //displays when user wins
    
    private GameObject[] tiles;
    private Color[] colors; // array of colors that tiles can be
    private System.Random random; // differentiated from Unity's Random
    
    // Start is called before the first frame update
    void Start()
    {
        //set up fields
        tiles = GameObject.FindGameObjectsWithTag("Interactable");
        Color orange = new Color(1.0f, 0.6f, 0.2f, 1.0f);
        colors = new Color[] {Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta,
                              orange, Color.grey};
        random = new System.Random();
        this.Reset();
    }

    public void Reset()
    {
        tilesLeft = tiles.Length;
        
        cursor.SetActive(false);
        
        //generate a random order to traverse through and color tile array
        int[] indices = Enumerable.Range(0, tiles.Length).OrderBy(c => random.Next()).ToArray();
        
        int id = 0; //index of current color in color list
        int tilesPerColor = tiles.Length / colors.Length;
        for (int i = 0; i < indices.Length; i++)
        {
            int idx = indices[i];
            tiles[idx].SetActive(true); //make all tiles visible again
            tiles[idx].GetComponent<Renderer>().material.color = colors[id];
            tiles[idx].GetComponent<Tile>().id = id;

            //switch to next color after correct number of tiles
            if (i % tilesPerColor == tilesPerColor - 1)
            {
                id++;
            }
        }
    }

    /* checks whether the board is empty and displays a win message if it is */
    public void checkWin()
    {
        if (tilesLeft < 1)
        {
            winMessage.gameObject.SetActive(true);
        }
    }
}
