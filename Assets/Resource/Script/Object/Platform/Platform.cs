using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public AnimationCurve moveIntensityX;
    public AnimationCurve moveIntensityY;
    public AnimationCurve moveIntensityZ;
    Vector3 startPosition;
    public bool isMove;
    public float time;
    public float maxTime;
    private float timeRate = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            time = (time + timeRate * Time.deltaTime) % maxTime;

            Move();
        }
    }

    private void Move()
    {
        float intensityX = moveIntensityX.Evaluate(time);
        float intensityY = moveIntensityY.Evaluate(time);
        float intensityZ = moveIntensityZ.Evaluate(time);

        transform.localPosition = startPosition + new Vector3(intensityX, intensityY, intensityZ);
    }
}
