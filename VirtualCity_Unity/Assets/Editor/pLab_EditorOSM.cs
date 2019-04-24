/******************************************************************************
 * File         : pLab_EditorOSM.cs            
 * Lisence      : BSD 3-Clause License
 * Copyright    : Lapland University of Applied Sciences
 * Authors      : Toni Westerlund (toni.westerlund@lapinamk.fi),
 *                Alexander Zeyer (Alexander.Zeyer@edu.lapinamk.fi),
 *                Laura Pfeiffer (Laura.Pfeiffer@edu.lapinamk.fi),
 *                Kristof Laszlo (Kristof.Laszlo@edu.lapinamk.fi),
 *                Jinuk Seong (Jinuk.Seong@edu.lapinamk.fi)
 * BSD 3-Clause License
 *
 * Copyright (c) 2019, Lapland University of Applied Sciences
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *  list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *  this list of conditions and the following disclaimer in the documentation
 *  and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its
 *  contributors may be used to endorse or promote products derived from
 *  this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using UnityEngine;
using UnityEditor;


/// <summary>
/// pLab_EditorOSM
/// </summary>
[CustomEditor(typeof(pLab_OSMReader))]
[CanEditMultipleObjects]
public class pLab_EditorOSM : Editor {


    #region // SerializeField
    #endregion

    #region // Private Attributes

    /// <summary>
    /// editorData
    /// </summary>
    private OSMEditorData editorData = null;
    #endregion

    #region // Public Attributes

    #endregion

    #region // Protected Attributes

    #endregion

    #region // Set/Get

    #endregion

    #region // Base Class Methods
    #endregion

    #region // Private Methods
    #endregion

    #region // Public Methods

    public override void OnInspectorGUI() {
        pLab_OSMReader osmReader = (pLab_OSMReader)target;

        if (null == editorData) {
            editorData = osmReader.Open();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Min Lat");
        string minLat = EditorGUILayout.TextField(editorData.minLat);
        if (minLat != editorData.minLat) {
            editorData.minLat = minLat;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Min Lon");
        string minLon = EditorGUILayout.TextField(editorData.minLon);
        if (minLon != editorData.minLon) {
            editorData.minLon = minLon;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Lat");
        string maxLat = EditorGUILayout.TextField(editorData.maxLat);
        if (maxLat != editorData.maxLat) {
            editorData.maxLat = maxLat;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Lon");
        string maxLon = EditorGUILayout.TextField(editorData.maxLon);
        if (maxLon != editorData.maxLon) {
            editorData.maxLon = maxLon;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Water Material");
        Material waterMaterial = (Material)EditorGUILayout.ObjectField("material", editorData.waterMaterial, typeof(Material));
        if (waterMaterial != editorData.waterMaterial) {
            editorData.waterMaterial = waterMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Roof Material");
        Material roofMaterial = (Material)EditorGUILayout.ObjectField("material", editorData.roofMaterial, typeof(Material));
        if (roofMaterial != editorData.roofMaterial) {
            editorData.roofMaterial = roofMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Road Material");
        Material roadMaterial = (Material)EditorGUILayout.ObjectField("material", editorData.roadMaterial, typeof(Material));

        if (roadMaterial != editorData.roadMaterial) {
            editorData.roadMaterial = roadMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wall Material");
        Material wallMaterial = (Material)EditorGUILayout.ObjectField("material", editorData.wallMaterial, typeof(Material));
        if (wallMaterial != editorData.wallMaterial) {
            editorData.wallMaterial = wallMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Green1 Material");
        Material green1Material = (Material)EditorGUILayout.ObjectField("material", editorData.green1Material, typeof(Material));
        if (green1Material != editorData.green1Material) {
            editorData.green1Material = green1Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Green2 Material");
        Material green2Material = (Material)EditorGUILayout.ObjectField("material", editorData.green2Material, typeof(Material));
        if (green2Material != editorData.green2Material) {
            editorData.green2Material = green2Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Green3 Material");
        Material green3Material = (Material)EditorGUILayout.ObjectField("material", editorData.green3Material, typeof(Material));
        if (green3Material != editorData.green3Material) {
            editorData.green3Material = green3Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brown Material");
        Material brownMaterial = (Material)EditorGUILayout.ObjectField("material", editorData.brownMaterial, typeof(Material));
        if (brownMaterial != editorData.brownMaterial) {
            editorData.brownMaterial = brownMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wetland Material");
        Material wetlandMaterial = (Material)EditorGUILayout.ObjectField("material", editorData.wetlandMaterial, typeof(Material));
        if (wetlandMaterial != editorData.wetlandMaterial) {
            editorData.wetlandMaterial = wetlandMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Generate MAP")) {


            osmReader.GetDataFromServer(double.Parse(editorData.minLat), double.Parse(editorData.minLon), double.Parse(editorData.maxLat), double.Parse(editorData.maxLon),
                editorData.waterMaterial, editorData.roofMaterial, editorData.wallMaterial, editorData.roadMaterial, editorData.green1Material, editorData.green2Material, editorData.green3Material, editorData.brownMaterial, editorData.wetlandMaterial);
        }

        if (GUILayout.Button("Load From file")) {

            string path = EditorUtility.OpenFilePanel("Select OSM file", "", "osm");
             osmReader.GenerateMapFromFile(path,
                editorData.waterMaterial, 
                editorData.roofMaterial, 
                editorData.wallMaterial, 
                editorData.roadMaterial,
                editorData.green1Material,
                editorData.green2Material, 
                editorData.green3Material, 
                editorData.brownMaterial,
                editorData.wetlandMaterial);
        }

    }
    #endregion
}
