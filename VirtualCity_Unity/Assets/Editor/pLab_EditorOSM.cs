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

        osmReader.editorData = (OSMEditorData) EditorGUILayout.ObjectField("Editor data", osmReader.editorData, typeof(OSMEditorData), false);
        if (null == osmReader.editorData) {
            osmReader.editorData = osmReader.Open();
        }

        editorData = osmReader.editorData;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("API URL");
        osmReader.apiUrl = EditorGUILayout.TextField(osmReader.apiUrl);
        EditorGUILayout.EndHorizontal();

        // Start of area coordinates

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Map area coordinates", EditorStyles.boldLabel);

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

        // Start of Materials

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Water Material");
        Material waterMaterial = (Material)EditorGUILayout.ObjectField("", editorData.waterMaterial, typeof(Material), false);
        if (waterMaterial != editorData.waterMaterial) {
            editorData.waterMaterial = waterMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Road Material");
        Material roadMaterial = (Material)EditorGUILayout.ObjectField("", editorData.roadMaterial, typeof(Material), false);

        if (roadMaterial != editorData.roadMaterial) {
            editorData.roadMaterial = roadMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Railway Material");
        Material railwayMaterial = (Material)EditorGUILayout.ObjectField("", editorData.railwayMaterial, typeof(Material), false);

        if (railwayMaterial != editorData.railwayMaterial) {
            editorData.railwayMaterial = railwayMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Roof Material");
        Material roofMaterial = (Material)EditorGUILayout.ObjectField("", editorData.roofMaterial, typeof(Material), false);
        if (roofMaterial != editorData.roofMaterial) {
            editorData.roofMaterial = roofMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wall Material");
        Material wallMaterial = (Material)EditorGUILayout.ObjectField("", editorData.wallMaterial, typeof(Material), false);
        if (wallMaterial != editorData.wallMaterial) {
            editorData.wallMaterial = wallMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Green1 Material");
        Material green1Material = (Material)EditorGUILayout.ObjectField("", editorData.green1Material, typeof(Material), false);
        if (green1Material != editorData.green1Material) {
            editorData.green1Material = green1Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Green2 Material");
        Material green2Material = (Material)EditorGUILayout.ObjectField("", editorData.green2Material, typeof(Material), false);
        if (green2Material != editorData.green2Material) {
            editorData.green2Material = green2Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Green3 Material");
        Material green3Material = (Material)EditorGUILayout.ObjectField("", editorData.green3Material, typeof(Material), false);
        if (green3Material != editorData.green3Material) {
            editorData.green3Material = green3Material;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brown Material");
        Material brownMaterial = (Material)EditorGUILayout.ObjectField("", editorData.brownMaterial, typeof(Material), false);
        if (brownMaterial != editorData.brownMaterial) {
            editorData.brownMaterial = brownMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wetland Material");
        Material wetlandMaterial = (Material)EditorGUILayout.ObjectField("", editorData.wetlandMaterial, typeof(Material), false);
        if (wetlandMaterial != editorData.wetlandMaterial) {
            editorData.wetlandMaterial = wetlandMaterial;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }
        EditorGUILayout.EndHorizontal();

        // End of Materials
        
        // Start of Layers

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Water Layer");
        int waterLayer = EditorGUILayout.LayerField("", editorData.waterLayer);

        if (waterLayer != editorData.waterLayer) {
            editorData.waterLayer = waterLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Road Layer");
        int roadLayer = EditorGUILayout.LayerField("", editorData.roadLayer);

        if (roadLayer != editorData.roadLayer) {
            editorData.roadLayer = roadLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Railway Layer");
        int railwayLayer = EditorGUILayout.LayerField("", editorData.railwayLayer);

        if (railwayLayer != editorData.railwayLayer) {
            editorData.railwayLayer = railwayLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();

        // EditorGUILayout.BeginHorizontal();
        // GUILayout.Label("Roof Layer");
        // int roofLayer = EditorGUILayout.LayerField("", editorData.roofLayer);

        // if (roofLayer != editorData.roofLayer) {
        //     editorData.roofLayer = roofLayer;
        //     EditorUtility.SetDirty(editorData);
        //     osmReader.SaveOSMEditorData();
        // }

        // EditorGUILayout.EndHorizontal();

        // EditorGUILayout.BeginHorizontal();
        // GUILayout.Label("Wall Layer");
        // int wallLayer = EditorGUILayout.LayerField("", editorData.wallLayer);

        // if (wallLayer != editorData.wallLayer) {
        //     editorData.wallLayer = wallLayer;
        //     EditorUtility.SetDirty(editorData);
        //     osmReader.SaveOSMEditorData();
        // }

        // EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Building Layer");
        int buildingLayer = EditorGUILayout.LayerField("", editorData.buildingLayer);

        if (buildingLayer != editorData.buildingLayer) {
            editorData.buildingLayer = buildingLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Greenway 1 Layer");
        int greenway1Layer = EditorGUILayout.LayerField("", editorData.greenway1Layer);

        if (greenway1Layer != editorData.greenway1Layer) {
            editorData.greenway1Layer = greenway1Layer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Greenway 2 Layer");
        int greenway2Layer = EditorGUILayout.LayerField("", editorData.greenway2Layer);

        if (greenway2Layer != editorData.greenway2Layer) {
            editorData.greenway2Layer = greenway2Layer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Greenway 3 Layer");
        int greenway3Layer = EditorGUILayout.LayerField("", editorData.greenway3Layer);

        if (greenway3Layer != editorData.greenway3Layer) {
            editorData.greenway3Layer = greenway3Layer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brownway Layer");
        int brownwayLayer = EditorGUILayout.LayerField("", editorData.brownwayLayer);

        if (brownwayLayer != editorData.brownwayLayer) {
            editorData.brownwayLayer = brownwayLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wetland Layer");
        int wetlandLayer = EditorGUILayout.LayerField("", editorData.wetlandLayer);

        if (wetlandLayer != editorData.wetlandLayer) {
            editorData.wetlandLayer = wetlandLayer;
            EditorUtility.SetDirty(editorData);
            osmReader.SaveOSMEditorData();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Map (API)")) {

            osmReader.GetDataFromServer(
                double.Parse(editorData.minLat),
                double.Parse(editorData.minLon), 
                double.Parse(editorData.maxLat), 
                double.Parse(editorData.maxLon),
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
                // editorData.roofLayer,
                // editorData.wallLayer,
                editorData.roadLayer,
                editorData.railwayLayer,
                editorData.greenway1Layer,
                editorData.greenway2Layer,
                editorData.greenway3Layer,
                editorData.brownwayLayer,
                editorData.wetlandLayer
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
                    // editorData.roofLayer,
                    // editorData.wallLayer,
                    editorData.roadLayer,
                    editorData.railwayLayer,
                    editorData.greenway1Layer,
                    editorData.greenway2Layer,
                    editorData.greenway3Layer,
                    editorData.brownwayLayer,
                    editorData.wetlandLayer
                    );
            }

        }

    }
    #endregion
}
