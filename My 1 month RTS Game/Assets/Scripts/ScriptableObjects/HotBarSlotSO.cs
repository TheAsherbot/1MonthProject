using Sirenix.OdinInspector;

using UnityEngine;

[CreateAssetMenu()]
public class HotBarSlotSO : ScriptableObject
{

    [PreviewField(alignment: ObjectFieldAlignment.Center)]
    public Sprite image;
    [Title("Tooltip"), HideLabel]
    [MultiLineProperty(lines: 10)]
    public string toolTip;


}
