using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    [Header("Objects")]
    public Transform cam;          // Cámara principal
    public Transform backGround;
    public Transform midleGround;
    public Transform foreGround;

    [Header("Parallax velocities")]
    [Range(0f, 1f)] public float backSpeed = 0.2f;
    [Range(0f, 1f)] public float midleSpeed = 0.5f;
    [Range(0f, 1f)] public float frontSpeed = 0.8f;

    private Vector3 lastCamPosition;

    void Start()
    {
        lastCamPosition = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPosition;

        backGround.position += new Vector3(delta.x * backSpeed, delta.y * backSpeed, 0);
        midleGround.position += new Vector3(delta.x * midleSpeed, delta.y * midleSpeed, 0);
        foreGround.position += new Vector3(delta.x * frontSpeed, delta.y * frontSpeed, 0);

        lastCamPosition = cam.position;
    }
}
