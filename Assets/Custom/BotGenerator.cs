using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BotGenerator : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject botGameObject;

    public GameController gameController;
    public float botScale;

    // This script will simply instantiate the Prefab when the game starts.
    private void Start()
    {
    }

    //봇들을 빈칸에만 생성한다.
    public void GenerateBots(int[,] mapData, int amount)
    {
        var openSpaces = gameController.OpenSpaces;
        for (int i = 0; i < amount; i++)
        {
            var spawnPlace = openSpaces[Random.Range(0, openSpaces.Count)];
            var newBot = Instantiate(botGameObject, spawnPlace, Quaternion.identity);
            newBot.transform.localScale = Vector3.one * botScale;
            newBot.gameObject.name = $"Bot{i}";
        }
        Debug.Log($"spawned{amount}");
    }
}