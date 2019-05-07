using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    private GameObject currentWeaponViewModel;
    public Transform hands;
    public Canvas playerHUD;
    private AudioSource playerAudioSource;
    public Transform playerViewCam;

    void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "Weapon")
    //    {
    //        PickUpWeapon(other.gameObject);
    //    }
    //}

    public Transform PlayerViewCamPosition
    {
        get { return playerViewCam; }
    }
}
