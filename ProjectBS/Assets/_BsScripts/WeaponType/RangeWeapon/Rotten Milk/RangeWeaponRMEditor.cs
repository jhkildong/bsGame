using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RangeWeaponRM))]
public class RangeWeaponRMEditor : Editor
{
    private void OnSceneGUI()
    {
        RangeWeaponRM rangeWeapon = (RangeWeaponRM)target;
        Handles.color = rangeWeapon.GizmosColor;
        Handles.DrawWireDisc(rangeWeapon.transform.position, Vector3.up, rangeWeapon.AtRange);
    }
}
