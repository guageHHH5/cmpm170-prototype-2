using UnityEngine;
using UnityEngine.UI;

public class TimeSlow : MonoBehaviour
{
    [SerializeField] private float slowTimeScale = 0.1f;
    [SerializeField] private float normalTimeScale = 1.0f;
    [SerializeField] private float transitionSpeed = 2.0f;
    [SerializeField] private Image timeSlowIcon;

    private bool isTimeSlowed = false;

    private void Start() => timeSlowIcon?.gameObject.SetActive(false);

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (!isTimeSlowed)
            {
                timeSlowIcon?.gameObject.SetActive(true);
                isTimeSlowed = true;
            }
            AdjustTimeScale(slowTimeScale);
        }
        else
        {
            if (isTimeSlowed)
            {
                timeSlowIcon?.gameObject.SetActive(false);
                isTimeSlowed = false;
            }
            AdjustTimeScale(normalTimeScale);
        }
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void AdjustTimeScale(float targetScale)
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);
    }
}
