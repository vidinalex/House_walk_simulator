using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class CharacterBrain : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] clothed;
    [SerializeField]
    private GameObject[] nacked;
    public GameObject cam;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed = 20f;
    private CameraController cameraController;
    [SyncVar]
    public int clothId = -1;
    [SyncVar]
    public bool needToUpdate = false;
    private bool prevUpdate = false;

    private void CreateLocalCamera()
    {
        GameObject temp = Instantiate(cam);
        cameraController = temp.GetComponent<CameraController>();
        temp.GetComponent<CameraFollower>().target = gameObject;
    }

    private void CreateLocalCanvas()
    {
        Instantiate(canvas).GetComponent<ClothChanger>().SetOwner(gameObject);
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            CreateLocalCamera();
            CreateLocalCanvas();
        }    
    }

    private void Move(Vector3 movement)
    {
        transform.Translate(movement * speed * Time.deltaTime);
        if (!movement.Equals(Vector3.zero))
        {
            transform.rotation = Quaternion.Euler(0, cameraController.ApplyRotationToBody(), 0);
        }
    }

    private void UpdateAnimator(Vector3 movement)
    {
        animator.SetFloat("Fwd", movement.z);
        animator.SetFloat("Side", movement.x);
    }

    private void ChangeCloth()
    {
        nacked[clothId].SetActive(!nacked[clothId].activeInHierarchy);
        clothed[clothId].SetActive(!clothed[clothId].activeInHierarchy);
        prevUpdate = needToUpdate;
    }

    private void Update()
    {
        if (needToUpdate!=prevUpdate)
        {
            ChangeCloth();
        }
        if (!isLocalPlayer)
        {
            return;
        }

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));

        Move(movement);

        UpdateAnimator(movement);
    }

    [Command]
    private void ServerCloth(GameObject pl, int id, bool update)
    {
        pl.GetComponent<CharacterBrain>().clothId = id;
        pl.GetComponent<CharacterBrain>().needToUpdate = update;
    }

    public void CallCloth(GameObject pl,int id, bool update)
    {
        ServerCloth(pl,id,update);
        clothId = id;
        needToUpdate = update;
    }
}
