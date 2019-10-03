using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Sprites;

public class LevelEditorWindow : EditorWindow
{
    int numberOfTilesWidth = 0;
    int numberOfTilesHeight = 0;
    GameObject[,] tileHolder;

    GameObject[] tilePrefabsHolder;

    GameObject selectedTilePrefab;

    [MenuItem("Window/LevelEditor")]
    static void Init()
    {
        LevelEditorWindow window = (LevelEditorWindow)EditorWindow.GetWindow(typeof(LevelEditorWindow));
        window.Show();
    }
    private void OnGUI()
    {
        numberOfTilesWidth = EditorGUILayout.DelayedIntField(numberOfTilesWidth);
        numberOfTilesHeight = EditorGUILayout.DelayedIntField(numberOfTilesWidth);
        if (GUILayout.Button("Create Level"))
        {
            CreateLevel();
        }
        AddTileSelectorButton();
        if(GUILayout.Button("Deselect"))
        {
            selectedTilePrefab = null;
        }
    }

    private void OnInspectorUpdate()
    {
        if(selectedTilePrefab != null)
        {
            if(Selection.activeGameObject != null 
                && Selection.activeGameObject.GetComponent<TileElementEditor>() != null)
            {
                Selection.activeGameObject.GetComponent<TileElementEditor>().ReplaceWithNewTile(selectedTilePrefab);
            }
        }
    }

    void CreateLevel()
    {
        tileHolder = new GameObject[numberOfTilesWidth, numberOfTilesHeight];
        GetTilePrefabs();
        FillTileHolderWithBlanks();
    }

    void FillTileHolderWithBlanks()
    {
        for(int i = 0; i < numberOfTilesWidth; i++)
        {
            for(int j = 0; j < numberOfTilesHeight; j++)
            {
                tileHolder[i, j] = Instantiate(tilePrefabsHolder[0], new Vector3(i, j, 0), Quaternion.identity);
            }
        }
    }

    void ChangeTileAt(int tileWidthNumber, int tileHeightNumber, GameObject replacementTile)
    {
        if (CheckIfInTileHolderBounds(tileWidthNumber, tileHeightNumber))
        {
            tileHolder[tileWidthNumber, tileHeightNumber] = replacementTile;
        } else
        {
            Debug.LogError("Tile is not inside bounds of tileHolder. tileWidthNumber is: " + tileWidthNumber
                + ", while tileHolder width length is: " + tileHolder.GetLength(0)
                + ", tileHeightNumber is: " + tileHeightNumber
                +", while tileHolder height length is: " + tileHolder.GetLength(1));
        }
    }

    bool CheckIfInTileHolderBounds(int width, int height)
    {
        if (tileHolder != null)
        {
            if (0 <= width && width < tileHolder.GetLength(0))
            {
                if (0 <= height && height < tileHolder.GetLength(1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void GetTilePrefabs()
    {
        Object[] asset = Resources.LoadAll("Tiles");
        tilePrefabsHolder = new GameObject[asset.Length];
        for(int i = 0; i < asset.Length; i++)
        {
            tilePrefabsHolder[i] = asset[i] as GameObject;
        }
        Debug.Log(tilePrefabsHolder.Length);
    }

    void AddTileSelectorButton()
    {
        if (tilePrefabsHolder != null)
        {
            for (int i = 0; i < tilePrefabsHolder.Length; i++)
            {
                Texture icon = AssetPreview.GetAssetPreview(tilePrefabsHolder[i]);
                if(GUI.Button(new Rect(50*i, 75, 50, 50), new GUIContent(icon)))
                {
                    selectedTilePrefab = tilePrefabsHolder[i];
                }
            }
        }
    }
}
