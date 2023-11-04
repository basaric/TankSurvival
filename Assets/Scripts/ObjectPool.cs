using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> objects;
    public GameObject type;
    public int amount;

    void Start() {
        objects = new List<GameObject>();
        GameObject newObject;
        for (int i = 0; i < amount; i++) {
            newObject = Instantiate(type);
            newObject.SetActive(false);
            objects.Add(newObject);
        }
    }
    public GameObject getObject() {
        for (int i = 0; i < amount; i++) {
            if (!objects[i].activeInHierarchy) {
                return objects[i];
            }
        }
        return null;
    }
}
