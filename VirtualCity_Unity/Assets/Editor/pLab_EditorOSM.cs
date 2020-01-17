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
using System.IO;

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
        pLab_OSMReader osmReader = (pLab_OSMReader) target;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        osmReader.editorData = (OSMEditorData) EditorGUILayout.ObjectField("Editor data", osmReader.editorData, typeof(OSMEditorData), false);
        if (null == osmReader.editorData) {
            if (GUILayout.Button("Create...")) {
                OSMEditorData newData = osmReader.CreateEditorData();

                if (null != newData) {
                    osmReader.editorData = newData;
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        
        osmReader.apiUrl = EditorGUILayout.TextField("API URL", osmReader.apiUrl);

        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(osmReader);
            AssetDatabase.SaveAssets();
        }

        // if (null == osmReader.editorData) {
        //     osmReader.editorData = osmReader.Open();
        // }


        editorData = osmReader.editorData;

        if (null == editorData) return;
        // Start of area coordinates

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Map area coordinates", EditorStyles.boldLabel);

        double minLat = EditorGUILayout.DoubleField("Min Lat", editorData.minLat);
        if (minLat != editorData.minLat) {
            editorData.minLat = minLat;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        double minLon = EditorGUILayout.DoubleField("Min Lon", editorData.minLon);
        if (minLon != editorData.minLon) {
            editorData.minLon = minLon;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        double maxLat = EditorGUILayout.DoubleField("Max Lat", editorData.maxLat);
        if (maxLat != editorData.maxLat) {
            editorData.maxLat = maxLat;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        double maxLon = EditorGUILayout.DoubleField("Max Lon", editorData.maxLon);
        if (maxLon != editorData.maxLon) {
            editorData.maxLon = maxLon;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        // Start of Materials

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);

        Material waterMaterial = (Material)EditorGUILayout.ObjectField("Water Material", editorData.waterMaterial, typeof(Material), false);
        if (waterMaterial != editorData.waterMaterial) {
            editorData.waterMaterial = waterMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material roadMaterial = (Material)EditorGUILayout.ObjectField("Road Material", editorData.roadMaterial, typeof(Material), false);

        if (roadMaterial != editorData.roadMaterial) {
            editorData.roadMaterial = roadMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material railwayMaterial = (Material)EditorGUILayout.ObjectField("Railway Material", editorData.railwayMaterial, typeof(Material), false);

        if (railwayMaterial != editorData.railwayMaterial) {
            editorData.railwayMaterial = railwayMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material roofMaterial = (Material)EditorGUILayout.ObjectField("Roof Material", editorData.roofMaterial, typeof(Material), false);
        if (roofMaterial != editorData.roofMaterial) {
            editorData.roofMaterial = roofMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material wallMaterial = (Material)EditorGUILayout.ObjectField("Wall Material", editorData.wallMaterial, typeof(Material), false);
        if (wallMaterial != editorData.wallMaterial) {
            editorData.wallMaterial = wallMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material green1Material = (Material)EditorGUILayout.ObjectField("Green1 Material", editorData.green1Material, typeof(Material), false);
        if (green1Material != editorData.green1Material) {
            editorData.green1Material = green1Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material green2Material = (Material)EditorGUILayout.ObjectField("Green2 Material", editorData.green2Material, typeof(Material), false);
        if (green2Material != editorData.green2Material) {
            editorData.green2Material = green2Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material green3Material = (Material)EditorGUILayout.ObjectField("Green3 Material", editorData.green3Material, typeof(Material), false);
        if (green3Material != editorData.green3Material) {
            editorData.green3Material = green3Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material brownMaterial = (Material)EditorGUILayout.ObjectField("Brown Material", editorData.brownMaterial, typeof(Material), false);
        if (brownMaterial != editorData.brownMaterial) {
            editorData.brownMaterial = brownMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        Material wetlandMaterial = (Material)EditorGUILayout.ObjectField("Wetland Material", editorData.wetlandMaterial, typeof(Material), false);
        if (wetlandMaterial != editorData.wetlandMaterial) {
            editorData.wetlandMaterial = wetlandMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        // End of Materials
        
        // Start of Layers

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);

        int waterLayer = EditorGUILayout.LayerField("Water Layer", editorData.waterLayer);

        if (waterLayer != editorData.waterLayer) {
            editorData.waterLayer = waterLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }


        int roadLayer = EditorGUILayout.LayerField("Road Layer", editorData.roadLayer);

        if (roadLayer != editorData.roadLayer) {
            editorData.roadLayer = roadLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        
        int railwayLayer = EditorGUILayout.LayerField("Railway Layer", editorData.railwayLayer);

        if (railwayLayer != editorData.railwayLayer) {
            editorData.railwayLayer = railwayLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        
        int buildingLayer = EditorGUILayout.LayerField("Building Layer", editorData.buildingLayer);

        if (buildingLayer != editorData.buildingLayer) {
            editorData.buildingLayer = buildingLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        int greenway1Layer = EditorGUILayout.LayerField("Greenway 1 Layer", editorData.greenway1Layer);

        if (greenway1Layer != editorData.greenway1Layer) {
            editorData.greenway1Layer = greenway1Layer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        int greenway2Layer = EditorGUILayout.LayerField("Greenway 2 Layer", editorData.greenway2Layer);

        if (greenway2Layer != editorData.greenway2Layer) {
            editorData.greenway2Layer = greenway2Layer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }


        int greenway3Layer = EditorGUILayout.LayerField("Greenway 3 Layer", editorData.greenway3Layer);

        if (greenway3Layer != editorData.greenway3Layer) {
            editorData.greenway3Layer = greenway3Layer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        int brownwayLayer = EditorGUILayout.LayerField("Brownway Layer", editorData.brownwayLayer);

        if (brownwayLayer != editorData.brownwayLayer) {
            editorData.brownwayLayer = brownwayLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }


        int wetlandLayer = EditorGUILayout.LayerField("Wetland Layer", editorData.wetlandLayer);

        if (wetlandLayer != editorData.wetlandLayer) {
            editorData.wetlandLayer = wetlandLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }


        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Infotext Colors", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        Color32 buildingInfoTextColor = EditorGUILayout.ColorField("Building Infotext Color", editorData.buildingInfoTextColor);
        if (EditorGUI.EndChangeCheck()) {
            editorData.buildingInfoTextColor = buildingInfoTextColor;
            EditorUtility.SetDirty(editorData);
        }

        EditorGUI.BeginChangeCheck();
        Color32 parkingLotTextColor = EditorGUILayout.ColorField("Parkinglot Infotext Color", editorData.parkingLotTextColor);
        if (EditorGUI.EndChangeCheck()) {
            editorData.parkingLotTextColor = parkingLotTextColor;
            EditorUtility.SetDirty(editorData);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Map (API)")) {

            osmReader.GetDataFromServer(
                editorData.minLat,
                editorData.minLon, 
                editorData.maxLat, 
                editorData.maxLon,
                editorData.waterMaterial, 
                editorData.roofMaterial, 
                editorData.wallMaterial, 
                editorData.roadMaterial, 
                editorData.railwayMaterial, 
                editorData.green1Material, 
                editorData.green2Material, 
                editorData.green3Material, 
                editorData.brownMaterial, 
                editorData.wetlandMaterial, 
                editorData.waterLayer,
                editorData.buildingLayer,
                editorData.roadLayer,
                editorData.railwayLayer,
                editorData.greenway1Layer,
                editorData.greenway2Layer,
                editorData.greenway3Layer,
                editorData.brownwayLayer,
                editorData.wetlandLayer,
                editorData.buildingInfoTextColor,
                editorData.parkingLotTextColor
                );
        }

        if (GUILayout.Button("Load and Generate Map from File...")) {

            string path = EditorUtility.OpenFilePanel("Select OSM file", "", "osm");

            //Check if the path is not empty and the path exists
            if (path != "" && File.Exists(path)) {
                osmReader.GenerateMapFromFile(path,
                    editorData.waterMaterial, 
                    editorData.roofMaterial, 
                    editorData.wallMaterial, 
                    editorData.roadMaterial,
                    editorData.railwayMaterial,
                    editorData.green1Material,
                    editorData.green2Material, 
                    editorData.green3Material, 
                    editorData.brownMaterial,
                    editorData.wetlandMaterial,
                    editorData.waterLayer,
                    editorData.buildingLayer,
                    editorData.roadLayer,
                    editorData.railwayLayer,
                    editorData.greenway1Layer,
                    editorData.greenway2Layer,
                    editorData.greenway3Layer,
                    editorData.brownwayLayer,
                    editorData.wetlandLayer,
                    editorData.buildingInfoTextColor,
                    editorData.parkingLotTextColor
                    );
            }

        }

    }
    #endregion
}
