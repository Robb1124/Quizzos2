using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorHelper : MonoBehaviour
{
    [SerializeField] List<GameObject> listGameObjectToDesactivate = new List<GameObject>();
    [SerializeField] List<GameObject> listGameObjectToActivate = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetDesactivationList()
    {
        return listGameObjectToDesactivate;
    }

    public List<GameObject> GetActivationList()
    {
        return listGameObjectToActivate;
    }

}
