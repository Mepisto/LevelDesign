using System;
using UnityEngine;
using UnityEditor;
using Orca.Contents.LevelDesign;

public partial class LdSpawnerEditor
{
    #region "OnGUI Point"

    private void OnGUISpawnPoints()
    {
        EditorGUILayout.BeginVertical("box");
        {
            EditorGUI.indentLevel++;
            GUI.color = FoldSubTitleColor;
            GUILayout.Label(" ＠[Spawn Points]");
            GUI.color = OldColor;
            EditorGUI.indentLevel--;

            var title = m_showSpawnPoints == true ? "▲ Hide Spawn Points ▲" : "▼ Show Spawn Points ▼";
            GUI.color = m_showSpawnPoints == true ? Color.yellow : Color.green;
            if (GUILayout.Button(title))
            {
                m_showSpawnPoints = !m_showSpawnPoints;
            }
            EditorGUILayout.Space();
            GUI.color = OldColor;
            if (0 <= m_selectedWaveIndex)
            {
                if (m_showSpawnPoints)
                {
                    SerializedProperty spawnWave = _spawnWaves.GetArrayElementAtIndex(m_selectedWaveIndex);

                    var spawnPoints = spawnWave.FindPropertyRelative("SpawnPoints");
                    for (int i = 0; i < spawnPoints.arraySize; i++)
                    {
                        SerializedProperty point = spawnPoints.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginVertical("box");
                        {
                            GUI.color = Color.yellow;
                            string index = string.Format("Index : {0}", i);
                            GUILayout.Label(index);
                            GUI.color = OldColor;

                            var pos = point.FindPropertyRelative("Position");
                            //LdSpawnPoint spawnPoint = EditorHelper.GetTartgetObject<LdSpawnPoint>(point);
                            //Vector3 position = spawnPoint.GetEditorPosition(m_targetSpawnGroup.transform);
                            EditorGUILayout.BeginHorizontal();
                            {
                                //position = EditorGUILayout.Vector3Field("Pos", position);
                                EditorGUILayout.PropertyField(pos);

                                GUI.color = Color.cyan;
                                if (GUILayout.Button("LookAt", GUILayout.Width(55)))
                                {
                                    SceneView sv = SceneView.lastActiveSceneView;
                                    sv.LookAt(pos.vector3Value, Quaternion.Euler(45f, 0f, 0f));
                                }
                                GUI.color = OldColor;
                            }
                            EditorGUILayout.EndHorizontal();

                            SerializedProperty localRadian = point.FindPropertyRelative("LocalRadian");
                            EditorGUILayout.IntSlider(localRadian, 0, 360);
                            //float rotate = EditorGUILayout.IntSlider(localRadian, (int)OMath.RadianToDegree(localRadian.floatValue), 0, 360);
                            //spawnPoint.SetPosition(position, m_targetSpawnGroup.transform);
                            //spawnPoint.SetRotate(rotate);

                            //var tooltip = new GUIContent("CopyTransform", "Transform 정보복사");
                            //var copyTransform = point.FindPropertyRelative("CopyTransform");
                            //copyTransform.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField(tooltip, copyTransform.objectReferenceValue, typeof(GameObject), true);
                            //if (copyTransform.objectReferenceValue != null)
                            //{
                            //    spawnPoint.SetCopyTransform(m_targetSpawnGroup.transform, (copyTransform.objectReferenceValue as GameObject).transform);
                            //}

                            SerializedProperty spawnFlags = point.FindPropertyRelative("spawnFlags");
                            var sb = new System.Text.StringBuilder();
                            sb.Append(" - [Is Disabled : disable] \n");
                            sb.Append("- [Is Boss Point : 보스일 경우] \n");
                            sb.Append("- [Ignore Hero On Spawn : 자동으로 영웅을 보지않고 설정된 방향으로 스폰] \n");
                            sb.Append("- [Is Right Aligned : 스폰 할때 기본 몬스터의 방향] \n");
                            sb.Append("- [Local Space : 로컬 좌표]");
                            var tooltip = new GUIContent("Spawn Options ( mul )", sb.ToString());

                            spawnFlags.intValue = (int)((LdSpawnPoint.eSpawnFlags)EditorGUILayout.EnumMaskField(tooltip, (LdSpawnPoint.eSpawnFlags)spawnFlags.intValue));

                            EditorGUILayout.BeginHorizontal();
                            {
                                GUI.color = Color.red;
                                if (GUILayout.Button("Remove"))
                                {
                                    OnDeleteSpawnPoint(spawnPoints, i);
                                }
                                GUI.color = OldColor;

                                GUI.color = Color.cyan;
                                if (GUILayout.Button("Drop to Navigation"))
                                {
                                    //bool isHit = false;

                                    //Vector3 pos = spawnPoint.GetPosition(m_targetSpawnGroup);
                                    //UnityEngine.AI.NavMeshHit navhit;
                                    //if (UnityEngine.AI.NavMesh.SamplePosition(pos, out navhit, 999.0f, UnityEngine.AI.NavMesh.AllAreas))
                                    //{
                                    //    Undo.RecordObject(m_targetSpawnGroup, "SpawnPoint move");
                                    //    spawnPoint.SetPosition(navhit.position, m_targetSpawnGroup.transform);
                                    //    isHit = true;
                                    //}

                                    //if (!isHit)
                                    //{
                                    //    RaycastHit hit;
                                    //    Vector3 origin = spawnPoint.GetEditorPosition(m_targetSpawnGroup.transform);
                                    //    Ray r = new Ray(origin, Vector3.down);
                                    //    if (Physics.Raycast(r, out hit))
                                    //    {
                                    //        Undo.RecordObject(m_targetSpawnGroup, "SpawnPoint move");
                                    //        spawnPoint.SetPosition(hit.point, m_targetSpawnGroup.transform);
                                    //    }
                                    //}
                                }
                                GUI.color = OldColor;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                    }

                    GUI.color = Color.green;
                    if (GUILayout.Button("Add Child Spawn Point"))
                    {
                        OnAddSpawnPoint(spawnPoints);
                    }
                    GUI.color = OldColor;
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    #endregion "OnGUI Point"

    private void OnAddSpawnPoint(SerializedProperty spawnPoints)
    {
        var addPoint = InsertArrayElementAtIndex(spawnPoints);

        LdSpawnPoint spawnPoint = EditorHelper.GetTartgetObject<LdSpawnPoint>(addPoint);
        spawnPoint.InitializeByInsertArrayElement();
        //spawnPoint.SetPosition(m_targetSpawnGroup.transform.position + new Vector3(1, 0, 1), m_targetSpawnGroup.transform);
    }

    private void OnDeleteSpawnPoint(SerializedProperty spawnPoints, int spawnPointIndex)
    {
        if (spawnPoints.arraySize == 1)
        {
            if (EditorUtility.DisplayDialog("Warning", "SpawnPoints are forced more than one.", "OK"))
            {
                return;
            }
        }

        DeleteArrayElementAtIndex(spawnPoints, spawnPointIndex);
    }
}
