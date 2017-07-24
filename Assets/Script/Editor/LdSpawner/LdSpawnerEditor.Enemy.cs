using System;
using UnityEditor;
using UnityEngine;
using Orca.Contents.LevelDesign;

public partial class LdSpawnerEditor
{
    #region "OnGUI Enemy"

    private void OnGUIEnemy()
    {
        EditorGUILayout.BeginVertical("box");
        {
            EditorGUI.indentLevel++;
            GUI.color = FoldSubTitleColor;
            GUILayout.Label(" ＠[Wave Enemy Info]");
            GUI.color = OldColor;
            EditorGUI.indentLevel--;

            string title = m_showWaveEnemyList == true ? "▲ Hide Wave Enemy Info ▲" : "▼ Show Wave Enemy Info ▼";
            GUI.color = m_showWaveEnemyList == true ? Color.yellow : Color.green;
            if (GUILayout.Button(title))
            {
                m_showWaveEnemyList = !m_showWaveEnemyList;
            }
            GUI.color = OldColor;

            if (0 <= m_selectedWaveIndex)
            {
                if (m_showWaveEnemyList)
                {
                    SerializedProperty spawnWave = _spawnWaves.GetArrayElementAtIndex(m_selectedWaveIndex);

                    EditorGUILayout.BeginVertical("box");
                    {
                        var enemyList = spawnWave.FindPropertyRelative("EnemyList");
                        OnGUIEnemyList(enemyList);

                        OnGUIEnemyMenu(enemyList);

                        if (m_enemyIndex >= 0 && m_enemyIndex < enemyList.arraySize)
                        {
                            int explicitSpawnPointIdx = 0;
                            OnGUIEnemyDetail(enemyList.GetArrayElementAtIndex(m_enemyIndex), out explicitSpawnPointIdx);

                            if (explicitSpawnPointIdx == -1)
                            {
                                //OnGUIEnabledSpawnPoints(spawnWave);
                            }
                            EditorGUILayout.Space();
                        }

                        // @@@
                        //var triggerTargetList = spawnWave.FindPropertyRelative("triggerTargetList");
                        //TriggerEditor.DoInspectorTriggerTargets(triggerTargetList);

                        //GUI.color = Color.green;
                        //if (GUILayout.Button("Add Target"))
                        //{
                        //    OnAddTriggerTarget(triggerTargetList);
                        //}
                        //GUI.color = Color.white;
                    }
                    EditorGUILayout.EndVertical();                    
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void OnGUIEnemyList(SerializedProperty enemyList)
    {
        EditorGUILayout.BeginVertical();
        {
            string title = "-> Enemy List";
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUI.color = OldColor;

            EditorGUILayout.BeginVertical("box", GUILayout.Height(120));
            {
                m_enemyListScroll = EditorGUILayout.BeginScrollView(m_enemyListScroll);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("NPC", EditorStyles.boldLabel, GUILayout.Width(240));
                        GUILayout.Label("AI", EditorStyles.boldLabel);
                    }
                    EditorGUILayout.EndHorizontal();

                    if (enemyList.arraySize > 0)
                    {
                        for (int i = 0; i < enemyList.arraySize; ++i)
                        {
                            bool selected = i == m_enemyIndex;
                            var element = enemyList.GetArrayElementAtIndex(i);
                            var enemyCount = element.FindPropertyRelative("enemyCount");
                            var npcID = element.FindPropertyRelative("npcID");
                            var npcName = element.FindPropertyRelative("npcName");

                            string name = "";
                            EditorGUILayout.BeginHorizontal();
                            {
                                // @@@
                                //var monsterData = Table.ActorTableHelper.GetMonsterData(npcID.intValue);
                                //if (string.IsNullOrEmpty(npcName.stringValue) == false)
                                //{

                                //    if (monsterData != null)
                                //    {
                                //        name = monsterData.NameKR;
                                //        npcID.intValue = monsterData.ID;
                                //    }
                                //    else
                                //    {
                                //        name = "Select Look!";
                                //    }
                                //}
                                //else
                                //{
                                //    if (monsterData != null)
                                //    {
                                //        name = monsterData.NameKR;
                                //        npcName.stringValue = monsterData.NameKR;
                                //    }
                                //    else
                                //    {
                                //        name = "Select Look!";
                                //    }
                                //}

                                string label = string.Format("{0} : {1} ({2})", i, name, enemyCount.intValue);
                                EditorGUIUtil.Label(label, selected, 240, 2);

                                if (EditorGUIEventUtil.IsLastRectClicked())
                                {
                                    m_enemyIndex = i;
                                    RepaintEditor();
                                }

                                if (npcName.stringValue != string.Empty)
                                {
                                    EditorGUIUtil.Label(npcName.stringValue, selected, 0, 2);
                                }
                                else
                                {
                                    EditorGUIUtil.Label("Default", selected, 0, 2);
                                }

                                if (EditorGUIEventUtil.IsLastRectClicked())
                                {
                                    m_enemyIndex = i;
                                    RepaintEditor();
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnGUIEnemyMenu(SerializedProperty enemyList)
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUI.contentColor = Color.green;
            if (GUILayout.Button("Add Enemy"))
            {
                OnAddEnemy(enemyList);
            }
            GUI.contentColor = OldColor;

            GUI.enabled = m_enemyIndex > 0;
            if (GUILayout.Button("Up"))
            {
                OnMoveUpEnemy(enemyList, m_enemyIndex);
            }

            GUI.enabled = m_enemyIndex < enemyList.arraySize - 1;
            if (GUILayout.Button("Down"))
            {
                OnMoveDownEnemy(enemyList, m_enemyIndex);
            }
            GUI.enabled = true;

            GUI.color = Color.red;
            if (GUILayout.Button("X"))
            {
                OnDeleteEnemy(enemyList);
            }
            GUI.color = OldColor;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUIEnemyDetail(SerializedProperty enemy, out int explicitSpawnPointIdx)
    {
        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUI.contentColor = Color.cyan;
                EditorGUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Select NPCType"))
                    {
                        // @@@
                        //ShowNPCTypeList(enemy);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    if (GUILayout.Button("NPC Refresh"))
                    {
                        // @@@
                        //UpdateNPCTypes();
                    }
                }
                EditorGUILayout.EndVertical();
                GUI.contentColor = OldColor;
            }
            EditorGUILayout.EndHorizontal();

            var tooltip = new GUIContent("Weight(1 ~ 1000)", "스폰 개수");
            var enemyCount = enemy.FindPropertyRelative("enemyCount");
            EditorGUILayout.IntSlider(enemyCount, 1, 1000, tooltip);
            if (enemyCount.intValue < 1)
            {
                Debug.LogError("weight are forced more than one.");
                enemyCount.intValue = 1;
                RepaintEditor();
            }

            GUI.enabled = false;
            tooltip = new GUIContent("NPCID", "몬스터 아이디");
            var npcID = enemy.FindPropertyRelative("npcID");
            EditorGUILayout.PropertyField(npcID, tooltip);

            tooltip = new GUIContent("NPCName", "몬스터 이름");
            var npcName = enemy.FindPropertyRelative("npcName");
            EditorGUILayout.PropertyField(npcName, tooltip);
            GUI.enabled = true;

            tooltip = new GUIContent("SpawnFlag", "스폰 위치 설정 \n Exact : 설정된 위치에서 스폰 \n InRadius : SpawnRadius 범위내의 랜덤 포지션 \n AwayInRadius 영웅위치 주변으로..");
            var spawnFlag = enemy.FindPropertyRelative("spawnFlag");
            EditorGUILayout.PropertyField(spawnFlag, tooltip);

            var eSpawnFlag = (LdSpawnWave.Enemy.eSpawnFlag)Enum.Parse(typeof(LdSpawnWave.Enemy.eSpawnFlag), spawnFlag.enumNames[spawnFlag.intValue]);
            if (eSpawnFlag != LdSpawnWave.Enemy.eSpawnFlag.Exact)
            {
                EditorGUI.indentLevel++;
                tooltip = new GUIContent("-> SpawnRadius", "몬스터 스폰 범위 (반지름)");
                var spawnRadius = enemy.FindPropertyRelative("spawnRadius");
                EditorGUILayout.PropertyField(spawnRadius, tooltip);
                EditorGUI.indentLevel--;
            }

            tooltip = new GUIContent("SpawnState", "특수 Spawn State 설정(Animator 참조 < ex : Spawn2 벽타고 올라오는 오크 >)");
            var startState = enemy.FindPropertyRelative("startState");
            EditorGUILayout.PropertyField(startState, tooltip);

            EditorGUILayout.BeginHorizontal();
            {
                //var title = string.Format("Explicit SpawnPoint({0} ~ {1})", -1, _points.arraySize - 1);
                //tooltip = new GUIContent(title, "몬스터 소환 위치");
                //var explicitSpawnPoint = enemy.FindPropertyRelative("explicitSpawnPoint");
                //var tempPoint = explicitSpawnPoint.intValue;
                //EditorGUILayout.IntSlider(explicitSpawnPoint, -1, _points.arraySize - 1, tooltip);
                //if (explicitSpawnPoint.intValue < -1)
                //{
                //    Debug.LogError("SpawnPoint index is -1 or more");
                //    explicitSpawnPoint.intValue = 0;
                //    RepaintEditor();
                //}

                //explicitSpawnPointIdx = explicitSpawnPoint.intValue;
                explicitSpawnPointIdx = 0;

                //if (GUILayout.Button("Select"))
                //{
                //    //SelectSpawnPoint(enemy);
                //}
            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndVertical();
    }

    #endregion "OnGUI Enemy"

    private void OnAddEnemy(SerializedProperty enemyList)
    {
        var addEnemy = InsertArrayElementAtIndex(enemyList);

        LdSpawnWave.Enemy enemy = EditorHelper.GetTartgetObject<LdSpawnWave.Enemy>(addEnemy);
        enemy.InitializeByInsertArrayElement();

        m_enemyIndex = enemyList.arraySize - 1;
    }

    private void OnMoveUpEnemy(SerializedProperty enemyList, int enemyIndex)
    {
        MoveArrayElement(enemyList, enemyIndex, enemyIndex - 1);

        m_enemyIndex--;
    }

    private void OnMoveDownEnemy(SerializedProperty enemyList, int enemyIndex)
    {
        MoveArrayElement(enemyList, enemyIndex, enemyIndex + 1);

        m_enemyIndex++;
    }

    private void OnDeleteEnemy(SerializedProperty enemyList)
    {
        int removedEnemyIndex = m_enemyIndex;
        if (m_enemyIndex == enemyList.arraySize - 1)
        {
            m_enemyIndex--;
        }

        if (0 <= removedEnemyIndex)
        {
            DeleteArrayElementAtIndex(enemyList, removedEnemyIndex);
        }
    }
}
