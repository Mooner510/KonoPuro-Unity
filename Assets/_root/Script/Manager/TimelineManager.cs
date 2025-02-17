using System.Collections;
using System.Collections.Generic;
using _root.Script.Manager;
using _root.Script.UI;
using UnityEngine;
using UnityEngine.Playables;

public enum Scenestate
{
    Start,
    Title,
    Lobby,
    Obj1,
    Obj2,
    Obj3
}

public class TimelineManager : MonoBehaviour
{
    public readonly Stack<Scenestate> stateStack = new();
    private BackButton backBtn;
    private CamManager camManager;

    private Coroutine coroutine;
    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        backBtn = FindObjectOfType<BackButton>();
        camManager = FindObjectOfType<CamManager>();

        PlayTimeline(Scenestate.Start, Scenestate.Title);
    }

    public void PlayTimeline(Scenestate state1, Scenestate state2)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(PlayTimelineCoroutine(state1, state2));
    }

    private IEnumerator PlayTimelineCoroutine(Scenestate state1, Scenestate state2)
    {
        backBtn.OnOff(false);
        director.Stop();
        director.playableAsset = Resources.Load("Timeline/At" + state1 + "Timeline") as PlayableAsset;
        director.Play();
        yield return null;
        yield return new WaitUntil(() => director.time >= director.duration);
        director.Stop();
        director.playableAsset = Resources.Load("Timeline/To" + state2 + "Timeline") as PlayableAsset;
        director.Play();
        stateStack.Push(state2);
        camManager.SetCam(state2);
        if (state2 == Scenestate.Title)
        {
            backBtn.OnOff(false);
        }
        else
        {
            yield return null;
            yield return new WaitUntil(() => director.time >= director.duration);
            backBtn.OnOff(true);
        }
        // yield return null;
        // yield return new WaitUntil(() => director.time >= director.duration);
    }

    public void Back()
    {
        if (stateStack.Count == 0) return;
        PlayTimeline(stateStack.Pop(), stateStack.Pop());
    }

    public PlayState GetdirectorState()
    {
        return director.state;
    }
}