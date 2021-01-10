using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectChanges : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToScene(string sceneName)
    {
        //Debug.Log("New Scene");
        //SceneManager.LoadScene("sceneName");
    }

    public void ToggleActive(GameObject g)
    {

        bool isactive = g.activeSelf;
        g.SetActive(!isactive);
        Debug.Log("Toggled!");
        Debug.Log(g.name);
    }

    public void Enlarge(GameObject g)
    {
        Debug.Log("Enlarging");
        Debug.Log(g.name);
        g.transform.localScale = new Vector3(2f, 2f, 2f);
    }

    public void NormalSize(GameObject g)
    {
        Debug.Log("Normal Size Again for");
        Debug.Log(g.name);
        g.transform.localScale = new Vector3(1f, 1f, 1f);
    }


}
