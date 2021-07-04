using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public void FadeToScene(int _index)
    {
        StartCoroutine(FadeToSceneCoRoutine(_index));
    }

    private IEnumerator FadeToSceneCoRoutine(int _index)
    {
        FaderOverlay.instance.FadeOut();
        yield return new WaitForSecondsRealtime(1f);
        var asyncLoad = SceneManager.LoadSceneAsync(_index);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        Time.timeScale = 1f;
    }
}
