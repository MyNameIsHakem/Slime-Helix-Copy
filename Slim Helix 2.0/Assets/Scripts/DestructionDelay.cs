using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionDelay : MonoBehaviour
{
    [Tooltip("How Much Time Before this object is destroyed in seconds")]
    public float TimeLeft;

    void Start()
    {
        StartCoroutine(Delay());
    }
   
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(TimeLeft);

        Destroy(gameObject);
    }
}
