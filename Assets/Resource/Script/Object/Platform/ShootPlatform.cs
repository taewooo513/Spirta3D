using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlatform : InteractionObject
{
    public PlayerController player;
    public GameObject shootPoint;
    public float power;
    public float minX;
    public float maxX;
    public float rotX;
    public float rotZ;
    public float moveSpeed;
    bool isOnShootPlatform;
    public Camera camera;
    void Start()
    {
        camera.gameObject.SetActive(false);
        isOnShootPlatform = false;
        player = CharacterManager.Instance.player.controller;
    }

    void Update()
    {
        if (isOnShootPlatform)
        {
            RotateControll();
            ShootPlayer();
        }
    }
    override public void InteractionAction()
    {
        player.OnShootPlatform();
        isOnShootPlatform = true;
        camera.gameObject.SetActive(true);
    }

    private void RotateControll()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rotX -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rotX += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rotZ -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotZ += moveSpeed * Time.deltaTime;
        }
        rotX = Mathf.Clamp(rotX, minX, maxX);
        transform.localEulerAngles = new Vector3(rotX, rotZ, 0);
    }

    public void ShootPlayer()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 dir = shootPoint.transform.position - transform.position;
            dir = dir.normalized;
            Debug.Log(dir);
            player.transform.position = shootPoint.transform.position;
            player.OnShootPlayer(power, dir);
            isOnShootPlatform = false;
            camera.gameObject.SetActive(false);
        }
    }
}
