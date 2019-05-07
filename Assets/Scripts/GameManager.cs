using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameState { On, Off }

public enum SpriteAntiAliasLevelEnum { Low = 1, Medium = 2, High = 4, UltraHigh = 8 }

public class GameManager : MonoBehaviour 
{
    private static GameManager mainGame;
    public List<EnemyAI> Enemies { get; private set; }
    private GameState currentGameState;
    public Camera startCamera;
    public GameObject playerPrefab;
    public GameObject Player { get; private set; }
    public GameObject playerSpawn1;
    private bool okToRenderEnemy = true;
    public GameObject enemyMeshTestNode;
    private GameObject enemyMeshTest;

	public List<RawImage> EnemyPreviewSprites;
	public Text RenderTimeText;


	[Header("Test Settings")]
    public bool useTestEnemyMesh;
    public int numberOfEnemies;

    [Header("Sprite Settings")]
    public int spriteSize;
    public FilterMode spriteFilterMode;
    public SpriteAntiAliasLevelEnum spriteAntiAliasLevel;
    public int spriteRenderSpeed;

    void Awake()
    {
        mainGame = this;
        currentGameState = GameState.Off;
		Enemies = new List<EnemyAI>();
        enemyMeshTest = enemyMeshTestNode.transform.GetChild(0).gameObject;
		enemyMeshTestNode.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (currentGameState == GameState.Off && Input.GetKeyDown(KeyCode.Space))
        {
            startCamera.gameObject.SetActive(false);
            SpawnPlayer();
            currentGameState = GameState.On;
        }
	}

	public void AddEnemy(EnemyAI newEnemy)
	{
		Enemies.Add(newEnemy);

		for (int x = 0; x < Enemies.Count; x++)
		{
			Enemies[x].enemyNumber = x;

			//if (Enemies[x] != null)
			//{
			//	EnemyPreviewSprites[x].texture = Enemies[x].enemyCamera.EnemyRenderTexture;
			//}
		}
	}

    public void SpawnPlayer()
    {
        Player = (GameObject)Instantiate(playerPrefab, playerSpawn1.transform.position, playerSpawn1.transform.rotation);
    }

    public void KillEnemy(EnemyAI enemyToKill, GameObject enemyShadowBlob)
    {
		Enemies.Remove(enemyToKill);
        Destroy(enemyToKill.gameObject);
        Destroy(enemyShadowBlob);
    }

    public static GameManager MainGame
    {
        get { return mainGame; }
    }

    public GameState CurrentGameState
    {
        get { return currentGameState; }
        set { currentGameState = value; }
    }

    public Camera StartCamera
    {
        get { return startCamera; }
    }

    public GameObject EnemyMeshTest
    {
        get { return enemyMeshTest; }
    }

    public bool UseTestEnemyMesh
    {
        get { return useTestEnemyMesh; }
    }

    public bool OkToRenderEnemy
    {
        get { return okToRenderEnemy; }
        set { okToRenderEnemy = value; }
    }

    public int SpriteSize
    {
        get { return spriteSize; }
    }

    public FilterMode SpriteFilterMode
    {
        get { return spriteFilterMode; }
    }

    public SpriteAntiAliasLevelEnum SpriteAntiAliasLevel
    {
        get { return spriteAntiAliasLevel; }
    }

    public int NumberOfEnemies
    {
        get { return  numberOfEnemies; }
    }

    public int SpriteRenderSpeed
    {
        get { return spriteRenderSpeed; }
    }
}
