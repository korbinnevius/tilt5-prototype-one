using Timeline;
using UnityEngine;

public class TimelineRunner : MonoBehaviour
{
	public GameTimeline Timeline;

	private Coroutine timelineRoutine;
	//on enter gameplay state
	void Start()
	{
		timelineRoutine=StartCoroutine(Timeline.RunTimeline());
	}
}
