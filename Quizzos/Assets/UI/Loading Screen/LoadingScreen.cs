using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneAsync());

    }

    private IEnumerator LoadSceneAsync()
    {
        Debug.Log("allo");

        AsyncOperation levelToLoad = SceneManager.LoadSceneAsync(1);
        while (!levelToLoad.isDone)
        {
            float progress = Mathf.Clamp01(levelToLoad.progress / 0.9f);
            slider.value = progress;
            Debug.Log(progress);
            yield return new WaitForEndOfFrame();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
