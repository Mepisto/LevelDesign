using Orca.Contents.LevelDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(LdSpawner))]
public class LdSpawnerEditor : Editor
{
    SerializedObject _serObj;

    SerializedProperty _spawnCountWaves;

    SerializedProperty _deathCountWaves;

    SerializedProperty _killThemAllWaves;


    private void OnEnable()
    {
        _serObj = new SerializedObject(target);

        _spawnCountWaves = _serObj.FindProperty("SpawnCountWaves");

        _deathCountWaves = _serObj.FindProperty("DeathCountWaves");

        _killThemAllWaves = _serObj.FindProperty("KillThemAllWaves");


        m_dicSelectedWaveIndex.Add(eNextWaveCondition.SpawnCount, -1);
        m_dicSelectedWaveIndex.Add(eNextWaveCondition.DeathCount, -1);
        m_dicSelectedWaveIndex.Add(eNextWaveCondition.KillThemAll, -1);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        _serObj.Update();

        EditorGUILayout.BeginVertical();
        {
            OnGUIWave();
        }
        EditorGUILayout.EndVertical();

        _serObj.ApplyModifiedProperties();
    }

    private void OnGUIWave()
    {
        EditorGUILayout.BeginHorizontal("box");
        {
            OnGUISpawnWaveList();

            OnGUISpawnWaveDetail(m_selectedCondition);            
        }
        EditorGUILayout.EndHorizontal();
    }
   

    private int m_enemyIndex = 0;
    private Dictionary<eNextWaveCondition, int> m_dicSelectedWaveIndex = new Dictionary<eNextWaveCondition, int>();
    private int m_selectedWaveIndex = 0;
    private eNextWaveCondition m_selectedCondition;
    private Vector2 m_spawnWaveScroll = Vector2.zero;

    private void OnGUISpawnWaveList()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(200), GUILayout.Height(200));
        {
            m_spawnWaveScroll = EditorGUILayout.BeginScrollView(m_spawnWaveScroll);
            {
                int itor = 0;
                int index = 0;
                OnGUIWaveList(eNextWaveCondition.SpawnCount, ref index, ref itor);
                index = 0;
                OnGUIWaveList(eNextWaveCondition.DeathCount, ref index, ref itor);
                index = 0;
                OnGUIWaveList(eNextWaveCondition.KillThemAll, ref index, ref itor);
            }
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Add Wave"))
                {
                    OnGUIDropDownMenu();
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

            if (m_dicSelectedWaveIndex[m_selectedCondition] != -1)
            {
                OnGUISpawnWaveMenu(m_dicSelectedWaveIndex[m_selectedCondition]);
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void OnGUIDropDownMenu()
    {
        GenericMenu menu = new GenericMenu();
        foreach (eNextWaveCondition item in Enum.GetValues(typeof(eNextWaveCondition)))
        {
            var name = item.ToString();
            menu.AddItem(new GUIContent(name), false, SelectCondition, new object[] { item });
        }
        menu.ShowAsContext();
    }

    private void OnGUIWaveList(eNextWaveCondition waveCondition, ref int index, ref int itor)
    {
        var prop = GetCondition(waveCondition);

        for (index = 0; index < prop.arraySize; ++index)
        {
            bool selected = m_dicSelectedWaveIndex[waveCondition] == index ? true : false;
            var element = prop.GetArrayElementAtIndex(index);
            var waveName = string.Format("{0} : Wave", itor);
            EditorGUIUtil.Label(waveName, selected, 0, 2);

            if (EditorGUIEventUtil.IsLastRectClicked())
            {
                ResetSelectIndex();

                m_dicSelectedWaveIndex[waveCondition] = index;
                m_enemyIndex = 0;

                var selectElement = prop.GetArrayElementAtIndex(m_dicSelectedWaveIndex[waveCondition]);
                var condition = selectElement.FindPropertyRelative("NextWaveCondition");
                m_selectedCondition = (eNextWaveCondition)Enum.Parse(typeof(eNextWaveCondition), condition.enumNames[condition.intValue]);

                RepaintEditor();
            }

            itor++;
        }
    }

    private void ResetSelectIndex()
    {
        m_dicSelectedWaveIndex[eNextWaveCondition.SpawnCount] = -1;
        m_dicSelectedWaveIndex[eNextWaveCondition.DeathCount] = -1;
        m_dicSelectedWaveIndex[eNextWaveCondition.KillThemAll] = -1;
    }

    private void OnGUISpawnWaveMenu(int waveIndex)
    {
        if (waveIndex <= -1)
            return;

        EditorGUILayout.BeginHorizontal();
        {
            GUI.enabled = waveIndex > 0;
            if (GUILayout.Button("Up"))
            {
                //OnMoveUpSpawnWave(waveIndex);
            }

            //GUI.enabled = waveIndex < _waves.arraySize - 1;

            if (GUILayout.Button("Down"))
            {
                //OnMoveDownSpawnWave(waveIndex);
            }

            GUI.enabled = true;            
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUISpawnWaveDetail(eNextWaveCondition selectedCondition)
    {
        var waveIndex = m_dicSelectedWaveIndex[selectedCondition];
        if (waveIndex == -1)
            return;

        EditorGUILayout.BeginVertical("box");
        {
            SerializedProperty element = GetConditionElementAtIndex(selectedCondition, waveIndex);

            if (null != element)
            {
                var tooltip = new GUIContent("NextWave Condition", "다음 웨이브 조건 \n SpawnCount : n마리 소환후 다음 웨이브 \n DeathCount : n마리 죽인후 다음 웨이브 \n KillThemAll : 모든 몬스터 죽었을때 \n");
                var condition = element.FindPropertyRelative("NextWaveCondition");
                var nextWaveCondition = (eNextWaveCondition)Enum.Parse(typeof(eNextWaveCondition), condition.enumNames[condition.intValue]);
                GUI.enabled = false;
                EditorGUILayout.TextField(tooltip, nextWaveCondition.ToString());
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void SelectCondition(object arg)
    {
        object[] args = arg as object[];

        eNextWaveCondition condition = (eNextWaveCondition)args[0];


        var prop = GetCondition(condition);
        if (null != prop)
        {
            OnAddSpawnWave(prop, condition);
        }

        _serObj.ApplyModifiedProperties();
    }


    private SerializedProperty GetCondition(eNextWaveCondition condition)
    {
        switch (condition)
        {
            case eNextWaveCondition.SpawnCount: return _spawnCountWaves;                                
            case eNextWaveCondition.DeathCount: return _deathCountWaves;
            case eNextWaveCondition.KillThemAll: return _killThemAllWaves;                
        }
        return null;
    }

    private SerializedProperty GetConditionElementAtIndex(eNextWaveCondition condition, int index)
    {
        switch (condition)
        {
            case eNextWaveCondition.SpawnCount:
                {
                    if (index < _spawnCountWaves.arraySize)
                        return _spawnCountWaves.GetArrayElementAtIndex(index);
                    else
                        return null;
                }

            case eNextWaveCondition.DeathCount:
                {
                    if (index < _deathCountWaves.arraySize)
                        return _deathCountWaves.GetArrayElementAtIndex(index);
                    else
                        return null;
                }

            case eNextWaveCondition.KillThemAll:
                {
                    if (index < _killThemAllWaves.arraySize)
                        return _killThemAllWaves.GetArrayElementAtIndex(index);
                    else
                        return null;
                }
        }

        return null;
    }

    private void OnAddSpawnWave(SerializedProperty prop, eNextWaveCondition waveCondition)
    {
        var addSpawnWave = InsertArrayElementAtIndex(prop);
        
        LdSpawnWave spawnWave = null;
        switch (waveCondition)
        {
            case eNextWaveCondition.SpawnCount:
                spawnWave = EditorHelper.GetTartgetObject<LdConditionSpawnCount>(addSpawnWave);
                break;
            case eNextWaveCondition.DeathCount:
                spawnWave = EditorHelper.GetTartgetObject<LdConditionDeathCount>(addSpawnWave);
                break;
            case eNextWaveCondition.KillThemAll:
                spawnWave = EditorHelper.GetTartgetObject<LdConditionKillThemAll>(addSpawnWave);
                break;
        }
        
        spawnWave.InitializeByInsertArrayElement();
        
        //m_enemyIndex = -1;
    }

    private SerializedProperty InsertArrayElementAtIndex(SerializedProperty prop)
    {
        prop.InsertArrayElementAtIndex(prop.arraySize);
        _serObj.ApplyModifiedProperties();

        return prop.GetArrayElementAtIndex(prop.arraySize - 1);
    }

    private void OnDeleteSpawnWaveAtIndex()
    {
        var prop = GetCondition(m_selectedCondition);

        if (m_dicSelectedWaveIndex[m_selectedCondition] == prop.arraySize - 1)
        {
            DeleteArrayElementAtIndex(prop, m_dicSelectedWaveIndex[m_selectedCondition]);
            m_dicSelectedWaveIndex[m_selectedCondition]--;
        }
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
