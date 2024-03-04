using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class FlickeringEmissive : MonoBehaviour
{
    [SerializeField]
    private bool flicker;
    [SerializeField]
    [Min(0f)]
    private float flickerSpeed = 1f;
    [SerializeField]
    private AnimationCurve brightnessCurve;
    [SerializeField]
    private AnimationCurve reverseBrightnessCurve;
    [SerializeField]
    private AnimationCurve pingpongBrightnessCurve;


    public bool isReverse;

    private new Renderer renderer;
    private List<Material> materials = new List<Material>();
    private List<Color> initialColors = new List<Color>();

    private const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORD = "_EMISSION";

    private bool isFirstTime = true;
    private float scaledTime = 0f;

    private float brightness = 0f;
    private float maxValueBrightness = 9f;
    private float minValueBrightness = 0f;

    public bool isPingPong;
    public bool isInstantaneous;


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        brightnessCurve.postWrapMode = WrapMode.Clamp;

        foreach(Material mat in renderer.materials)
        {
            if(renderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORD) && renderer.material.HasColor(EMISSIVE_COLOR_NAME))
            {
                materials.Add(mat);
                initialColors.Add(mat.GetColor(EMISSIVE_COLOR_NAME));
            }
            else
            {
                Debug.LogWarning($"{mat.name} is not configured to be emissive. " +
                    $"so FlickeringEmissive on {name} cannot animate this material!");
            }
        }

        if(materials.Count == 0)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (!isInstantaneous)
        {
            if (flicker && renderer.isVisible)  // renderer.isVisible si on souhaite que le flicker s'active seulement lorsqu'on le voit
            {
                if (isFirstTime)
                {
                    isFirstTime = false;
                    scaledTime = 0f;
                }
                else
                {
                    scaledTime += Time.deltaTime * flickerSpeed;
                }

                for (int i = 0; i < materials.Count; i++)
                {
                    Color color = initialColors[i];
                    if (!isPingPong)
                    {
                        if (!isReverse)
                            brightness = brightnessCurve.Evaluate(scaledTime);
                        else
                            brightness = reverseBrightnessCurve.Evaluate(scaledTime);
                    }
                    else if (isPingPong)
                    {
                        brightness = pingpongBrightnessCurve.Evaluate(scaledTime);
                    }
                    color = new Color(
                        color.r * Mathf.Pow(2, brightness),
                        color.g * Mathf.Pow(2, brightness),
                        color.b * Mathf.Pow(2, brightness),
                        color.a * Mathf.Pow(2, brightness)
                    );
                    materials[i].SetColor(EMISSIVE_COLOR_NAME, color);
                }

                if (brightness == maxValueBrightness && !isReverse) // lorsque le flicker atteint sa valeur max, il se désactive
                {
                    isReverse = true;
                    scaledTime = 0f;
                    enabled = false;
                }
                if (brightness == minValueBrightness && isReverse)
                {
                    isReverse = false;
                    isFirstTime = true;
                    enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < materials.Count; i++)
            {
                Color color = initialColors[i];
                if (!isReverse)
                    brightness = maxValueBrightness;
                else
                    brightness = minValueBrightness;
                color = new Color(
                        color.r * Mathf.Pow(2, brightness),
                        color.g * Mathf.Pow(2, brightness),
                        color.b * Mathf.Pow(2, brightness),
                        color.a * Mathf.Pow(2, brightness)
                    );
                materials[i].SetColor(EMISSIVE_COLOR_NAME, color);
            }
        }
    }
}
