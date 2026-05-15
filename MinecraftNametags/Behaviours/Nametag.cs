using TMPro;
using UnityEngine;

namespace MinecraftNametags.Behaviours;

// i tried to clean this (ecstatic's code) up don't even consider this my code at this point i am half awake doing this
public class Nametag : MonoBehaviour
{
    private bool Loaded;
    private VRRig? Rig;

    private GameObject NametagObject = null!;
    private Canvas Canvas = null!;
    private TextMeshProUGUI NameText = null!;

    private GameObject? SpeakerIcon;
    private GameObject? SignificanceIconObject;
    private GameObject? PaintbrawlHealthObject;

    public void Awake()
    {
        Rig = GetComponent<VRRig>();

        Debug.Log("hi nametags can we load thanks");

        NametagObject = Instantiate(
            (Plugin.Instance?.Bundle!).LoadAsset<GameObject>("Nametag")
        );

        OnLoadComplete();
    }

    public void OnLoadComplete()
    {
        if (!Rig)
            return;

        NametagObject.transform.SetParent(Rig.transform, false);

        Canvas = NametagObject.transform.GetChild(0).GetComponent<Canvas>();

        var outline = Canvas.transform.Find("Outline")?.gameObject;
        if (outline)
            outline.SetActive(false);

        NameText = Canvas.transform
            .Find("Nameplate")
            .GetComponent<TextMeshProUGUI>();

        SpeakerIcon = Canvas.transform.Find("Speaker")?.gameObject;
        SignificanceIconObject = Canvas.transform.Find("Icon")?.gameObject;

        PaintbrawlHealthObject = Canvas.transform.Find("Paintbrawl Health")?.gameObject;
        if (PaintbrawlHealthObject)
            PaintbrawlHealthObject.SetActive(false);

        if (SpeakerIcon)
            SpeakerIcon.SetActive(false);

        if (SignificanceIconObject)
            SignificanceIconObject.SetActive(false);
        
        Rig.OnNameChanged += _ => UpdateState();
        
        Loaded = true;

        UpdateState();
    }

    public void Update()
    {
        if (!Loaded || !Rig || !Canvas || !NameText)
            return;

        var cam = GorillaTagger.Instance.mainCamera.transform;

        Canvas.transform.eulerAngles = new Vector3(
            cam.eulerAngles.x,
            cam.eulerAngles.y,
            0f
        );
        
        NameText.text = Rig.playerText1 ? Rig.playerText1.text : string.Empty;
    }

    public void UpdateState()
    {
        if (!Loaded || !Rig)
            return;

        if (SpeakerIcon)
            SpeakerIcon.SetActive(false);

        if (SignificanceIconObject)
            SignificanceIconObject.SetActive(false);

        if (PaintbrawlHealthObject)
            PaintbrawlHealthObject.SetActive(false);
    }
}