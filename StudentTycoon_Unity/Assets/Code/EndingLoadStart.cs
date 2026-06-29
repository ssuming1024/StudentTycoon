using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndingLoadStart : MonoBehaviour
{
    private enum ScreenFitMode
    {
        StretchToFill,
        FitInside,
        FillScreenCrop
    }

    [Header("Required")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;

    [Header("Scene Flow")]
    [SerializeField] private string nextSceneName = "Start";
    [SerializeField] private bool loadNextSceneWhenFinished = true;
    [SerializeField] private float minimumWatchSeconds = 0f;

    [Header("Screen")]
    [SerializeField] private ScreenFitMode screenFitMode = ScreenFitMode.FillScreenCrop;
    [SerializeField] private int renderTextureWidth = 1920;
    [SerializeField] private int renderTextureHeight = 1080;

    private RenderTexture runtimeRenderTexture;
    private bool videoFinished;
    private bool sceneLoadStarted;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (rawImage == null)
        {
            rawImage = GetComponent<RawImage>();
        }
    }

    private void Start()
    {
        if (!ValidateReferences())
        {
            return;
        }

        ConfigureVideoPlayer();
        ApplyRawImageTexture();
        ApplyFullScreenLayout();

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.errorReceived += OnVideoError;

        videoPlayer.Prepare();
    }

    private void Update()
    {
        DetectVideoFinishedFallback();
    }

    private bool ValidateReferences()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("EndingLoadStart needs a VideoPlayer.");
            return false;
        }

        if (rawImage == null)
        {
            Debug.LogError("EndingLoadStart needs a RawImage.");
            return false;
        }

        if (videoPlayer.clip == null)
        {
            Debug.LogError("EndingLoadStart needs a VideoClip on the VideoPlayer.");
            return false;
        }

        return true;
    }

    private void ConfigureVideoPlayer()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
        videoPlayer.waitForFirstFrame = true;

        if (videoPlayer.targetTexture == null)
        {
            int width = Mathf.Max(16, renderTextureWidth);
            int height = Mathf.Max(16, renderTextureHeight);
            runtimeRenderTexture = new RenderTexture(width, height, 0)
            {
                name = "RuntimeEndingRenderTexture"
            };
            runtimeRenderTexture.Create();
            videoPlayer.targetTexture = runtimeRenderTexture;
        }
    }

    private void ApplyRawImageTexture()
    {
        rawImage.texture = videoPlayer.targetTexture;
        rawImage.color = Color.white;
        rawImage.raycastTarget = false;
    }

    private void ApplyFullScreenLayout()
    {
        RectTransform rectTransform = rawImage.rectTransform;
        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        if (screenFitMode == ScreenFitMode.StretchToFill)
        {
            StretchToParent(rectTransform);
            return;
        }

        Vector2 parentSize = GetParentSize(rectTransform);
        if (parentSize.x <= 0f || parentSize.y <= 0f)
        {
            StretchToParent(rectTransform);
            return;
        }

        float videoWidth = videoPlayer.width > 0 ? (float)videoPlayer.width : renderTextureWidth;
        float videoHeight = videoPlayer.height > 0 ? (float)videoPlayer.height : renderTextureHeight;
        if (videoWidth <= 0f || videoHeight <= 0f)
        {
            StretchToParent(rectTransform);
            return;
        }

        float parentAspect = parentSize.x / parentSize.y;
        float videoAspect = videoWidth / videoHeight;
        Vector2 targetSize = parentSize;

        if (screenFitMode == ScreenFitMode.FillScreenCrop)
        {
            if (videoAspect > parentAspect)
            {
                targetSize.x = parentSize.y * videoAspect;
            }
            else
            {
                targetSize.y = parentSize.x / videoAspect;
            }
        }
        else
        {
            if (videoAspect > parentAspect)
            {
                targetSize.y = parentSize.x / videoAspect;
            }
            else
            {
                targetSize.x = parentSize.y * videoAspect;
            }
        }

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = targetSize;
    }

    private void StretchToParent(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
    }

    private Vector2 GetParentSize(RectTransform rectTransform)
    {
        RectTransform parentRect = rectTransform.parent as RectTransform;
        if (parentRect != null && parentRect.rect.width > 0f && parentRect.rect.height > 0f)
        {
            return parentRect.rect.size;
        }

        return new Vector2(Screen.width, Screen.height);
    }

    private void OnPrepared(VideoPlayer vp)
    {
        videoFinished = false;
        ApplyRawImageTexture();
        ApplyFullScreenLayout();

        Debug.Log("Ending video ready. Length: " + videoPlayer.length + " seconds");
        videoPlayer.Play();
        StartCoroutine(LoadSceneAfterVideoFinished());
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        MarkVideoFinished("Ending video finished.");
    }

    private void DetectVideoFinishedFallback()
    {
        if (videoFinished || videoPlayer == null || !videoPlayer.isPrepared || videoPlayer.isLooping)
        {
            return;
        }

        ulong frameCount = videoPlayer.frameCount;
        if (frameCount > 0)
        {
            long lastFrame = frameCount > (ulong)long.MaxValue ? long.MaxValue : (long)frameCount - 1;
            if (videoPlayer.frame >= lastFrame)
            {
                MarkVideoFinished("Ending video finished by frame check.");
                return;
            }
        }

        if (videoPlayer.length > 0.01d && videoPlayer.time >= videoPlayer.length - 0.05d)
        {
            MarkVideoFinished("Ending video finished by time check.");
        }
    }

    private void MarkVideoFinished(string message)
    {
        if (videoFinished)
        {
            return;
        }

        Debug.Log(message);
        videoFinished = true;
    }

    private IEnumerator LoadSceneAfterVideoFinished()
    {
        float timer = 0f;

        while (!videoFinished || timer < minimumWatchSeconds)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (!loadNextSceneWhenFinished || sceneLoadStarted || string.IsNullOrWhiteSpace(nextSceneName))
        {
            yield break;
        }

        sceneLoadStarted = true;
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("VideoPlayer error: " + message);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnPrepared;
            videoPlayer.loopPointReached -= OnVideoFinished;
            videoPlayer.errorReceived -= OnVideoError;
        }

        if (runtimeRenderTexture != null)
        {
            runtimeRenderTexture.Release();
            Destroy(runtimeRenderTexture);
        }
    }
}
