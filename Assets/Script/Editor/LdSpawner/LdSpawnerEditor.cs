using System;
using UnityEngine;
using UnityEditor;
using Orca.Contents.LevelDesign;

[CustomEditor(typeof(LdSpawner))]
public partial class LdSpawnerEditor : Editor
{
    #region "Variables"

    private readonly Color OldColor = Color.white;
    private readonly Color FoldTitleColor = new Color(0, 1f, 0.8f, 1);
    private readonly Color FoldSubTitleColor = new Color(1f, 0.7f, 0.5f, 1);

    private bool m_showGUISettings = false;
    private bool m_showSpawnGroup = true;
    private bool m_showNpclist = false;
    private bool m_showWaveEnemyList = true;
    private bool m_showSpawnPoints = true;
    private bool m_showDebugSettings = false;

    private Vector2 m_spawnWaveScroll = Vector2.zero;
    private Vector2 m_enemyListScroll = Vector2.zero;

    private int m_enemyIndex = 0;
    private int m_selectedWaveIndex = -1;

    private eNextWaveCondition m_selectedCondition;    

    SerializedObject _serObj;

    SerializedProperty _spawnWaves;
    
    #endregion "Variables"

    [DrawGizmo(GizmoType.InSelectionHierarchy)]
    private static void OnDrawGizmosSelected(LdSpawner spawner, GizmoType gizmoType)
    {
        //Gizmos.DrawIcon(spawner.transform.position, "SpawnGroupIcon.tif", true);

        //Gizmos.color = Color.yellow;
        //int i = 0;

        //foreach (LdSpawner point in spawner.points)
        //{
        //    Vector3 spPos = point.GetEditorPosition(spawnGroup.transform);
        //    if (showConnectors)
        //        Gizmos.DrawLine(spawnGroup.transform.position, spPos);
        //    Gizmos.DrawIcon(spPos, "SpawnPointIcon.tif", true);

        //    if (showSpawnPtNumbers)
        //    {
        //        Camera cam = Camera.current;
        //        if (cam != null)
        //        {
        //            Vector3 vpPos = cam.WorldToViewportPoint(spPos);
        //            if (vpPos.z > 0.0f)
        //            {
        //                GUI.color = Color.green;
        //                Handles.Label(spPos + Vector3.up * 1.5f, string.Format("Point:{0}", i), EditorStyles.boldLabel);
        //                GUI.color = Color.white;
        //            }
        //        }
        //    }

        //    Vector3 spDir = point.GetEditorRotateDir();
        //    //SceneGUIUtil.DrawArrow(spPos, spPos + spDir * 1.0f);
        //    i++;
        //}
    }

    public void OnSceneGUI()
    {
        //SpawnGroup spawnGroup = target as SpawnGroup;
        //if (spawnGroup != null && spawnGroup.points != null)
        //{
        //    if (Tools.current == Tool.Move)
        //    {
        //        foreach (SpawnPoint point in spawnGroup.points)
        //        {
        //            Vector3 spPos = point.GetEditorPosition(spawnGroup.transform);
        //            Quaternion spRot = point.GetEditorRotate();
        //            Vector3 newPos = Handles.PositionHandle(spPos, spRot);
        //            if (newPos != spPos)
        //            {
        //                Undo.RecordObject(spawnGroup, "SpawnPoint move");
        //                point.SetPosition(newPos, spawnGroup.transform);
        //            }
        //        }
        //    }
        //}
    }

    private void OnEnable()
    {
        _serObj = new SerializedObject(target);

        _spawnWaves = _serObj.FindProperty("SpawnWaves");

        if (0 < _spawnWaves.arraySize)
            m_selectedWaveIndex = 0;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        _serObj.Update();

        EditorGUILayout.BeginVertical();
        {
            OnGUIWave();
            EditorGUILayout.Space();

            OnGUIEnemy();
            EditorGUILayout.Space();

            OnGUISpawnPoints();
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndVertical();

        _serObj.ApplyModifiedProperties();
    }

    private SerializedProperty InsertArrayElementAtIndex(SerializedProperty prop)
    {
        prop.InsertArrayElementAtIndex(prop.arraySize);
        _serObj.ApplyModifiedProperties();

        return prop.GetArrayElementAtIndex(prop.arraySize - 1);
    }

    private void MoveArrayElement(SerializedProperty prop, int srcIndex, int dstIndex)
    {
        prop.MoveArrayElement(srcIndex, dstIndex);
        _serObj.ApplyModifiedProperties();
    }

    private void DeleteArrayElementAtIndex(SerializedProperty prop, int index)
    {
        prop.DeleteArrayElementAtIndex(index);
        _serObj.ApplyModifiedProperties();
    }

    private void RepaintEditor()
    {
        GUI.SetNextControlName("");
        GUI.FocusControl("");
        Repaint();
    }
}
