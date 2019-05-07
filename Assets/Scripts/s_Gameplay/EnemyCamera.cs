using UnityEngine;
using System.Collections;

public class EnemyCamera : MonoBehaviour 
{
    public GameObject enemyCenter;
	public EnemyAI enemyAI;
    //public GameObject visualGameObject;
    public GameObject spriteSheet;
    private Camera thisCamera;
    private Transform firstPersonTransform;
    //public GameObject oldEnemyObject;
    //public GameObject newEnemyObject;
    //private GameObject newEnemyObjectMesh;
	public SkinnedMeshRenderer EnemyRenderer { get; set; }

    private Material enemySpriteSheetMaterial;
	public RenderTexture EnemyRenderTexture { get; private set; }

	private GameManager mainGame;
    private Player player;
    private float cameraYOffset;

    //test
    private int renderCounterTime = 120;
    private int renderCounter = 0;
	public static float debugCounter = 0;
	public static float debugTimer = 0;

    void Start()
    {
        mainGame = GameManager.MainGame;

        player = mainGame.Player.GetComponent<Player>();
        cameraYOffset = player.PlayerViewCamPosition.position.y - player.transform.position.y;

        if (mainGame.UseTestEnemyMesh)
        {
			//visualGameObject = newEnemyObject;
			//newEnemyObject.SetActive(true);
			//newEnemyObjectMesh = Instantiate(mainGame.EnemyMeshTest, newEnemyObject.transform.position + new Vector3(0, -1.0f, 0), newEnemyObject.transform.rotation);
   //         newEnemyObjectMesh.transform.parent = newEnemyObject.transform;
			//enemyRenderer = newEnemyObjectMesh.GetComponentInChildren<SkinnedMeshRenderer>();
		}
        else
        {
            //visualGameObject = oldEnemyObject;
        }
        
        thisCamera = GetComponent<Camera>();
        thisCamera.fieldOfView = 40.0f;

        enemySpriteSheetMaterial = spriteSheet.GetComponent<Renderer>().material;
        firstPersonTransform = GameManager.MainGame.Player.transform;

		EnemyRenderTexture = new RenderTexture(mainGame.SpriteSize, mainGame.SpriteSize, 1);
		EnemyRenderTexture.filterMode = mainGame.SpriteFilterMode;
		EnemyRenderTexture.antiAliasing = (int)mainGame.SpriteAntiAliasLevel;
        thisCamera.targetTexture = EnemyRenderTexture;

        renderCounter = mainGame.SpriteRenderSpeed;

        enemySpriteSheetMaterial.SetTexture("_MainTex", EnemyRenderTexture);

		EnemyRenderer.enabled = true;
		thisCamera.Render();
		EnemyRenderer.enabled = false;

		mainGame.EnemyPreviewSprites[enemyAI.enemyNumber].texture = EnemyRenderTexture;
	}

	//public void UpdateSpriteCamera()
	//{
	//	if(EnemyRenderTexture == null)
	//	{
	//		Debug.Log("what");
	//	}


	//}

    void Update()
    {
        renderCounter--;

        if (mainGame.OkToRenderEnemy && renderCounter <= 0)
        {
			float startTime = Time.realtimeSinceStartup;

			mainGame.OkToRenderEnemy = false;

			EnemyRenderer.enabled = true;
			thisCamera.Render();
			EnemyRenderer.enabled = false;

            mainGame.OkToRenderEnemy = true;
            renderCounter = mainGame.SpriteRenderSpeed;

			debugTimer += Time.realtimeSinceStartup - startTime;
			debugCounter++;

			mainGame.RenderTimeText.text = (debugTimer / debugCounter).ToString();
		}

		transform.position = enemyCenter.transform.position - ((enemyCenter.transform.position - firstPersonTransform.position).normalized * 3) + new Vector3(0, cameraYOffset, 0);
        transform.LookAt(enemyCenter.transform.position, Vector3.up);

        //transform.position = firstPersonTransform.position + new Vector3(0, cameraYOffset, 0);

        //thisCamera.fieldOfView = 115/Vector3.Distance(thisCamera.transform.position, enemyGameObject.transform.position);  //works with quad size of 2

        //thisCamera.fieldOfView = 130 / Vector3.Distance(thisCamera.transform.position, enemyGameObject.transform.position);
    }
}
