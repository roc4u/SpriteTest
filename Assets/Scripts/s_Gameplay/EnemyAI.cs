using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour 
{
    public enum DetectionRadius { VeryNear, Near, Far, VeryFar }
    public DetectionRadius detectionRadius;
    private float detectionRadiusValue;

    public enum CheckRateBase { VeryOften, Often, Seldom, VerySeldom }
    public CheckRateBase checkRateBase;
    private float checkRateValue;

    public enum Speed { VeryFast, Fast, Slow, VerySlow }
    public Speed speed;

    public enum Toughness { VeryTough, Tough, Fragile, VeryFragile }
    public Toughness toughness;
    public int HealthValue { get; set; }

    private Transform myTransform;
    private UnityEngine.AI.NavMeshAgent myNavMeshAgent;
    private Collider[] hitColliders;
    private float nextCheck = 0.0f;
    public LayerMask playerDetectionLayer;

	public GameObject EnemyModel { get; set; }

    public Text healthText;
    public GameObject shadowBlobPrefab;
    private Projector shadowBlobProjector;
    private CapsuleCollider enemyCollider;
    public GameObject eyes;

	public EnemyCamera enemyCamera;
	public int enemyNumber = 0;
	public static List<Color> enemyColors;

	public Color enemyColor = Color.white;


	private bool inAir = false;

	void Start()
	{
		if(enemyColors == null)
		{
			enemyColors = new List<Color>() { new Color(.5f, 1, .5f), new Color(.5f, .5f, 1), new Color(1, .5f, .5f) };
		}

		SetInitialReferences();
		enemyCollider = this.gameObject.GetComponent<CapsuleCollider>();
		shadowBlobPrefab = (GameObject)Instantiate(shadowBlobPrefab, transform.TransformPoint(enemyCollider.center), Quaternion.Euler(new Vector3(90, 0, 0)));
		shadowBlobProjector = shadowBlobPrefab.GetComponent<Projector>();
		EnemyModel = Instantiate(GameManager.MainGame.EnemyMeshTest, transform.position + new Vector3(0, 0.0f, 0), transform.rotation);
		EnemyModel.transform.parent = transform;
		enemyCamera.EnemyRenderer = EnemyModel.GetComponentInChildren<SkinnedMeshRenderer>();

		EnemyModel.GetComponent<EnemyInfo>().gameModel.GetComponent<Renderer>().material.SetColor("_Color", enemyColors[enemyNumber]);
		//enemyCamera.UpdateSpriteCamera();
	}

	void Update()
    {
        shadowBlobPrefab.transform.position = transform.TransformPoint(enemyCollider.center);
        shadowBlobProjector.fieldOfView = 120 - (transform.TransformPoint(enemyCollider.center).y * 30.0f);
    }

    void SetInitialReferences()
    {
        myTransform = transform;
        myNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		switch (detectionRadius)
        {
            case DetectionRadius.VeryNear:
                detectionRadiusValue = 20.0f;
                break;
            case DetectionRadius.Near:
                detectionRadiusValue = 30.0f;
                break;
            case DetectionRadius.Far:
                detectionRadiusValue = 40.0f;
                break;
            case DetectionRadius.VeryFar:
                detectionRadiusValue = 50.0f;
                break;
        }

        switch (checkRateBase)
        {
            case CheckRateBase.VeryOften:
                checkRateValue = Random.Range(-0.15f, 0.15f) + 0.5f;
                break;
            case CheckRateBase.Often:
                checkRateValue = Random.Range(-0.15f, 0.15f) + 0.8f;
                break;
            case CheckRateBase.Seldom:
                checkRateValue = Random.Range(-0.15f, 0.15f) + 1.1f;
                break;
            case CheckRateBase.VerySeldom:
                checkRateValue = Random.Range(-0.15f, 0.15f) + 1.4f;
                break;
        }

        switch (speed)
        {
            case Speed.VeryFast:
                myNavMeshAgent.speed = 5.0f;
                break;
            case Speed.Fast:
                myNavMeshAgent.speed = 4.0f;
                break;
            case Speed.Slow:
                myNavMeshAgent.speed = 3.0f;
                break;
            case Speed.VerySlow:
                myNavMeshAgent.speed = 2.0f;
                break;
        }

        switch (toughness)
        {
            case Toughness.VeryTough:
				HealthValue = 200;
                break;
            case Toughness.Tough:
				HealthValue = 150;
                break;
            case Toughness.Fragile:
				HealthValue = 100;
                break;
            case Toughness.VeryFragile:
				HealthValue = 50;
                break;
        }

        healthText.text = HealthValue.ToString();
    }

    void CheckIfPlayerInRange()
    {
        if (Time.time > nextCheck && myNavMeshAgent.enabled == true)
        {
            nextCheck = Time.time + checkRateValue;

            hitColliders = Physics.OverlapSphere(myTransform.position, detectionRadiusValue, playerDetectionLayer);
            {
                if (hitColliders.Length > 0)
                {
                    myNavMeshAgent.SetDestination(hitColliders[0].transform.position);
                }
            }
        }
    }

    void InvokeKill()
    {
        GameManager.MainGame.KillEnemy(this, shadowBlobPrefab);
    }

	public void UpdateEnemyText()
	{
		healthText.text = "(" + enemyNumber + ") " + HealthValue.ToString();
	}

    public bool ApplyDamage(int damageValue)
    {
		HealthValue -= damageValue;

        if (HealthValue <= 0)
        {
            healthText.text = string.Empty;
            eyes.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            return true;  //enemy dead
        }
        else
        {
            healthText.text = "(" + enemyNumber + ") " + HealthValue.ToString();
            return false;  //enemy still alive
        }
    }

    public void KillEnemy()
    {
        Invoke("InvokeKill", 3.0f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (inAir && other.transform.tag == "Floor")
        {
            myNavMeshAgent.enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            inAir = false;
        }
    }

	void FixedUpdate () 
    {
        CheckIfPlayerInRange();
	}

    public bool InAir
    {
        get { return inAir; }
        set { inAir = value; }
    }
}
