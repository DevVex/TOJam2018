﻿using UnityEditor;
using UnityEngine;
using Crosstales.TrueRandom.EditorUtil;

namespace Crosstales.TrueRandom.EditorIntegration
{
    /// <summary>Editor window extension.</summary>
    [InitializeOnLoad]
    public class ConfigWindow : ConfigBase
    {

        #region Variables

        private int tab = 0;
        private int lastTab = 0;

        private Vector2 scrollPosPrefabs;
        private Vector2 scrollPosTD;

        private bool showIntegerGenerator;
        private bool showIntResults;
        private int intNumber = 1;
        private int intMin = 1;
        private int intMax = 10;

        private bool showFloatGenerator;
        private bool showFloatResults;
        private int floatNumber = 1;
        private float floatMin = 0;
        private float floatMax = 1;

        private bool showSequenceGenerator;
        private bool showSeqResults;
        private int seqtMin = 1;
        private int seqMax = 6;

        private bool showStringGenerator;
        private bool showStringResults;
        private int stringNumber = 1;
        private int stringLength = 6;
        private bool digits = true;
        private bool upper = true;
        private bool lower = true;
        private bool unique = false;

        private bool showVector2Generator;
        private bool showVector2Results;
        private int vector2Number = 1;
        private Vector2 vector2Min = Vector2.zero;
        private Vector2 vector2Max = Vector2.one;

        private bool showVector3Generator;
        private bool showVector3Results;
        private int vector3Number = 1;
        private Vector3 vector3Min = Vector3.zero;
        private Vector3 vector3Max = Vector3.one;

        private bool showVector4Generator;
        private bool showVector4Results;
        private int vector4Number = 1;
        private Vector4 vector4Min = Vector4.zero;
        private Vector4 vector4Max = Vector4.one;

        #endregion


        #region EditorWindow methods

        [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Configuration...", false, EditorHelper.MENU_ID + 1)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ConfigWindow));
        }

        public static void ShowWindow(int tab)
        {
            ConfigWindow window = EditorWindow.GetWindow(typeof(ConfigWindow)) as ConfigWindow;
            window.tab = tab;
        }

        public void OnEnable()
        {
            titleContent = new GUIContent(Util.Constants.ASSET_NAME, EditorHelper.Logo_Asset_Small);
        }

        public void OnGUI()
        {
            tab = GUILayout.Toolbar(tab, new string[] { "Config", "Prefabs", "TD", "Help", "About" });

            if (tab != lastTab)
            {
                lastTab = tab;
                GUI.FocusControl(null);
            }

            if (tab == 0)
            {
                showConfiguration();

                EditorHelper.SeparatorUI();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(new GUIContent(" Save", EditorHelper.Icon_Save, "Saves the configuration settings for this project")))
                    {
                        save();

                        GAApi.Event(typeof(ConfigWindow).Name, "Save configuration");
                    }

                    if (GUILayout.Button(new GUIContent(" Reset", EditorHelper.Icon_Reset, "Resets the configuration settings for this project.")))
                    {
                        if (EditorUtility.DisplayDialog("Reset configuration?", "Reset the configuration of " + Util.Constants.ASSET_NAME + "?", "Yes", "No"))
                        {
                            Util.Config.Reset();
                            EditorConfig.Reset();
                            save();

                            GAApi.Event(typeof(ConfigWindow).Name, "Reset configuration");
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);
            }
            else if (tab == 1)
            {
                showPrefabs();
            }
            else if (tab == 2)
            {
                showTestDrive();
            }
            else if (tab == 3)
            {
                showHelp();
            }
            else
            {
                showAbout();
            }
        }

        public void OnInspectorUpdate()
        {
            Repaint();
        }

        #endregion


        #region Private methods

        private void showPrefabs()
        {
            scrollPosPrefabs = EditorGUILayout.BeginScrollView(scrollPosPrefabs, false, false);
            {
                GUILayout.Label("Available Prefabs", EditorStyles.boldLabel);

                GUILayout.Space(6);

                if (!EditorHelper.isTrueRandomInScene)
                {
                    if (!EditorHelper.isTrueRandomInScene)
                    {

                        GUILayout.Label(Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME);

                        if (GUILayout.Button(new GUIContent(" Add", EditorHelper.Icon_Plus, "Adds a '" + Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME + "'-prefab to the scene.")))
                        {
                            EditorHelper.InstantiatePrefab(Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME);
                            GAApi.Event(typeof(ConfigWindow).Name, "Add " + Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("All available prefabs are already in the scene.", MessageType.Info);
                }

                GUILayout.Space(6);
            }
            EditorGUILayout.EndScrollView();
        }

        private void showTestDrive()
        {
            if (EditorHelper.isTrueRandomInScene)
            {
                if (Util.Helper.isEditorMode)
                {
                    scrollPosTD = EditorGUILayout.BeginScrollView(scrollPosTD, false, false);
                    {
                        GUILayout.Label("Test-Drive", EditorStyles.boldLabel);

                        if (Util.Config.SHOW_QUOTA)
                        {
                            GUILayout.Label("Quota: " + TRManager.CurrentQuota);

                            GUILayout.Space(8);

                            EditorHelper.SeparatorUI();
                        }
                        else
                        {
                            GUILayout.Space(6);
                        }

                        TRManager.isPRNG = EditorGUILayout.Toggle(new GUIContent("PRNG", "Enable or disable the C#-standard Pseudo-Random-Number-Generator-mode (default: false)."), TRManager.isPRNG);

                        EditorHelper.SeparatorUI();

                        //tab = GUILayout.Toolbar(tab, new string[] { "Integer", "Float", "Sequence", "String" });

                        showIntegerGenerator = EditorGUILayout.Foldout(showIntegerGenerator, "Generate random integers");

                        if (showIntegerGenerator)
                        {
                            EditorGUI.indentLevel++;

                            //GUILayout.Label ("Generate random integers", EditorStyles.boldLabel);
                            intMin = EditorGUILayout.IntField(new GUIContent("Min", "Smallest possible number (range: -1'000'000'000 - 1'000'000'000)"), intMin);
                            intMax = EditorGUILayout.IntField(new GUIContent("Max", "Biggest possible number (range: -1'000'000'000 - 1'000'000'000)"), intMax);
                            intNumber = EditorGUILayout.IntField(new GUIContent("Number", "How many numbers you want to generate (range: 1 - 10'000)"), intNumber);

                            GUI.enabled = !TRManager.isGenerating;
                            if (GUILayout.Button(new GUIContent(TRManager.isGenerating ? " Generating... Please wait." : " Generate", EditorHelper.Icon_Generate, "Generate random integers.")))
                            {
                                TRManager.GenerateIntegerInEditor(intMin, intMax, Mathf.Abs(intNumber));
                                GAApi.Event(typeof(ConfigWindow).Name, "Generate random integers");
                            }
                            GUI.enabled = true;

                            EditorGUI.indentLevel++;

                            showIntResults = EditorGUILayout.Foldout(showIntResults, "Results (" + TRManager.CurrentIntegers.Count + ")");
                            if (showIntResults)
                            {
                                EditorGUI.indentLevel++;

                                foreach (int res in TRManager.CurrentIntegers)
                                {
                                    EditorGUILayout.SelectableLabel(res.ToString(), GUILayout.Height(16), GUILayout.ExpandHeight(false));
                                }

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel -= 2;
                        }

                        EditorHelper.SeparatorUI();

                        showFloatGenerator = EditorGUILayout.Foldout(showFloatGenerator, "Generate random floats");
                        if (showFloatGenerator)
                        {
                            EditorGUI.indentLevel++;
                            //GUILayout.Label ("Generate random floats", EditorStyles.boldLabel);
                            floatMin = EditorGUILayout.FloatField(new GUIContent("Min", "Smallest possible number (range: -1'000'000'000 - 1'000'000'000)"), floatMin);
                            floatMax = EditorGUILayout.FloatField(new GUIContent("Max", "Biggest possible number (range: -1'000'000'000 - 1'000'000'000)"), floatMax);
                            floatNumber = EditorGUILayout.IntField(new GUIContent("Number", "How many numbers you want to generate (range: 1 - 10'000)"), floatNumber);

                            GUI.enabled = !TRManager.isGenerating;
                            if (GUILayout.Button(new GUIContent(TRManager.isGenerating ? " Generating... Please wait." : " Generate", EditorHelper.Icon_Generate, "Generate random floats.")))
                            {
                                TRManager.GenerateFloatInEditor(floatMin, floatMax, Mathf.Abs(floatNumber));
                                GAApi.Event(typeof(ConfigWindow).Name, "Generate random floats");
                            }
                            GUI.enabled = true;

                            EditorGUI.indentLevel++;

                            showFloatResults = EditorGUILayout.Foldout(showFloatResults, "Results (" + TRManager.CurrentFloats.Count + ")");
                            if (showFloatResults)
                            {
                                EditorGUI.indentLevel++;

                                foreach (float res in TRManager.CurrentFloats)
                                {
                                    EditorGUILayout.SelectableLabel(res.ToString(), GUILayout.Height(16), GUILayout.ExpandHeight(false));
                                }

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel -= 2;
                        }

                        EditorHelper.SeparatorUI();

                        showSequenceGenerator = EditorGUILayout.Foldout(showSequenceGenerator, "Generate random sequence");
                        if (showSequenceGenerator)
                        {
                            EditorGUI.indentLevel++;
                            //GUILayout.Label ("Generate random sequence", EditorStyles.boldLabel);
                            seqtMin = EditorGUILayout.IntField(new GUIContent("Min", "Start of the interval (range: -1'000'000'000 - 1'000'000'000)"), seqtMin);
                            seqMax = EditorGUILayout.IntField(new GUIContent("Max", "End of the interval (range: -1'000'000'000 - 1'000'000'000)"), seqMax);

                            GUI.enabled = !TRManager.isGenerating;
                            if (GUILayout.Button(new GUIContent(TRManager.isGenerating ? " Generating... Please wait." : " Generate", EditorHelper.Icon_Generate, "Generate random sequence.")))
                            {
                                TRManager.GenerateSequenceInEditor(seqtMin, seqMax);
                                GAApi.Event(typeof(ConfigWindow).Name, "Generate random sequence");
                            }
                            GUI.enabled = true;

                            EditorGUI.indentLevel++;

                            showSeqResults = EditorGUILayout.Foldout(showSeqResults, "Results (" + TRManager.CurrentSequence.Count + ")");
                            if (showSeqResults)
                            {
                                EditorGUI.indentLevel++;

                                foreach (int res in TRManager.CurrentSequence)
                                {
                                    EditorGUILayout.SelectableLabel(res.ToString(), GUILayout.Height(16), GUILayout.ExpandHeight(false));
                                }

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel -= 2;
                        }

                        EditorHelper.SeparatorUI();

                        showStringGenerator = EditorGUILayout.Foldout(showStringGenerator, "Generate random strings");
                        if (showStringGenerator)
                        {
                            EditorGUI.indentLevel++;
                            //GUILayout.Label ("Generate random strings", EditorStyles.boldLabel);
                            stringLength = EditorGUILayout.IntField(new GUIContent("Length", "How long the strings should be (range: 1 - 20)"), stringLength);
                            stringNumber = EditorGUILayout.IntField(new GUIContent("Number", "How many strings you want to generate (range: 1 - 10'000)"), stringNumber);
                            digits = EditorGUILayout.Toggle(new GUIContent("Digits", "Allow digits (0-9)"), digits);
                            upper = EditorGUILayout.Toggle(new GUIContent("Uppercase letters", "Allow uppercase (A-Z) letters"), upper);
                            lower = EditorGUILayout.Toggle(new GUIContent("Lowercase letters", "Allow lowercase (a-z) letters"), lower);
                            unique = EditorGUILayout.Toggle(new GUIContent("Unique", "String shoud be unique in the result"), unique);

                            GUI.enabled = !TRManager.isGenerating;
                            if (GUILayout.Button(new GUIContent(TRManager.isGenerating ? " Generating... Please wait." : " Generate", EditorHelper.Icon_Generate, "Generate random strings.")))
                            {
                                TRManager.GenerateStringInEditor(stringLength, stringNumber, digits, upper, lower, unique);
                                GAApi.Event(typeof(ConfigWindow).Name, "Generate random strings");
                            }
                            GUI.enabled = true;

                            EditorGUI.indentLevel++;

                            showStringResults = EditorGUILayout.Foldout(showStringResults, "Results (" + TRManager.CurrentStrings.Count + ")");
                            if (showStringResults)
                            {
                                EditorGUI.indentLevel++;

                                foreach (string res in TRManager.CurrentStrings)
                                {
                                    EditorGUILayout.SelectableLabel(res, GUILayout.Height(16), GUILayout.ExpandHeight(false));
                                }

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel -= 2;
                        }


                        EditorHelper.SeparatorUI();

                        showVector2Generator = EditorGUILayout.Foldout(showVector2Generator, "Generate random Vector2");
                        if (showVector2Generator)
                        {
                            EditorGUI.indentLevel++;
                            vector2Min = EditorGUILayout.Vector2Field(new GUIContent("Min", "Smallest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)"), vector2Min);
                            vector2Max = EditorGUILayout.Vector2Field(new GUIContent("Max", "Biggest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)"), vector2Max);
                            vector2Number = EditorGUILayout.IntField(new GUIContent("Number", "How many Vector2 you want to generate (range: 1 - 10'000)"), vector2Number);

                            GUI.enabled = !TRManager.isGenerating;
                            if (GUILayout.Button(new GUIContent(TRManager.isGenerating ? " Generating... Please wait." : " Generate", EditorHelper.Icon_Generate, "Generate random Vector2.")))
                            {
                                TRManager.GenerateVector2InEditor(vector2Min, vector2Max, Mathf.Abs(vector2Number));
                                GAApi.Event(typeof(ConfigWindow).Name, "Generate random Vector2");
                            }
                            GUI.enabled = true;

                            EditorGUI.indentLevel++;

                            showVector2Results = EditorGUILayout.Foldout(showVector2Results, "Results (" + TRManager.CurrentVector2.Count + ")");
                            if (showVector2Results)
                            {
                                EditorGUI.indentLevel++;

                                foreach (Vector2 res in TRManager.CurrentVector2)
                                {
                                    EditorGUILayout.SelectableLabel(res.x + ", " + res.y, GUILayout.Height(16), GUILayout.ExpandHeight(false));
                                }

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel -= 2;
                        }

                        EditorHelper.SeparatorUI();

                        showVector3Generator = EditorGUILayout.Foldout(showVector3Generator, "Generate random Vector3");
                        if (showVector3Generator)
                        {
                            EditorGUI.indentLevel++;
                            vector3Min = EditorGUILayout.Vector3Field(new GUIContent("Min", "Smallest possible Vector3 (range: -1'000'000'000 - 1'000'000'000)"), vector3Min);
                            vector3Max = EditorGUILayout.Vector3Field(new GUIContent("Max", "Biggest possible Vector3 (range: -1'000'000'000 - 1'000'000'000)"), vector3Max);
                            vector3Number = EditorGUILayout.IntField(new GUIContent("Number", "How many Vector3 you want to generate (range: 1 - 10'000)"), vector3Number);

                            GUI.enabled = !TRManager.isGenerating;
                            if (GUILayout.Button(new GUIContent(TRManager.isGenerating ? " Generating... Please wait." : " Generate", EditorHelper.Icon_Generate, "Generate random Vector3.")))
                            {
                                TRManager.GenerateVector3InEditor(vector3Min, vector3Max, Mathf.Abs(vector3Number));
                                GAApi.Event(typeof(ConfigWindow).Name, "Generate random Vector3");
                            }
                            GUI.enabled = true;

                            EditorGUI.indentLevel++;

                            showVector3Results = EditorGUILayout.Foldout(showVector3Results, "Results (" + TRManager.CurrentVector3.Count + ")");
                            if (showVector3Results)
                            {
                                EditorGUI.indentLevel++;

                                foreach (Vector3 res in TRManager.CurrentVector3)
                                {
                                    EditorGUILayout.SelectableLabel(res.x + ", " + res.y + ", " + res.z, GUILayout.Height(16), GUILayout.ExpandHeight(false));
                                }

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel -= 2;
                        }

                        EditorHelper.SeparatorUI();

                        showVector4Generator = EditorGUILayout.Foldout(showVector4Generator, "Generate random Vector4");
                        if (showVector4Generator)
                        {
                            EditorGUI.indentLevel++;
                            vector4Min = EditorGUILayout.Vector4Field("Min", vector4Min);
                            vector4Max = EditorGUILayout.Vector4Field("Max", vector4Max);
                            vector4Number = EditorGUILayout.IntField(new GUIContent("Number", "How many Vector4 you want to generate (range: 1 - 10'000)"), vector4Number);

                            GUI.enabled = !TRManager.isGenerating;
                            if (GUILayout.Button(new GUIContent(TRManager.isGenerating ? " Generating... Please wait." : " Generate", EditorHelper.Icon_Generate, "Generate random Vector4.")))
                            {
                                TRManager.GenerateVector4InEditor(vector4Min, vector4Max, Mathf.Abs(vector4Number));
                                GAApi.Event(typeof(ConfigWindow).Name, "Generate random Vector4");
                            }
                            GUI.enabled = true;

                            EditorGUI.indentLevel++;

                            showVector4Results = EditorGUILayout.Foldout(showVector4Results, "Results (" + TRManager.CurrentVector4.Count + ")");
                            if (showVector4Results)
                            {
                                EditorGUI.indentLevel++;

                                foreach (Vector4 res in TRManager.CurrentVector4)
                                {
                                    EditorGUILayout.SelectableLabel(res.x + ", " + res.y + ", " + res.z + ", " + res.w, GUILayout.Height(16), GUILayout.ExpandHeight(false));
                                }

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel -= 2;
                        }
                    }
                    EditorGUILayout.EndScrollView();

                }
                else
                {
                    EditorGUILayout.HelpBox("Disabled in Play-mode!", MessageType.Info);
                }
            }
            else
            {
                EditorHelper.TRUnavailable();
            }
        }

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)