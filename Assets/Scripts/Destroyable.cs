using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int spawnCountMin = 5;
    public int spawnCountMax = 15;
    public GameObject spawnClass;
    public float horizontalLaunchStrength = 20f;
    public float verticalLaunchStrength = 10f;
    public float autoDestroyTime = 10f;
    public void destroy() {
        for (int i = 0; i < Random.Range(spawnCountMin, spawnCountMax); i++) {
            GameObject s = Instantiate(spawnClass, transform.position, Quaternion.identity);
            s.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-horizontalLaunchStrength, horizontalLaunchStrength), verticalLaunchStrength, Random.Range(-horizontalLaunchStrength, horizontalLaunchStrength));
            Destroy(s, autoDestroyTime);
        }
        Destroy(gameObject);
    }
}
