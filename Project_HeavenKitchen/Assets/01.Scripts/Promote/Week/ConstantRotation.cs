using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [Range(0, 5)]
    [SerializeField] private float rotateStrength = 1.5f;
    [SerializeField] Transform[] rotationTrms;
    private float rotate;

    [SerializeField] private float delay = 1f;

    private float initZRotation;

    private void Start()
    {
        initZRotation = rotationTrms[0].localRotation.z;
        rotate = rotateStrength;
    }

    private void OnEnable()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            rotate *= -1;

            for (int i = 0; i < rotationTrms.Length; i++)
            {
                rotationTrms[i].localRotation = Quaternion.Euler(new Vector3(0, 0, initZRotation + rotate));
            }
            yield return new WaitForSeconds(delay);
        }
    }
}
