using System;
using UnityEngine;
using UnityEditor;
using Orca.Contents.LevelDesign;

public partial class LdSpawnerEditor
{
    #region "OnGUI Wave"

    private void OnGUIWave()
    {
        EditorGUILayout.BeginHorizontal("box");
        {
            OnGUISpawnWaveList();

            OnGUISpawnWaveDetail();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUISpawnWaveList()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(200), GUILayout.Height(200));
        {
            m_spawnWaveScroll = EditorGUILayout.BeginScrollView(m_spawnWaveScroll);
            {
                OnGUIWaveList();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();

            OnGUIWaveMenu();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnGUIWaveList()
    {
        for (int index = 0; index < _spawnWaves.arraySize; ++index)
        {
            bool selected = m_selectedWaveIndex == index ? true : false;
            var element = _spawnWaves.GetArrayElementAtIndex(index);
            var waveName = string.Format("{0} : Wave", index);
            EditorGUIUtil.Label(waveName, selected, 0, 2);

            if (EditorGUIEventUtil.IsLastRectClicked())
            {
                m_selectedWaveIndex = index;
                m_enemyIndex = 0;

                var selectElement = _spawnWaves.GetArrayElementAtIndex(m_selectedWaveIndex);
                var waveCondition = selectElement.FindPropertyRelative("WaveCondition");
                var condition = waveCondition.FindPropertyRelative("NextWaveCondition");
                m_selectedCondition = (eNextWaveCondition)Enum.Parse(typeof(eNextWaveCondition), condition.enumNames[condition.intValue]);

                RepaintEditor();
            }
        }
    }

    private void OnGUIWaveMenu()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUI.color = Color.green;
            if (GUILayout.Button("Add Wave"))
            {
                DropDownMenu();
            }
            GUI.color = Color.white;

            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(64)))
            {
                OnDeleteSpawnWaveAtIndex();
            }
            GUI.color = Color.white;
        }
        EditorGUILayout.EndHorizontal();

        OnGUISpawnWaveOrder();
    }

    private void OnGUISpawnWaveOrder()
    {
        if (m_selectedWaveIndex <= -1)
            return;

        EditorGUILayout.BeginHorizontal();
        {
            GUI.enabled = m_selectedWaveIndex > 0;
            if (GUILayout.Button("Up"))
            {
                OnMoveUpSpawnWave(m_selectedWaveIndex);
            }

            GUI.enabled = m_selectedWaveIndex < _spawnWaves.arraySize - 1;

            if (GUILayout.Button("Down"))
            {
                OnMoveDownSpawnWave(m_selectedWaveIndex);
            }

            GUI.enabled = true;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUISpawnWaveDetail()
    {
        if (m_selectedWaveIndex == -1)
            return;

        EditorGUILayout.BeginVertical("box");
        {
            SerializedProperty spawnWaveProp = _spawnWaves.GetArrayElementAtIndex(m_selectedWaveIndex);

            if (null != spawnWaveProp)
            {
                SerializedProperty WaveConditionProp = spawnWaveProp.FindPropertyRelative("WaveCondition");

                GUI.enabled = false;
                var tooltip = new GUIContent("NextWave Condition", "다음 웨이브 조건 \n SpawnCount : n마리 소환후 다음 웨이브 \n DeathCount : n마리 죽인후 다음 웨이브 \n KillThemAll : 모든 몬스터 죽었을때 \n");
                var condition = WaveConditionProp.FindPropertyRelative("NextWaveCondition");
                var nextWaveCondition = (eNextWaveCondition)Enum.Parse(typeof(eNextWaveCondition), condition.enumNames[condition.intValue]);
                EditorGUILayout.TextField(tooltip, nextWaveCondition.ToString());
                GUI.enabled = true;

                tooltip = new GUIContent("Goal Count", "wave 완료 조건 개수 (0 == 즉시)");
                var conditionValue = WaveConditionProp.FindPropertyRelative("ConditionGoalCount");
                EditorGUILayout.PropertyField(conditionValue, tooltip);

                int enemyCount = 0;
                SerializedProperty enemyList = spawnWaveProp.FindPropertyRelative("EnemyList");
                foreach (SerializedProperty enemy in enemyList)
                {
                    enemyCount += enemy.FindPropertyRelative("enemyCount").intValue;
                }

                GUI.enabled = false;
                EditorGUILayout.IntField("Enemy Count", enemyCount);
                GUI.enabled = true;

                GUI.enabled = (1 < enemyCount);
                tooltip = new GUIContent("Spawn Delay(Time)", "몬스터 생성 지연시간 ( 하나의 wave 내에 Enemy Count가 1 이상일 경우.)");
                var spawnDelayTime = WaveConditionProp.FindPropertyRelative("SpawnDelayTime");
                EditorGUILayout.PropertyField(spawnDelayTime, tooltip);

                GUI.enabled = true;
            }
        }
        EditorGUILayout.EndVertical();
    }

    #endregion "OnGUI Wave"

    private void DropDownMenu()
    {
        GenericMenu menu = new GenericMenu();
        foreach (eNextWaveCondition item in Enum.GetValues(typeof(eNextWaveCondition)))
        {
            var name = item.ToString();
            menu.AddItem(new GUIContent(name), false, SelectCondition, new object[] { item });
        }
        menu.ShowAsContext();
    }

    private void SelectCondition(object arg)
    {
        object[] args = arg as object[];

        eNextWaveCondition condition = (eNextWaveCondition)args[0];

        if (null != _spawnWaves)
        {
            AddSpawnWave(_spawnWaves, condition);
        }
    }

    private void AddSpawnWave(SerializedProperty prop, eNextWaveCondition waveCondition)
    {
        var addSpawnWave = InsertArrayElementAtIndex(prop);

        LdSpawnWave spawnWave = EditorHelper.GetTartgetObject<LdSpawnWave>(addSpawnWave);
        switch (waveCondition)
        {
            case eNextWaveCondition.SpawnCount:
                spawnWave.WaveCondition = new LdWaveConditionSpawnCount();
                break;
            case eNextWaveCondition.DeathCount:
                spawnWave.WaveCondition = new LdWaveConditionDeathCount();
                break;
            case eNextWaveCondition.KillThemAll:
                spawnWave.WaveCondition = new LdWaveConditionKillThemAll();
                break;
        }

        m_enemyIndex = -1;
        _serObj.ApplyModifiedProperties();
    }

    private void OnMoveUpSpawnWave(int waveIndex)
    {
        MoveArrayElement(_spawnWaves, waveIndex, waveIndex - 1);
        m_selectedWaveIndex--;
    }

    private void OnMoveDownSpawnWave(int waveIndex)
    {
        MoveArrayElement(_spawnWaves, waveIndex, waveIndex + 1);
        m_selectedWaveIndex++;
    }

    private void OnDeleteSpawnWaveAtIndex()
    {
        if (m_selectedWaveIndex == _spawnWaves.arraySize - 1)
        {
            DeleteArrayElementAtIndex(_spawnWaves, m_selectedWaveIndex);
            m_selectedWaveIndex--;
        }
    }
}
