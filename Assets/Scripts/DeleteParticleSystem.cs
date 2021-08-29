using System.Collections;
using UnityEngine;


public class DeleteParticleSystem : MonoBehaviour
{
    public void Delete()
    {
        StartCoroutine(DeleteAfterPlay());
    }

    private IEnumerator DeleteAfterPlay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
