using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public string sceneState;
    public PlayableDirector director;
    
    private void Awake()
    {
        var timeline = Resources.Load("Timeline/TitleTimeline");
        director = GetComponent<PlayableDirector>();
        director.Play((PlayableAsset)timeline);
        sceneState = "Title";
    }

    public void Change(string state)
    {
        var timeline = Resources.Load("Timeline/" + state + "Timeline");
        director.Play((PlayableAsset)timeline);
        sceneState = state;
    }
}
