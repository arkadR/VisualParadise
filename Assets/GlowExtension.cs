using UnityEngine;

public static class GlowExtension
{
    private static readonly int emissionColor = Shader.PropertyToID("_EmissionColor");
    private static Color glowCollor = Color.yellow;
    private const string emission = "_EMISSION";

    public static void EnableGlow(this GameObject gameObject)
    {
        var material = LookupMaterial(gameObject);
        material.EnableKeyword(emission);
    }

    public static void DisableGlow(this GameObject gameObject)
    {
        var material = LookupMaterial(gameObject);
        material.DisableKeyword(emission);
    }

    public static void SetGlow(this GameObject gameObject, float glowStrength)
    {
        var physicalNodeMaterial = LookupMaterial(gameObject);
        physicalNodeMaterial.SetColor(emissionColor, glowCollor * glowStrength);
    }

    private static Material LookupMaterial(GameObject gameObject)
    {
        return gameObject.GetComponent<Renderer>().material;
    }
}