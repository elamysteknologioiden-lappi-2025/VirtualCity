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
using Assets.Tools.OSMRoad.Scripts.Objects;
using Assets.Tools.OSMRoad.Scripts.XmlObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

/// <summary>
/// Enum for Type (~sub-category / use type)
/// </summary>
public enum CategoryType {
    ENone,
    ECycleWay,
    ERoad,
    EMainRoad,
    ECemetery,
    EForest,
    EScrub,
    EOrchard
};

/// <summary>
/// Enum for parsing into big categories
/// </summary>
public enum InfraType {
    ENone,
    EHighway,
    EBuilding,
    EWater,
    EGreen1,
    EGreen2,
    ERailWay,
    EParkingLot,
    EWetland,
    EGreen3,
    EBrown,
    EStream
};

/// <summary>
/// pLab_OSMReader
/// </summary>
public class pLab_OSMReader : MonoBehaviour {

    public OSMEditorData editorData;

    // API CALL
    // Example : https://api.openstreetmap.org/api/0.6/map?bbox=11.54,48.14,11.543,48.145
    // min lon - min lat - max lon -  max lat
    public string apiUrl = "https://api.openstreetmap.org/api/0.6/map?bbox=";

    /// <summary>
    /// XML document to load OSM data into
    /// </summary>
    private XmlDocument doc = new XmlDocument();

    /// <summary>
    /// coordinates
    /// </summary>
    private double x;
    private double y;

    /// <summary>
    /// UTM Zero Points
    /// </summary>
    private double UTMN_Zero = 0;
    private double UTME_Zero = 0;

    /// <summary>
    /// Water Material
    /// </summary>
    private Material waterMaterial;

    /// <summary>
    /// Roof Material
    /// </summary>
    private Material roofMaterial;

    /// <summary>
    /// Wall Material
    /// </summary>
    private Material wallMaterial;

    /// <summary>
    /// Road Material
    /// </summary>
    private Material roadMaterial;

    /// <summary>
    /// Railway Material
    /// </summary>
    private Material railwayMaterial;

    /// <summary>
    /// Green area Material
    /// </summary>
    private Material green1Material;

    /// <summary>
    /// Green area Material
    /// </summary>
    private Material green2Material;

    /// <summary>
    /// Green area Material
    /// </summary>
    private Material green3Material;

    /// <summary>
    /// brownMaterial
    /// </summary>
    private Material brownMaterial;

    /// <summary>
    /// Wetland Material
    /// </summary>
    private Material wetlandMaterial;

    private int waterLayer = 4;

    private int roadLayer = 0;

    private int railwayLayer = 0;

    private int buildingLayer = 0;
    // private int wallLayer = 0;

    // private int roofLayer = 0;

    private int greenway1Layer = 0;

    private int greenway2Layer = 0;

    private int greenway3Layer = 0;

    private int brownwayLayer = 0;

    private int wetlandLayer = 0;

    private int UILayer = 5;

    private Color32 buildingInfoTextColor = Color.red;

    private Color32 parkinglotTextColor = Color.blue;

    /// <summary>
    /// List of nodes
    /// </summary>
    private List<Node> allnodes = new List<Node>();

    /// <summary>
    /// List of ways
    /// </summary>
    private List<Way> allways = new List<Way>();

    /// <summary>
    /// Water Relations
    /// </summary>
    private List<Relation> waterrelations = new List<Relation>();

    /// <summary>
    /// Wetland relations
    /// </summary>
    private List<Relation> wetlandrelations = new List<Relation>();

    /// <summary>
    /// Green Relations
    /// </summary>
    private List<Relation> green1relations = new List<Relation>();

    /// <summary>
    /// Green Relations
    /// </summary>
    private List<Relation> green2relations = new List<Relation>();

    /// <summary>
    /// Green Relations
    /// </summary>
    private List<Relation> green3relations = new List<Relation>();

    /// <summary>
    /// Green Relations
    /// </summary>
    private List<Relation> brownrelations = new List<Relation>();

    /// <summary>
    /// Water relation objects
    /// </summary>
    private List<Transform> waterrelationsObjects = new List<Transform>();

    /// <summary>
    /// Wetland relation objects
    /// </summary>
    private List<Transform> wetlandrelationsObjects = new List<Transform>();

    /// <summary>
    /// Green relation objects
    /// </summary>
    private List<Transform> green1relationsObjects = new List<Transform>();

    /// <summary>
    /// Green relation objects
    /// </summary>
    private List<Transform> green2relationsObjects = new List<Transform>();

    /// <summary>
    /// Green relation objects
    /// </summary>
    private List<Transform> green3relationsObjects = new List<Transform>();

    /// <summary>
    /// Green relation objects
    /// </summary>
    private List<Transform> brownrelationsObjects = new List<Transform>();

    /// <summary>
    /// Green objects
    /// </summary>
    private List<Transform> green1waysObjects = new List<Transform>();

    /// <summary>
    /// Green objects
    /// </summary>
    private List<Transform> green2waysObjects = new List<Transform>();

    /// <summary>
    /// Green objects
    /// </summary>
    private List<Transform> green3waysObjects = new List<Transform>();

    /// <summary>
    /// brown objects
    /// </summary>
    private List<Transform> brownwaysObjects = new List<Transform>();

    /// <summary>
    /// parkinglot objects
    /// </summary>
    private List<Transform> parkinglotwaysObjects = new List<Transform>();

    /// <summary>
    /// waterway objects
    /// </summary>
    private List<Transform> waterwayObjects = new List<Transform>();

    /// <summary>
    /// wetland objects
    /// </summary>
    private List<Transform> wetlandwayObjects = new List<Transform>();

    /// <summary>
    /// moved object objects
    /// </summary>
    private List<Transform> alreadyMovedObjects = new List<Transform>();

    // List of ways
    private List<Way> green1ways = new List<Way>();
    private List<Way> green2ways = new List<Way>();
    private List<Way> green3ways = new List<Way>();
    private List<Way> brownways = new List<Way>();
    private List<Way> waterways = new List<Way>();
    private List<Way> wetlandways = new List<Way>();
    private List<Way> parkinglotWays = new List<Way>();

    private List<WayList> waylists = new List<WayList>();
    private List<BuildingInfo> buildingInfos = new List<BuildingInfo>();
    private List<GameObject> buildingInfoObjects = new List<GameObject>();

    /// <summary>
    /// Finished Generating
    /// </summary>
    private bool finishedGeneratingMap = false;

    /// <summary>
    /// Is the data coming from file (or server)
    /// </summary>
    private bool isGeneratingFromFile = false;

    /// <summary>
    /// Helper GameObject to serve as a parent for SingleObjects
    /// </summary>
    private GameObject SingleObjectParent = null;

    /// <summary>
    ///  Counter for processed blocks
    /// </summary>
    private int readyCounter = 0;

    /// <summary>
    ///  Nav Mesh instance, used to generate final road mesh
    /// </summary>
    NavMeshDataInstance navMeshDataInstance;

    /// <summary>
    ///  Layer Mask for navmesh
    /// </summary>
    public LayerMask includedLayerMask = 0;

    #region Properties
    
    #endregion

    /// <summary>
    /// Loads OSMEditorData if it exists, create and save if it doesnt
    /// </summary>
    /// <returns></returns>
    public OSMEditorData Open() {
        OSMEditorData asset = null;

        #if UNITY_EDITOR //Editor only tag
        string[] assetGuids = AssetDatabase.FindAssets(string.Format("t:{0} {1}", typeof(OSMEditorData).Name, "OSMDATA_" + EditorSceneManager.GetActiveScene().name));
        if (assetGuids.Length > 0) {
            asset = (OSMEditorData) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetGuids[0]), typeof(OSMEditorData));
        }
        // OSMEditorData asset = AssetDatabase.LoadAssetAtPath("Assets/OSMDATA_" + EditorSceneManager.GetActiveScene().name + ".asset", typeof(OSMEditorData)) as OSMEditorData;
        if (asset == null) {
            asset = CreateEditorData();
        }
        if (asset == null) {
            return null;
        }
        #endif

        return asset;
    }

    /// <summary>
    /// Finds the UI layer and saves it to UILayer-variable
    /// </summary>
    private void FindAndInitializeUILayer() {
        // Find the UI layer
        int UILayerIndex = LayerMask.NameToLayer("UI");

        if (UILayerIndex != -1) {
            UILayer = UILayerIndex;
        }
    }

    /// <summary>
    /// Sets initial properties, divides the area into blocks and calls API get request for the blocks
    /// </summary>
    /// <param name="aMinLat">Minimum latitude</param>
    /// <param name="aMinLon">Minimum longitude</param>
    /// <param name="aMaxLat">Maximum latitude</param>
    /// <param name="aMaxLon">Maximum longitude</param>
    /// <param name="aWaterMaterial">Water material</param>
    /// <param name="aRoofMaterial">Roof material</param>
    /// <param name="aWallMaterial">Wall material</param>
    /// <param name="aRoadMaterial">Road material</param>
    /// <param name="aRailwayMaterial">Railway material</param>
    /// <param name="aGreen1Material">Green1 material</param>
    /// <param name="aGreen2Material">Green2 material</param>
    /// <param name="aGreen3Material">Green3 material</param>
    /// <param name="aBrownMaterial">Brown material</param>
    /// <param name="aWetlandMaterial">Wetland material</param>
    /// <param name="aWaterLayer">Water layer</param>
    /// <param name="aRoadLayer">Road layer</param>
    /// <param name="aRailwayLayer">Railway layer</param>
    /// <param name="aGreenway1Layer">Greenway 1 layer</param>
    /// <param name="aGreenway2Layer">Greenway 2 layer</param>
    /// <param name="aGreenway3Layer">Greenway 3 layer</param>
    /// <param name="aBrownwayLayer">Brownway layer</param>
    /// <param name="aWetlandLayer">Wetland layer</param>
    public void GetDataFromServer(double aMinLat, double aMinLon, double aMaxLat, double aMaxLon,
        Material aWaterMaterial = null, Material aRoofMaterial = null, Material aWallMaterial = null,
        Material aRoadMaterial = null, Material aRailwayMaterial = null, Material aGreen1Material = null,
        Material aGreen2Material = null, Material aGreen3Material = null, Material aBrownMaterial = null,
        Material aWetlandMaterial = null, int aWaterLayer = 4, int aBuildingLayer = 0, int aRoadLayer = 0, int aRailwayLayer = 0, 
        int aGreenway1Layer = 0, int aGreenway2Layer = 0, int aGreenway3Layer = 0, int aBrownwayLayer = 0, int aWetlandLayer = 0,
        Color32? aBuildingInfoTextColor = null, Color32? aParkinglotTextColor = null) {

        isGeneratingFromFile = false;

        waterMaterial = aWaterMaterial;
        roofMaterial = aRoofMaterial;
        wallMaterial = aWallMaterial;
        roadMaterial = aRoadMaterial;
        railwayMaterial = aRailwayMaterial;
        green1Material = aGreen1Material;
        green2Material = aGreen2Material;
        green3Material = aGreen3Material;
        brownMaterial = aBrownMaterial;
        wetlandMaterial = aWetlandMaterial;

        waterLayer = aWaterLayer;
        buildingLayer = aBuildingLayer;
        // roofLayer = aRoofLayer;
        // wallLayer = aWallLayer;
        roadLayer = aRoadLayer;
        railwayLayer = aRailwayLayer;
        greenway1Layer = aGreenway1Layer;
        greenway2Layer = aGreenway2Layer;
        greenway3Layer = aGreenway3Layer;
        brownwayLayer = aBrownwayLayer;
        wetlandLayer = aWetlandLayer;

        buildingInfoTextColor = aBuildingInfoTextColor ?? buildingInfoTextColor;

        parkinglotTextColor = aParkinglotTextColor ?? parkinglotTextColor;

        FindAndInitializeUILayer();

        double latBlock = 0.005;
        double lonBlock = 0.010;

        int latBlockCount = Mathf.CeilToInt((((float)((aMaxLat - aMinLat) / latBlock))));
        int lonBlockCount = Mathf.CeilToInt((((float)((aMaxLon - aMinLon) / lonBlock))));

        // Convert latitude and longitude to UTM format
        pLAB_GeoUtils.LatLongtoUTM(aMinLat, aMinLon, out UTMN_Zero, out UTME_Zero);

        int blockCount = 1;
        int requestCount = latBlockCount * lonBlockCount;
        readyCounter = 0;

        // Get data from server
        for (int ilat = 0; ilat < latBlockCount; ilat++) {
            for (int ilon = 0; ilon < lonBlockCount; ilon++) {
                string tmpUrl = apiUrl;
                tmpUrl += (aMinLon + lonBlock * ilon).ToString(CultureInfo.InvariantCulture) + ",";
                tmpUrl += (aMinLat + latBlock * ilat).ToString(CultureInfo.InvariantCulture) + ",";
                tmpUrl += (aMinLon + lonBlock * (ilon + 1)).ToString(CultureInfo.InvariantCulture) + ",";
                tmpUrl += (aMinLat + latBlock * (ilat + 1)).ToString(CultureInfo.InvariantCulture);
                GetRequest(tmpUrl, blockCount, requestCount);
                blockCount++;
            }
        }

    }

    /// <summary>
    /// GenerateMapFromFile
    /// </summary>
    /// <param name="aPath"></param>
    /// <param name="aWaterMaterial"></param>
    /// <param name="aRoofMaterial"></param>
    /// <param name="aWallMaterial"></param>
    /// <param name="aRoadMaterial"></param>
    /// <param name="aRailwayMaterial"></param>
    /// <param name="aGreen1Material"></param>
    /// <param name="aGreen2Material"></param>
    /// <param name="aGreen3Material"></param>
    /// <param name="aBrownMaterial"></param>
    /// <param name="aWetlandMaterial"></param>
    /// <param name="aWaterLayer">Water layer</param>
    /// <param name="aBuildingLayer">Building layer</param>
    /// <param name="aRoadLayer">Road layer</param>
    /// <param name="aRailwayLayer">Railway layer</param>
    /// <param name="aGreenway1Layer">Greenway 1 layer</param>
    /// <param name="aGreenway2Layer">Greenway 2 layer</param>
    /// <param name="aGreenway3Layer">Greenway 3 layer</param>
    /// <param name="aBrownwayLayer">Brownway layer</param>
    /// <param name="aWetlandLayer">Wetland layer</param>
    public void GenerateMapFromFile(string aPath,
        Material aWaterMaterial = null, Material aRoofMaterial = null, Material aWallMaterial = null,
        Material aRoadMaterial = null, Material aRailwayMaterial = null, Material aGreen1Material = null,
        Material aGreen2Material = null, Material aGreen3Material = null, Material aBrownMaterial = null,
        Material aWetlandMaterial = null, int aWaterLayer = 4, int aBuildingLayer = 0, int aRoadLayer = 0, int aRailwayLayer = 0, 
        int aGreenway1Layer = 0, int aGreenway2Layer = 0, int aGreenway3Layer = 0, int aBrownwayLayer = 0, int aWetlandLayer = 0,
        Color32? aBuildingInfoTextColor = null, Color32? aParkinglotTextColor = null)
    {

        isGeneratingFromFile = true;
        waterMaterial = aWaterMaterial;
        roofMaterial = aRoofMaterial;
        wallMaterial = aWallMaterial;
        roadMaterial = aRoadMaterial;
        railwayMaterial = aRailwayMaterial;
        green1Material = aGreen1Material;
        green2Material = aGreen2Material;
        green3Material = aGreen3Material;
        brownMaterial = aBrownMaterial;
        wetlandMaterial = aWetlandMaterial;

        waterLayer = aWaterLayer;
        buildingLayer = aBuildingLayer;
        // roofLayer = aRoofLayer;
        // wallLayer = aWallLayer;
        roadLayer = aRoadLayer;
        railwayLayer = aRailwayLayer;
        greenway1Layer = aGreenway1Layer;
        greenway2Layer = aGreenway2Layer;
        greenway3Layer = aGreenway3Layer;
        brownwayLayer = aBrownwayLayer;
        wetlandLayer = aWetlandLayer;

        buildingInfoTextColor = aBuildingInfoTextColor ?? buildingInfoTextColor;

        parkinglotTextColor = aParkinglotTextColor ?? parkinglotTextColor;

        FindAndInitializeUILayer();

        // double latBlock = 0.005;
        // double lonBlock = 0.010;

        string content = File.ReadAllText(aPath);

        List<Node> nodes = new List<Node>();
        doc.LoadXml(content);

        XmlNodeList elementList = doc.GetElementsByTagName("node");
        XmlNodeList boundsList = doc.GetElementsByTagName("bounds");

        if (boundsList.Count > 0) {
            XmlNode boundNode = boundsList[0];
            double minLat = double.Parse(boundNode.Attributes["minlat"].InnerText, CultureInfo.InvariantCulture);
            double minLon = double.Parse(boundNode.Attributes["minlon"].InnerText, CultureInfo.InvariantCulture);

            pLAB_GeoUtils.LatLongtoUTM(minLat, minLon, out UTMN_Zero, out UTME_Zero);
        } else {

            double minLat = 99999;
            double minLon = 99999;

            foreach (XmlNode i in elementList) {
                string latText = i.Attributes["lat"].InnerText;

                if (latText.Length > 0) {

                    double latDouble;
                    if (double.TryParse(latText, out latDouble)) {
                        if( minLat >= latDouble) {
                            minLat = latDouble;
                        }
                    }
                }

                string lonText = i.Attributes["lon"].InnerText;

                if (lonText.Length > 0) {

                    double lonDouble;

                    if (double.TryParse(lonText, out lonDouble)) {
                        if (minLon >= lonDouble) {
                            minLon = lonDouble;
                        }
                    }
                }
            }

            pLAB_GeoUtils.LatLongtoUTM(minLat, minLon, out UTMN_Zero, out UTME_Zero);
        }

        Create(content, 1, UTMN_Zero, UTME_Zero);

        HandleMultiPoly();
        finishedGeneratingMap = true;
        ClearLists();
        #if UNITY_EDITOR
        EditorUtility.UnloadUnusedAssetsImmediate();
        #endif
    }

        /// <summary>
        /// Starts GetRequestCoroutine
        /// </summary>
        /// <param name="aRequest"></param>
        /// <param name="aBlockCount"></param>
        /// <param name="aRequestCount"></param>
    public void GetRequest(string aRequest, int aBlockCount, int aRequestCount) {
        StartCoroutine(GetRequestCoroutine(aRequest, aBlockCount, aRequestCount));
    }

    /// <summary>
    /// Makes the request to the server for data of blocks, stores response, Creates XML document from response to Read Data from. Calls polygon GameObject creation if all the blocks are reveiced.
    /// </summary>
    /// <param name="aRequest"></param>
    /// <param name="aBlockCount"></param>
    /// <param name="aRequestCount"></param>
    /// <returns></returns>
    IEnumerator GetRequestCoroutine(string aRequest, int aBlockCount, int aRequestCount) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(aRequest)) {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError) {
                Debug.LogError($"Error requesting map data for blockCount {aBlockCount}: {webRequest.error}");
            }
            else {
                Create(webRequest.downloadHandler.text, aBlockCount);
                readyCounter++;
                if (readyCounter == aRequestCount) {
                    HandleMultiPoly();
                    finishedGeneratingMap = true;
                    ClearLists();
                    #if UNITY_EDITOR
                    EditorUtility.UnloadUnusedAssetsImmediate();
                    #endif
                }
            }
        }
    }

    /// <summary>
    /// Create  and save OSMEditorData  
    /// </summary>
    /// <returns></returns>
    public OSMEditorData CreateEditorData() {
        OSMEditorData asset = null;

        #if UNITY_EDITOR //Editor only tag
        string scene = EditorSceneManager.GetActiveScene().name;

        string path = EditorUtility.SaveFilePanelInProject("Save to...", scene, "asset", "Select where to save file");

        //Check if the path is not empty and the path exists
        if (path == null || path == "") return null;

        asset = ScriptableObject.CreateInstance<OSMEditorData>();

        string assetPath = path;

        try {
            AssetDatabase.CreateAsset(asset, assetPath);
        } catch (Exception e) {
            Debug.LogError(e.Message);
            return null;
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Created asset file for VirtualCity scene " + scene + " to " + assetPath);
        EditorGUIUtility.PingObject(asset);
        #endif

        return asset;
    }

    /// <summary>
    /// Save OSMEDitorData
    /// </summary>
    public void SaveOSMEditorData() {
        #if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        #endif
    }

    /// <summary>
    /// Create XML file for parsing from data from the server
    /// </summary>
    /// <param name="aXML">Data File</param>
    /// <param name="aBlockCount">Index Number of Block</param>
    public void Create(string aXML, int aBlockCount, double utmX = 0, double utmY = 0) {

        List<Node> nodes = new List<Node>();
        doc.LoadXml(aXML);
        XmlNodeList elementList = doc.GetElementsByTagName("node");
        XmlNodeList boundsList = doc.GetElementsByTagName("bounds");
        XmlNode boundNode = boundsList[0];

        // double xBound = 0;
        // double yBound = 0;
        if(utmX == 0 && utmY == 0) {
            double minLat = double.Parse(boundNode.Attributes["minlat"].InnerText, CultureInfo.InvariantCulture);
            double minLon = double.Parse(boundNode.Attributes["minlon"].InnerText, CultureInfo.InvariantCulture);

            pLAB_GeoUtils.LatLongtoUTM(minLat, minLon, out utmX, out utmY);
        }

        utmX = utmX - UTMN_Zero;
        utmY = utmY - UTME_Zero;

        // Debug.LogFormat("Min Lat UTM (UTMX): {0}, Min Lon UTM (UTMY): {1}", utmX, utmY);
        // pLab_GeoMap geoMap = GameObject.FindObjectOfType<pLab_GeoMap>();

        foreach (XmlNode attr in elementList) {
            double UTMN = 0;
            double UTME = 0;
            double latAttr = double.Parse(attr.Attributes["lat"].InnerText, CultureInfo.InvariantCulture);
            double lonAttr = double.Parse(attr.Attributes["lon"].InnerText, CultureInfo.InvariantCulture);

            pLAB_GeoUtils.LatLongtoUTM(latAttr, lonAttr, out UTMN, out UTME);
            var node = new Node(long.Parse(attr.Attributes["id"].InnerText), UTMN, UTME);
            nodes.Add(node);
            if (allnodes.Find(a => a.Id == node.Id) == null) allnodes.Add(node);
        }
        // Read the data in the current block
        ReadData(nodes, aBlockCount, utmX, utmY);
    }

    /// <summary>
    /// Calls polygon GameObject creation for each category
    /// </summary>
    public void HandleMultiPoly() {
        Transform wayObjectsParent = new GameObject("Wayobjects").transform;

        Transform greenway1ObjectParent = new GameObject("Greenway1Objects").transform;
        greenway1ObjectParent.transform.SetParent(wayObjectsParent, wayObjectsParent);
        CreateWayObjects("Green1", green1waysObjects, green1ways, green1Material, greenway1Layer, greenway1ObjectParent);

        Transform greenway2ObjectParent = new GameObject("Greenway2Objects").transform;
        greenway2ObjectParent.transform.SetParent(wayObjectsParent);
        CreateWayObjects("Green2", green2waysObjects, green2ways, green2Material, greenway2Layer, greenway2ObjectParent);
        
        Transform greenway3ObjectParent = new GameObject("Greenway3Objects").transform;
        greenway3ObjectParent.transform.SetParent(wayObjectsParent);
        CreateWayObjects("Green3", green3waysObjects, green3ways, green3Material, greenway3Layer, greenway3ObjectParent);
        
        Transform brownwayObjectParent = new GameObject("BrownwayObjects").transform;
        brownwayObjectParent.transform.SetParent(wayObjectsParent);
        CreateWayObjects("Brown", brownwaysObjects, brownways, brownMaterial, brownwayLayer, brownwayObjectParent);
        
        Transform parkinglotsObjectParent = new GameObject("ParkinglotsObjects").transform;
        parkinglotsObjectParent.transform.SetParent(wayObjectsParent);
        CreateWayObjects("ParkingLot", parkinglotwaysObjects, parkinglotWays, roadMaterial, roadLayer, parkinglotsObjectParent);
        
        Transform waterwayObjectParent = new GameObject("WaterwayObjects").transform;
        waterwayObjectParent.transform.SetParent(wayObjectsParent);
        CreateWayObjects("Water", waterwayObjects, waterways, waterMaterial, waterLayer, waterwayObjectParent);
        
        Transform wetlandObjectParent = new GameObject("WetlandObjects").transform;
        wetlandObjectParent.transform.SetParent(wayObjectsParent);
        CreateWayObjects("Wetland", wetlandwayObjects, wetlandways, wetlandMaterial, wetlandLayer, wetlandObjectParent);
        
        Transform relationObjectsParent = new GameObject("RelationObjects").transform;

        Transform waterRelationsObjectParent = new GameObject("WaterRelations").transform;
        waterRelationsObjectParent.SetParent(relationObjectsParent);
        CreateRelationObjects("Water", waterrelationsObjects, waterrelations, waterMaterial, waterLayer, waterRelationsObjectParent.transform);

        Transform wetlandRelationsObjectParent = new GameObject("WetlandRelations").transform;
        wetlandRelationsObjectParent.SetParent(relationObjectsParent);
        CreateRelationObjects("Wetland", wetlandrelationsObjects, wetlandrelations, wetlandMaterial, wetlandLayer, wetlandRelationsObjectParent.transform);

        Transform green1RelationsObjectParent = new GameObject("Green1Relations").transform;
        green1RelationsObjectParent.SetParent(relationObjectsParent);
        CreateRelationObjects("Green1", green1relationsObjects, green1relations, green1Material, greenway1Layer, green1RelationsObjectParent.transform);

        Transform green2RelationsObjectParent = new GameObject("Green2Relations").transform;
        green2RelationsObjectParent.SetParent(relationObjectsParent);
        CreateRelationObjects("Green2", green2relationsObjects, green2relations, green2Material, greenway2Layer, green2RelationsObjectParent.transform);

        Transform green3RelationsObjectParent = new GameObject("Green3Relations").transform;
        green3RelationsObjectParent.SetParent(relationObjectsParent);
        CreateRelationObjects("Green3", green3relationsObjects, green3relations, green3Material, greenway3Layer, green3RelationsObjectParent.transform);

        Transform brownRelationsObjectParent = new GameObject("BrownRelations").transform;
        brownRelationsObjectParent.SetParent(relationObjectsParent);
        CreateRelationObjects("Brown", brownrelationsObjects, brownrelations, brownMaterial, brownwayLayer, brownRelationsObjectParent.transform);

    }

    /// <summary>
    /// Create polygon Gameobjects for relations of a certain category and add mesh to it with given material
    /// </summary>
    /// <param name="category">Name of the category</param>
    /// <param name="relationobjects">List of 3D objects to fill with the GameObjects</param>
    /// <param name="relations">List of relations that contains the data to create the GameObjects</param>
    /// <param name="material">The material to be set for the created GameObjects mesh</param>
    /// <param name="layer">The # of the layer of the category</param>
    /// <param name="parentObject">Parent object (transform). New object will be created as a child of this transform.</param>
    private void CreateRelationObjects(string category, List<Transform> relationobjects, List<Relation> relations, Material material, int layer, Transform parentObject = null) {
        relationobjects.Clear();
        for (int i = 0; i < relations.Count; i++) {
            Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
            GameObject go = new GameObject(category + "Relations" + relations[i].Id);

            if (parentObject != null) {
                go.transform.SetParent(parentObject);
            }

            relationobjects.Add(go.transform);


            poly.outside = relations[i].OuterVectors;
            for (int io = 0; io < relations[i].InnerVectorLists.Count; io++) {
                poly.holes.Add(relations[i].InnerVectorLists[io].InnerVectors);
            }


            if (poly.outside.Count > 2) poly.CalcPlaneNormal(Vector3.up);



            if (relations[i].OuterVectors.Count > 0) {
                Poly2Mesh.CreateGameObject(poly, go);
                MeshRenderer goMeshRenderer = go.GetComponent<MeshRenderer>();
                goMeshRenderer.sharedMaterial = material;
                goMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                go.layer = layer;
                MeshCollider meshCollider = go.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;

                FixOnTopLayers(go);

                switch (relations[i].CategoryType) {
                    case ((int)CategoryType.EForest):
                        PlaceObjects("tree", 10, go, 15, true, 4, layer);
                        break;

                    case ((int)CategoryType.ECemetery):
                        PlaceObjects("tombstones", 2, go, 15, false, 0, layer);
                        break;

                    case ((int)CategoryType.EScrub):
                        PlaceObjects("bush", 3, go, 6, true, 2, layer);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Create polygon Gameobjects for ways of a certain category and add mesh to it with given material
    /// </summary>
    /// <param name="category">Name of the category</param>
    /// <param name="wayobjects">List of 3D objects to fill with the GameObjects</param>
    /// <param name="ways">List of ways that contains the data to create the GameObjects</param>
    /// <param name="material">The material to be set for the created GameObjects mesh</param>
    /// <param name="layer">The # of the layer of the category</param>
    /// <param name="parentObject">Parent object (transform). New object will be created as a child of this transform.</param>
    private void CreateWayObjects(string category, List<Transform> wayobjects, List<Way> ways, Material material, int layer, Transform parentObject = null) {
        wayobjects.Clear();
        for (int i = 0; i < ways.Count; i++) {
            List<Vector3> tempVectors = new List<Vector3>();
            
            WayList tmpWaylist = waylists.Find(a => a.WayId == ways[i].Id);

            //If the waylist was not on the list, add it
            if (tmpWaylist == null) {
                tmpWaylist = new WayList(ways[i].Id);
                waylists.Add(tmpWaylist);
            }

            if (tmpWaylist != null && !tmpWaylist.Filled) {
                tmpWaylist.Filled = true;
                Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
                GameObject go = new GameObject(category + "Ways" + ways[i].Id);

                if (parentObject != null) {
                    go.transform.SetParent(parentObject);
                }

                wayobjects.Add(go.transform);

                var list = ways[i].WayNodeIds.Distinct();

                foreach (long nodRef in list) {
                    Node wayNode = allnodes.Find(x => x.Id == nodRef);
                    if (wayNode != null) {
                        x = wayNode.Latitude;
                        y = wayNode.Longitude;

                        float z = 0;

                        switch (category) {
                            case "Water":
                                z += 0.1f;
                                break;
                            case "Wetland":
                                z += 0.2f;
                                break;
                            default:
                                break;
                        }
                        tempVectors.Add(new Vector3((float)(y - UTME_Zero), z, (float)(x - UTMN_Zero)));
                    }
                }
                poly.outside = tempVectors;
                poly.CalcPlaneNormal(Vector3.up);


                if (tempVectors.Count > 0) {
                    GameObject gameObject = Poly2Mesh.CreateGameObject(poly, go);

                    MeshRenderer goMeshRenderer = go.GetComponent<MeshRenderer>();

                    goMeshRenderer.sharedMaterial = material;
                    goMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                    gameObject.layer = layer;

                    MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

                    FixOnTopLayers(gameObject);

                    switch (ways[i].CategoryType) {
                        case ((int)CategoryType.EForest):
                            PlaceObjects("tree", 10, gameObject, 15, true, 4, layer);
                            break;

                        case ((int)CategoryType.ECemetery):
                            PlaceObjects("tombstones", 2, gameObject, 15, false, 0, layer);
                            break;

                        case ((int)CategoryType.EScrub):
                            PlaceObjects("bush", 3, gameObject, 6, true, 2, layer);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if gameObject collides with other GameObjects on its elevation, if there is collision move one of the objects up or down
    /// </summary>
    /// <param name="gameObject">The GameObject to check</param>
    private void FixOnTopLayers(GameObject gameObject) {

        List<RaycastHit> hits = new List<RaycastHit>();
        RaycastHit[] hits2;

        var bounds = gameObject.GetComponent<MeshCollider>().bounds;

        for (float i = bounds.min.x; i < bounds.max.x; i += 15) {
            for (float j = bounds.min.z; j < bounds.max.z; j++) {
                hits2 = Physics.RaycastAll(new Ray(new Vector3(i, 3.9f, j), Vector3.down));
                for (int ii = 0; ii < hits2.Length; ii++) {
                    hits.Add(hits2[ii]);
                }

            }
        }

        List<MeshCollider> colliders = new List<MeshCollider>();

        foreach (var item in hits) {
            if (item.collider.GetType() == typeof(MeshCollider)) {
                colliders.Add(item.collider as MeshCollider);
            }
        }

        colliders = colliders.Distinct().ToList();

        colliders.RemoveAll(a => a.gameObject.layer == gameObject.layer);

        for (int n = 0; n < colliders.Count; n++) {
            var collhit = colliders[n];
            var gObjhit = colliders[n].gameObject;

            bool originalmoved = alreadyMovedObjects.Contains(gameObject.transform);
            bool hitmoved = alreadyMovedObjects.Contains(gameObject.transform);

            // if they are on the same elevation currently
            if (gameObject.transform.position.y == gObjhit.transform.position.y) {
                // if original is bigger than hit
                if (bounds.size.sqrMagnitude > collhit.bounds.size.sqrMagnitude) {
                    if (!originalmoved) {
                        if (!hitmoved) {
                            gObjhit.transform.position += new Vector3(0, 0.1f, 0);
                            alreadyMovedObjects.Add(gObjhit.transform);
                        }

                        if (hitmoved) {
                            gameObject.transform.position += new Vector3(0, -0.1f, 0);
                            alreadyMovedObjects.Add(gameObject.transform);
                        }
                    }

                    if (originalmoved) {
                        if (!hitmoved) {
                            gObjhit.transform.position += new Vector3(0, 0.1f, 0);
                            alreadyMovedObjects.Add(gObjhit.transform);
                        }
                        if (hitmoved) {
                        }
                    }
                }

                // if original is smaller than hit
                if (bounds.size.sqrMagnitude < collhit.bounds.size.sqrMagnitude) {
                    if (!originalmoved) {
                        gameObject.transform.position += new Vector3(0, 0.1f, 0);
                        alreadyMovedObjects.Add(gameObject.transform);
                    }
                    if (originalmoved) {
                        if (!hitmoved) {
                            gObjhit.transform.position += new Vector3(0, -0.1f, 0);
                            alreadyMovedObjects.Add(gObjhit.transform);
                        }
                        // if (hitmoved) {
                        // }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Place 3D objects of a type eg.: trees on an area eg. a wayObject or relationObject
    /// </summary>
    /// <param name="modelType" >The type of object to place eg.: tree</param>
    /// <param name="modelsCount">The number of different models of the type</param>
    /// <param name="go">The GameObject of the area to place objects on</param>
    /// <param name="step">The base distance of an iteration while placing objects (recommended ~15 for trees, ~6 for bushes)</param>
    /// <param name="distance">The max/min random value that is added to or subtracted from position to provide randomness. Set to 0 if method is called with randomGap = false.</param>
    /// <param name="randomGap">Create randomness in the gaps between objects. Set the min/max random distance in the "distance" parameter</param>
    /// <param name="parentLayer">Parent's layer. Object will be placed on this layer also</param>
    private void PlaceObjects(string modelType, int modelsCount, GameObject go, float step, bool randomGap, float distance, int parentLayer) {
        MeshCollider meshCollider = go.GetComponent<MeshCollider>();

        Bounds box = meshCollider.bounds;

        modelsCount += 1;
        RaycastHit rayHit;
        RaycastHit rayHit2;
        List<RaycastHit> hits;

        for (float X = box.min.x; X < box.max.x; X = X + step) {
            for (float Y = box.min.z; Y < box.max.z; Y = Y + step) {
                float plusX = UnityEngine.Random.Range(-distance, distance);
                float plusY = UnityEngine.Random.Range(-distance, distance);

                if (!randomGap) {
                    plusX = 0;
                    plusY = 0;
                }

                Vector3 point = new Vector3(X + plusX, 30, Y + plusY);

                Ray ray = new Ray(point, Vector3.down);

                if (meshCollider.Raycast(ray, out rayHit, Mathf.Infinity)) {

                    int modelId = UnityEngine.Random.Range(1, modelsCount);

                    string type = modelType + modelId.ToString();

                    GameObject model = null;

                    try {
                        model = GameObject.Instantiate(Resources.Load(type)) as GameObject;
                    } catch {
                        Debug.Log("Failed to instatiate " + go.name + " " + type);
                        //Model is null so just jump to the next loop iteration
                        continue;
                    }

                    model.transform.position = new Vector3(X + plusX, go.transform.position.y + 0.3f, Y + plusY);
                    model.name = go.name + type;
                    model.layer = parentLayer;

                    model.transform.parent = go.transform;

                    // if model collides with other objects eg.buildings, delete model

                    hits = Physics.RaycastAll(ray, 50).ToList();

                    // the first it is somehow always hits "Plane" object, so Plane hits from List
                    hits = hits.Where(a => a.collider.gameObject.name != "Plane").ToList();

                    int index = 100;

                    for (int i = 0; i < hits.Count; i++) {
                        rayHit2 = hits[i];

                        if (rayHit2.collider.gameObject.layer == meshCollider.gameObject.layer) {
                            index = i;
                        }
                    }

                    //if the gameObject attached to the first collider hit by the ray is not the same layer as we want, delete model

                    if (index != 0) DestroyImmediate(model);
                }
            }
        }
    }

    /// <summary>
    /// Read data: parse the data into Nodes, Ways, Relations, further parse these into lists based on category
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="aBlockCount"></param>
    /// <param name="axBound"></param>
    /// <param name="ayBound"></param>
    public void ReadData(List<Node> nodes, int aBlockCount, double axBound, double ayBound) {

        GameObject waysParent = new GameObject("[WAYS]");
        GameObject BuildsParent = new GameObject("[BUILDINGS]");
        GameObject block = new GameObject("[BLOCK_" + aBlockCount + "]");
        waysParent.transform.parent = block.transform;
        BuildsParent.transform.parent = block.transform;

        XmlNodeList nodeList = doc.GetElementsByTagName("node");

        ParseNodes(nodeList);

        XmlNodeList wayList = doc.GetElementsByTagName("way");

        ParseWays(wayList, waysParent, nodes, axBound, ayBound, block, BuildsParent);

        XmlNodeList relationList = doc.GetElementsByTagName("relation");

        ParseRelations(relationList);

        // helper 
        DestroyImmediate(waysParent);
    }

    /// <summary>
    /// Parse Relations
    /// </summary>
    /// <param name="relationList"></param>
    private void ParseRelations(XmlNodeList relationList) {
        InfraType type;

        // START OF Relations
        foreach (XmlNode node in relationList) {
            // innertype = holes
            XmlNodeList relationNodes = node.ChildNodes;

            int tmpRelationId = int.Parse(node.Attributes["id"].InnerText);

            Relation tmpRelation = waterrelations.Find(x => x.Id == tmpRelationId);

            bool updateRelation = true;

            if (null != tmpRelation) {
                tmpRelation.Updated = true;
            }
            else if (null == tmpRelation) {
                updateRelation = false;
                tmpRelation = new Relation(tmpRelationId);
            }

            type = InfraType.ENone;

            // PARSE TAGS
            foreach (XmlNode nd in relationNodes) {

                string attrZeroName = nd.Attributes[0].Name;
                string attrZeroInnerText = nd.Attributes[0].InnerText;
                string attrOneName = nd.Attributes.Count > 1 ? nd.Attributes[1].Name : "";
                string attrOneInnerText = nd.Attributes.Count > 1 ? nd.Attributes[1].InnerText : "";

                // parsing of waters 
                if (attrZeroName == "k" && attrZeroInnerText == "waterway") {
                    if (attrOneName == "v" && (attrOneInnerText == "riverbank" || attrOneInnerText == "river")) {
                        type = InfraType.EWater;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "water") {
                        type = InfraType.EWater;
                    }
                }

                // parse green1
                if (attrZeroName == "k" && attrZeroInnerText == "landuse") {
                    if (attrOneName == "v" && attrOneInnerText == "forest") {
                        type = InfraType.EGreen1;
                        tmpRelation.CategoryType = (int)CategoryType.EForest;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "cemetery") {
                        type = InfraType.EGreen1;
                        tmpRelation.CategoryType = (int)CategoryType.ECemetery;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "wood") {
                        type = InfraType.EGreen1;
                        tmpRelation.CategoryType = (int)CategoryType.EForest;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "leisure") {
                    if (attrOneName == "v" && attrOneInnerText == "pitch") {
                        type = InfraType.EGreen1;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "amenity") {
                    if (attrOneName == "v" && attrOneInnerText == "grave_yard") {
                        type = InfraType.EGreen1;
                        tmpRelation.CategoryType = (int)CategoryType.ECemetery;
                    }
                }

                // parse green2
                if (attrZeroName == "k" && attrZeroInnerText == "leisure") {
                    if (attrOneName == "v" && attrOneInnerText == "park") {
                        type = InfraType.EGreen2;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "playground") {
                        type = InfraType.EGreen2;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "tourism") {
                    if (attrOneName == "v" && attrOneInnerText == "camp_site") {
                        type = InfraType.EGreen2;
                    }
                }

                // parse green3
                if (attrZeroName == "k" && attrZeroInnerText == "landuse") {
                    if (attrOneName == "v" && attrOneInnerText == "grass") {
                        type = InfraType.EGreen3;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "meadow") {
                        type = InfraType.EGreen3;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "scrub") {
                        type = InfraType.EGreen3;
                        tmpRelation.CategoryType = (int)CategoryType.EScrub;
                    }
                }

                // parse brown
                if (attrZeroName == "k" && attrZeroInnerText == "landuse") {
                    if (attrOneName == "v" && attrOneInnerText == "farmyard") {
                        type = InfraType.EBrown;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "farmland") {
                        type = InfraType.EBrown;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "allotments") {
                        type = InfraType.EBrown;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "orchard") {
                        type = InfraType.EBrown;
                        tmpRelation.CategoryType = (int)CategoryType.EOrchard;
                    }
                }

                // WAYS of the relation
                if (attrZeroName == "type" && attrZeroInnerText == "way") {
                    if (attrOneName == "ref") {
                        int refValue = int.Parse(attrOneInnerText);

                        if (nd.Attributes[2].Name == "role" && nd.Attributes[2].InnerText == "outer") {
                            long refAttrTextLong = long.Parse(nd.Attributes["ref"].InnerText);
                            if (null == tmpRelation.OuterrwayList.Find(x => x.WayId == refAttrTextLong)) {
                                tmpRelation.OuterrwayList.Add(new WayList(refAttrTextLong));
                            }
                        }
                        else if (nd.Attributes[2].Name == "role" && nd.Attributes[2].InnerText == "inner") {
                            long refAttrTextLong = long.Parse(nd.Attributes["ref"].InnerText);
                            if (null == tmpRelation.InnerwayList.Find(x => x.WayId == refAttrTextLong)) {
                                tmpRelation.InnerwayList.Add(new WayList(refAttrTextLong));
                            }
                        }
                    }
                }
            }

            // Parse relation Inner Ways == "holes"
            ParseRelationInner(tmpRelation, type);

            // Parse relation Outer Ways == "outline
            ParseRelationOuter(tmpRelation, type);

            if (updateRelation == false) {
                switch (type) {
                    case InfraType.EWater:
                        waterrelations.Add(tmpRelation);
                        break;
                    case InfraType.EWetland:
                        wetlandrelations.Add(tmpRelation);
                        break;
                    case InfraType.EGreen1:
                        green1relations.Add(tmpRelation);
                        break;
                    case InfraType.EGreen2:
                        green2relations.Add(tmpRelation);
                        break;
                    case InfraType.EGreen3:
                        green3relations.Add(tmpRelation);
                        break;
                    case InfraType.EBrown:
                        brownrelations.Add(tmpRelation);
                        break;
                    default:
                        break;
                }
            } else {
                //relations
            }

        } // END OF RELATIONS
    }

    /// <summary>
    /// Parse the Outer Ways of a relation
    /// </summary>
    /// <param name="tmpRelation">The relation</param>
    /// <param name="type">The relations infratype</param>
    private void ParseRelationOuter(Relation tmpRelation, InfraType type) {
        for (int io = 0; io < tmpRelation.OuterrwayList.Count; io++) {
            Way tmpWayList = allways.Find(x => x.Id == tmpRelation.OuterrwayList[io].WayId);
            if (tmpWayList != null && tmpRelation.OuterrwayList[io].Filled == false && tmpRelation.OuterVectors.Count == 0) {
                tmpRelation.OuterrwayList[io].Filled = true;
                foreach (long nodRef in tmpWayList.WayNodeIds) {
                    Node wayNode = allnodes.Find(x => x.Id == nodRef);
                    if (wayNode != null) {
                        x = wayNode.Latitude;
                        y = wayNode.Longitude;

                        float z = 0;

                        switch (type) {
                            case InfraType.EWater:
                                z = 0.01f;
                                break;
                            case InfraType.EWetland:
                                z = 0.02f;
                                break;
                        }
                        tmpRelation.OuterVectors.Add(new Vector3((float)(y - UTME_Zero), z, (float)(x - UTMN_Zero)));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Parse the Inner Ways of a relation
    /// </summary>
    /// <param name="tmpRelation">The relation</param>
    /// <param name="type">The infratype of the relation</param>
    private void ParseRelationInner(Relation tmpRelation, InfraType type) {
        for (int io = 0; io < tmpRelation.InnerwayList.Count; io++) {
            Way tmpWayList = allways.Find(x => x.Id == tmpRelation.InnerwayList[io].WayId);

            InnerVectorList listVectortmp = new InnerVectorList();

            if (tmpWayList != null && !tmpRelation.InnerwayList[io].Filled) {
                tmpRelation.InnerwayList[io].Filled = true;
                foreach (long nodRef in tmpWayList.WayNodeIds) {
                    Node wayNode = allnodes.Find(x => x.Id == nodRef);
                    if (wayNode != null) {
                        x = wayNode.Latitude;
                        y = wayNode.Longitude;

                        float z = 0;

                        switch (type) {
                            case InfraType.EWater:
                                z = 0.01f;
                                break;
                            case InfraType.EWetland:
                                z = 0.02f;
                                break;
                        }

                        listVectortmp.InnerVectors.Add(new Vector3((float)(y - UTME_Zero), z, (float)(x - UTMN_Zero)));
                    }
                }

                tmpRelation.InnerVectorLists.Add(listVectortmp);
            }
        }
    }

    /// <summary>
    /// Parse Ways, GENERATE invoke generation of buildings and roads for them
    /// </summary>
    /// <param name="wayList"></param>
    /// <param name="waysParent"></param>
    /// <param name="nodes"></param>
    /// <param name="axBound"></param>
    /// <param name="ayBound"></param>
    /// <param name="block"></param>
    /// <param name="buildsParent"></param>
    private void ParseWays(XmlNodeList wayList, GameObject waysParent, List<Node> nodes, double axBound, double ayBound, GameObject block, GameObject buildsParent) {
        List<Transform> roadsObjects = new List<Transform>();
        List<Transform> railwaysObjects = new List<Transform>();
        List<Transform> streamObjects = new List<Transform>();
        List<Way> roads = new List<Way>();
        List<Way> railways = new List<Way>();
        List<Way> streams = new List<Way>();
        List<Way> buildings = new List<Way>();
        List<Way> general = new List<Way>();

        List<Transform> buildingsObjects = new List<Transform>();
        InfraType type;

        // START OF WAYS != Road
        foreach (XmlNode node in wayList) {
            type = InfraType.ENone;
            XmlNodeList wayNodes = node.ChildNodes;

            Way tmpWay = new Way(int.Parse(node.Attributes["id"].InnerText));

            if (allways.Find(a => a.Id == tmpWay.Id) == null) allways.Add(tmpWay);

            // Parse Tags
            foreach (XmlNode nd in wayNodes) {
                if (nd.Attributes[0].Name == "ref") {
                    tmpWay.WayNodeIds.Add(long.Parse(nd.Attributes["ref"].InnerText));
                }

                string attrZeroName = nd.Attributes[0].Name;
                string attrZeroInnerText = nd.Attributes[0].InnerText;
                string attrOneName = nd.Attributes.Count > 1 ? nd.Attributes[1].Name : "";
                string attrOneInnerText = nd.Attributes.Count > 1 ? nd.Attributes[1].InnerText : "";

                // parse roads
                if (attrZeroName == "k" && attrZeroInnerText == "highway") {
                    // if the road narrow
                    if (attrOneName == "v" && (attrOneInnerText == "cycleway" || attrOneInnerText == "footway" || attrOneInnerText == "path" || attrOneInnerText == "pedestrian" || attrOneInnerText == "steps" || attrOneInnerText == "bridleway")) {
                        tmpWay.CategoryType = (int)CategoryType.ECycleWay;
                    }
                    // if the road is medium width
                    else if (attrOneName == "v" && (attrOneInnerText == "track" || attrOneInnerText == "secondary" || attrOneInnerText == "tertiary" || attrOneInnerText == "residential" || attrOneInnerText == "service" || attrOneInnerText == "unclassified")) {
                        tmpWay.CategoryType = (int)CategoryType.ERoad;
                    }
                    // if the road is wide
                    else if (attrOneName == "v" && (attrOneInnerText == "trunk" || attrOneInnerText == "primary" || attrOneInnerText == "trunk_link" || attrOneInnerText == "primary_link")) {
                        tmpWay.CategoryType = (int)CategoryType.EMainRoad;
                    }
                    type = InfraType.EHighway;
                }


                // parse buildings
                if (attrZeroName == "k" && attrZeroInnerText == "building") {
                    type = InfraType.EBuilding;
                }

                #region Green1 (forests, cemeteries, woods, pitches, graveyars)

                // parse green1
                if (attrZeroName == "k" && attrZeroInnerText == "landuse") {
                    if (attrOneName == "v" && attrOneInnerText == "forest") {
                        type = InfraType.EGreen1;
                        tmpWay.CategoryType = (int)CategoryType.EForest;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "cemetery") {
                        type = InfraType.EGreen1;
                        tmpWay.CategoryType = (int)CategoryType.ECemetery;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "wood") {
                        type = InfraType.EGreen1;
                        tmpWay.CategoryType = (int)CategoryType.EForest;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "leisure") {
                    if (attrOneName == "v" && attrOneInnerText == "pitch") {
                        type = InfraType.EGreen1;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "amenity") {
                    if (attrOneName == "v" && attrOneInnerText == "grave_yard") {
                        type = InfraType.EGreen1;
                        tmpWay.CategoryType = (int)CategoryType.ECemetery;
                    }
                }

                #endregion

                #region Green2 (parks, playgrounds, campsites)

                // parse green2
                if (attrZeroName == "k" && attrZeroInnerText == "leisure") {
                    if (attrOneName == "v" && attrOneInnerText == "park") {
                        type = InfraType.EGreen2;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "playground") {
                        type = InfraType.EGreen2;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "tourism") {
                    if (attrOneName == "v" && attrOneInnerText == "camp_site") {
                        type = InfraType.EGreen2;
                    }
                }

                #endregion

                #region Green3 (grasslands, meadows, scrubs)

                // parse green3
                if (attrZeroName == "k" && attrZeroInnerText == "landuse") {
                    if (attrOneName == "v" && (attrOneInnerText == "grass" || attrOneInnerText == "meadow")) {
                        type = InfraType.EGreen3;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "scrub") {
                        type = InfraType.EGreen3;
                        tmpWay.CategoryType = (int)CategoryType.EScrub;
                    }
                }

                #endregion

                #region Brown (farmyards, allotments, orchards)

                // parse brown
                if (attrZeroName == "k" && attrZeroInnerText == "landuse") {
                    if (attrOneName == "v" && attrOneInnerText == "farmyard") {
                        type = InfraType.EBrown;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "farmland") {
                        type = InfraType.EBrown;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "allotments") {
                        type = InfraType.EBrown;
                    }
                    else if (attrOneName == "v" && attrOneInnerText == "orchard") {
                        type = InfraType.EBrown;
                        tmpWay.CategoryType = (int)CategoryType.EOrchard;
                    }
                }

                #endregion

                #region Railways

                // parse railways
                if (attrZeroName == "k" && attrZeroInnerText == "railway") {
                    if (attrOneName == "v" && attrOneInnerText == "rail") {
                        type = InfraType.ERailWay;
                    }
                }

                #endregion

                #region Parking lots

                // parse parking lots
                if (attrZeroName == "k" && attrZeroInnerText == "amenity") {
                    if (attrOneName == "v" && attrOneInnerText == "parking") {
                        type = InfraType.EParkingLot;
                    }
                }

                #endregion

                #region Waters

                // parse waters
                if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "water") {
                        type = InfraType.EWater;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "waterway") {
                    if (attrOneName == "v" && attrOneInnerText == "riverbank") {
                        type = InfraType.EWater;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "leisure") {
                    if (attrOneName == "v" && attrOneInnerText == "swimming_pool") {
                        type = InfraType.EWater;
                    }
                }
                if (attrZeroName == "k" && attrZeroInnerText == "waterway") {
                    if (attrOneName == "v" && attrOneInnerText == "stream") {
                        type = InfraType.EStream;
                    }
                }
                #endregion

                #region Wetlands

                // parse wetlands
                if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "wetland") {
                        type = InfraType.EWetland;
                    }
                }

                #endregion

            }

            // Add way to respective lists based on (infra)type, do some additional things in some cases
            switch (type) {
                case InfraType.EHighway:
                    roads.Add(tmpWay);
                    break;
                case InfraType.EBuilding:

                    // if its a building look through the XML nodes again for building info
                    FindBuildingInfo(tmpWay, wayNodes);

                    buildings.Add(tmpWay);
                    break;
                case InfraType.ENone:
                    general.Add(tmpWay);
                    break;
                case InfraType.EGreen1:
                    green1ways.Add(tmpWay);
                    break;
                case InfraType.EGreen2:
                    green2ways.Add(tmpWay);
                    break;
                case InfraType.EGreen3:
                    green3ways.Add(tmpWay);
                    break;
                case InfraType.EBrown:
                    brownways.Add(tmpWay);
                    break;
                case InfraType.ERailWay:
                    railways.Add(tmpWay);
                    break;
                case InfraType.EParkingLot:
                    parkinglotWays.Add(tmpWay);
                    break;
                case InfraType.EWater:
                    waterways.Add(tmpWay);
                    break;
                case InfraType.EWetland:
                    wetlandways.Add(tmpWay);
                    break;
                case InfraType.EStream:
                    streams.Add(tmpWay);
                    break;
                default:
                    break;
            }
        } // END OF WAYS

        NavMesh.RemoveAllNavMeshData();
        GenerateRoads("road", roads, roadsObjects, waysParent, nodes, axBound, ayBound, block);
        GenerateRoads("railway", railways, railwaysObjects, waysParent, nodes, axBound, ayBound, block);
        GenerateRoads("stream", streams, streamObjects, waysParent, nodes, axBound, ayBound, block);

        GenerateBuildings(buildings, buildingsObjects, buildsParent, nodes);
    }

    /// <summary>
    /// Parse XML nodes again for building info for this way
    /// </summary>
    /// <param name="tmpWay">the way</param>
    /// <param name="wayNodes">XML nodes to parse</param>
    private void FindBuildingInfo(Way tmpWay, XmlNodeList wayNodes) {
        // string info = "";
        string name = "";

        foreach (XmlNode item in wayNodes) {
            //Info is disabled because they are in english and contain underscores e.g. "bus_station"
            // if (item.Attributes[0].Name == "k" && item.Attributes[0].InnerText == "amenity") {
            //     if (item.Attributes[1].Name == "v") {
            //         info = item.Attributes[1].InnerText;
            //     }
            // }
            if (item.Attributes[0].Name == "k" && item.Attributes[0].InnerText == "name") {
                if (item.Attributes[1].Name == "v") {
                    name = item.Attributes[1].InnerText;
                }
            }
        }

        // bool hasInfo = !string.IsNullOrWhiteSpace(info);
        bool hasName = !string.IsNullOrWhiteSpace(name);

        string buildingInfo = "";

        if (hasName) {
            buildingInfo = name;
        }

        if (!string.IsNullOrWhiteSpace(buildingInfo)) {
            buildingInfos.Add(new BuildingInfo(tmpWay.Id, buildingInfo));
        }
    }

    /// <summary>
    /// Generate Buildings for Block
    /// </summary>
    /// <param name="buildings"></param>
    /// <param name="buildingsObjects"></param>
    /// <param name="BuildsParent"></param>
    /// <param name="nodes"></param>
    private void GenerateBuildings(List<Way> buildings, List<Transform> buildingsObjects, GameObject BuildsParent, List<Node> nodes) {
        // Start of Buildings
        for (int i = 0; i < buildings.Count; i++) {

            GameObject buildingObject = new GameObject("BuildingObject" + buildings[i].Id);
            buildingsObjects.Add(buildingObject.transform);
            buildingObject.transform.parent = BuildsParent.transform;
            int buildCount = buildings[i].WayNodeIds.Count;

            List<Vector2> vectors = new List<Vector2>();
            List<Vector3> vectorsWall = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int j = 0; j < buildCount; j++) {
                foreach (Node nod in nodes) {
                    if (nod.Id == buildings[i].WayNodeIds[j]) {
                        x = nod.Latitude;
                        y = nod.Longitude;
                    }
                }
                vectorsWall.Add(new Vector3((float)(y - UTME_Zero), 0, (float)(x - UTMN_Zero)));
                vectorsWall.Add(new Vector3((float)(y - UTME_Zero), 4f, (float)(x - UTMN_Zero)));
                if (j < (buildCount - 1)) {
                    vectors.Add(new Vector2((float)(y - UTME_Zero), (float)(x - UTMN_Zero)));
                }
            }

            Vector2[] vertices2D = vectors.ToArray();
            bool clockWise = IsClockwise(vectors);

            Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();


            List<Vector3> testList = new List<Vector3>() ;
            for (int ii = 0; ii < vertices2D.Length; ii++) {
                testList.Add ( new Vector3(vertices2D[ii].x, 4f, vertices2D[ii].y));
            }

            poly.outside = testList;



            string name = "wall" + i.ToString();
            GameObject wall = new GameObject(name);
            int uvCount = ((vectorsWall.Count - 2) / 2);

            if (clockWise) {
                for (int bi = 0; bi < uvCount; bi++) {
                    triangles.Add(1 + (bi * 2));
                    triangles.Add(0 + (bi * 2));
                    triangles.Add(2 + (bi * 2));
                    triangles.Add(1 + (bi * 2));
                    triangles.Add(2 + (bi * 2));
                    triangles.Add(3 + (bi * 2));
                }
            } else {
                for (int bi = 0; bi < uvCount; bi++) {
                    triangles.Add(0 + (bi * 2));
                    triangles.Add(1 + (bi * 2));
                    triangles.Add(2 + (bi * 2));
                    triangles.Add(3 + (bi * 2));
                    triangles.Add(2 + (bi * 2));
                    triangles.Add(1 + (bi * 2));
                }
            }

            wall.transform.parent = buildingObject.transform;
            wall.AddComponent<MeshFilter>();
            wall.AddComponent<MeshRenderer>();
            Mesh mshWall = new Mesh();
            mshWall.vertices = vectorsWall.ToArray();
            mshWall.triangles = triangles.ToArray();
            mshWall.RecalculateNormals();
            mshWall.RecalculateBounds();
            wall.GetComponent<MeshFilter>().sharedMesh = mshWall;
            wall.GetComponent<MeshRenderer>().sharedMaterial = wallMaterial;
            wall.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
            wall.layer = buildingLayer;

            poly.CalcPlaneNormal(Vector3.up);
            Poly2Mesh.CreateGameObject(poly, buildingObject);
            buildingObject.AddComponent<BoxCollider>();

            //What layer should be used here, buildingslayer maybe?
            buildingObject.layer = buildingLayer;
            buildingObject.GetComponent<MeshRenderer>().sharedMaterial = roofMaterial;
            // if there is Building Info with this buildings ID create GameObject with TextMesh over the building for it
            BuildingInfo info = buildingInfos.Find(o => o.WayId == buildings[i].Id);

            if (null != info) {
                CreateBuldingInfoObject(info, buildingObject.transform);
            }
        }
        // End of Buildings
    }

    /// <summary>
    /// Create GameObject for BuildingInfo
    /// </summary>
    /// <param name="info">The BuildingInfo object</param>
    /// <param name="buildingsObject">The building that the info belongs to</param>
    private void CreateBuldingInfoObject(BuildingInfo info, Transform buildingsObject) {
        GameObject infoObject = new GameObject();

        TextMesh text = infoObject.AddComponent<TextMesh>();
        infoObject.AddComponent<pLab_BuildingInfo>();

        text.text = info.Info;

        text.color = buildingInfoTextColor;
        text.fontSize = 30;

        infoObject.transform.position = buildingsObject.gameObject.GetComponent<BoxCollider>().bounds.center + new Vector3(0, 10, 0);

        infoObject.transform.parent = buildingsObject.gameObject.transform;
        infoObject.name = buildingsObject.gameObject.name + "_info";
        infoObject.layer = UILayer;

        buildingInfoObjects.Add(infoObject);

        Camera cam = Camera.main;

        if (cam != null && cam.transform != null) {
            infoObject.transform.LookAt(cam.transform);

            infoObject.transform.rotation = Quaternion.LookRotation(infoObject.transform.position - cam.transform.position);
        }
    }

    /// <summary>
    /// Generate roads for the block
    /// </summary>
    /// <param name="roads"></param>
    /// <param name="roadsObjects"></param>
    /// <param name="waysParent"></param>
    /// <param name="nodes"></param>
    /// <param name="axBound"></param>
    /// <param name="ayBound"></param>
    /// <param name="block"></param>
    private void GenerateRoads(string category, List<Way> roads, List<Transform> roadsObjects, GameObject waysParent, List<Node> nodes, double axBound, double ayBound, GameObject block) {
        // NavMesh.RemoveAllNavMeshData();
        // Start of Roads
        for (int i = 0; i < roads.Count; i++) {
            if (roads[i].WayNodeIds.Count <= 1) continue;

            GameObject roadObject = new GameObject(category + "Object" + roads[i].Id); 
            roadObject.transform.parent = waysParent.transform;
            LineRenderer lRender = roadObject.AddComponent<LineRenderer>();
            MeshFilter mFilter = roadObject.AddComponent<MeshFilter>();
            MeshRenderer mRender = roadObject.AddComponent<MeshRenderer>();
            roadsObjects.Add(roadObject.transform);

            float width = 4f;

            switch (category) {
                case "road":
                    if (roads[i].CategoryType == (int)CategoryType.ECycleWay) {
                        width = 2f;
                    }
                    if (roads[i].CategoryType == (int)CategoryType.EMainRoad) {
                        width = 6f;
                    }
                    break;
                case "railway":
                    width = 3f;
                    break;
                case "stream":
                    width = 2f;
                    break;
            }

            // lRender.SetWidth(width, width);
            lRender.startWidth = width;
            lRender.endWidth = width;
            // lRender.SetVertexCount(roads[i].WayNodeIds.Count);
            lRender.positionCount = roads[i].WayNodeIds.Count;

            for (int j = 0; j < roads[i].WayNodeIds.Count; j++) {
                Node nod = nodes.Find(n => n.Id == roads[i].WayNodeIds[j]);
                if (nod == null) continue;

                x = nod.Latitude;
                y = nod.Longitude;
                
                lRender.SetPosition(j, new Vector3((float)(y - UTME_Zero), 0, (float)(x - UTMN_Zero)));

            }
            mFilter.sharedMesh = new Mesh();
            lRender.BakeMesh(mFilter.sharedMesh, true);
            lRender.enabled = false;
            mRender.sharedMaterial = roofMaterial;
        }

        if (roadsObjects.Count <= 1) {
            foreach(Transform roadTransform in roadsObjects) {
                DestroyImmediate(roadTransform.gameObject);
            }
            return;
        };

        CombineInstance[] combine = new CombineInstance[roadsObjects.Count];

        int iaa = 0;

        foreach (Transform r in roadsObjects) {
            combine[iaa].mesh = r.gameObject.GetComponent<MeshFilter>().sharedMesh;
            combine[iaa].transform = r.localToWorldMatrix;
            // combine[iaa].transform = r.gameObject.GetComponent<MeshFilter>().transform.localToWorldMatrix;

            iaa++;
        }

        GameObject combineGO = new GameObject("combine " + category);
        MeshFilter goMeshFilter = combineGO.AddComponent<MeshFilter>();
        combineGO.AddComponent<MeshRenderer>();
        goMeshFilter.sharedMesh = new Mesh();
        goMeshFilter.sharedMesh.CombineMeshes(combine);

        /*
        This will make the combineGo act as the road.
        If you want to use this,make sure you remove the DestroyImmediate(combineGo) which is at the end of this function
        combineGO.GetComponent<MeshRenderer>().sharedMaterial = roadMaterial;

        List<Vector3> goMeshVerts = new List<Vector3>();
        goMeshFilter.sharedMesh.GetVertices(goMeshVerts);
        Vector2[] gouvs = new Vector2[goMeshVerts.Count];

        for (int uv = 0; uv < goMeshVerts.Count; uv++) {
            gouvs[uv] = new Vector2(goMeshVerts[uv].x, goMeshVerts[uv].z);
        }

        goMeshFilter.sharedMesh.uv = gouvs;
        goMeshFilter.sharedMesh.RecalculateNormals();
        goMeshFilter.sharedMesh.RecalculateBounds();
        */


        combineGO.isStatic = true;
        BuildNavMesh(combineGO.transform, axBound, ayBound);

        NavMeshTriangulation navMesh = NavMesh.CalculateTriangulation();
        Vector3[] verticesa = navMesh.vertices;

        Vector2[] uvs = new Vector2[verticesa.Length];

        for (int uv = 0; uv < verticesa.Length; uv++) {
            uvs[uv] = new Vector2(verticesa[uv].x, verticesa[uv].z);
        }
        int[] polygons = navMesh.indices;


        Mesh roadCombineMesh = new Mesh();

        if (verticesa.Length > Int16.MaxValue) {
            Debug.LogWarning("IndexFormat for combined road mesh was changed from UInt16 to UInt32. This will take more memory, and may not work on some platforms such as mobile phones!");
            roadCombineMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }

        roadCombineMesh.vertices = verticesa;
        roadCombineMesh.triangles = polygons;
        roadCombineMesh.uv = uvs;
        roadCombineMesh.RecalculateNormals();

        roadCombineMesh.RecalculateBounds();

        GameObject roadCombineGameObject = new GameObject("roadCombineGameObject");
        MeshRenderer meshRenderer = roadCombineGameObject.AddComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        MeshFilter meshFilter = roadCombineGameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = roadCombineMesh;

        switch (category) {
            case "road":
                meshRenderer.sharedMaterial = roadMaterial;
                roadCombineGameObject.layer = roadLayer;
                break;
            case "railway":
                meshRenderer.sharedMaterial = railwayMaterial;
                roadCombineGameObject.layer = railwayLayer;
                break;
            case "stream":
                meshRenderer.sharedMaterial = waterMaterial;
                roadCombineGameObject.layer = waterLayer;
                break;
        }

        roadCombineGameObject.transform.parent = block.transform;
        //Move the road a bit higher so it will on top
        roadCombineGameObject.transform.Translate(0, 0.03f, 0);

        DestroyImmediate(combineGO);

        for (int i = 0; i < roads.Count; i++) {
            roadsObjects[i].gameObject.SetActive(false);
        }
        NavMesh.RemoveAllNavMeshData();
        // End of Roads
    }

    /// <summary>
    /// Parse Nodes
    /// </summary>
    /// <param name="nodeList">List of XMLNodes containing node data</param>
    private void ParseNodes(XmlNodeList nodeList) {
        foreach (XmlNode item in nodeList) {
            long idNum = long.Parse(item.Attributes[0].InnerText);

            XmlNodeList tags = item.ChildNodes;

            foreach (XmlNode item2 in tags) {
                string attrZeroName = item2.Attributes[0].Name;
                string attrZeroInnerText = item2.Attributes[0].InnerText;
                string attrOneName = item2.Attributes[1].Name;
                string attrOneInnerText = item2.Attributes[0].InnerText;

                if (attrZeroName == "k" && attrZeroInnerText == "historic") {
                    if (attrOneName == "v" && attrOneInnerText == "memorial") {
                        PlaceObjectOnNode("tombstones3", idNum);
                    }
                }

                else if (attrZeroName == "k" && attrZeroInnerText == "natural") {
                    if (attrOneName == "v" && attrOneInnerText == "tree") {
                        int no = UnityEngine.Random.Range(1, 1);
                        PlaceObjectOnNode("tree" + no.ToString(), idNum);
                    }
                }

                else if (attrZeroName == "k" && attrZeroInnerText == "amenity") {
                    if (attrOneName == "v" && attrOneInnerText == "parking") {
                        PlaceObjectOnNode("parking", idNum);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Place a 3D object on a node
    /// </summary>
    /// <param name="modelType">Name of model</param>
    /// <param name="idNum">ID of node</param>
    private void PlaceObjectOnNode(string modelType, long idNum) {
        if (SingleObjectParent == null) {
            SingleObjectParent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            SingleObjectParent.name = "Single_Objects";
            SingleObjectParent.transform.position = new Vector3(0, 0, 0);
            SingleObjectParent.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (SingleObjectParent != null) {
            Node locNode = allnodes.Find(x => x.Id == idNum);
            if (locNode != null) {
                // create object
                GameObject model = null;

                float z = 0;

                if (modelType == "parking") {
                    model = new GameObject();

                    TextMesh text = model.AddComponent<TextMesh>();

                    text.text = "P";
                    text.fontSize = 30;
                    text.color = parkinglotTextColor;
                    z = 0.1f;

                    // model.transform.RotateAroundLocal(Vector3.right, (float)Math.PI / 2);
                    model.transform.Rotate(Vector3.right, (float) Math.PI / 2 * Mathf.Rad2Deg, Space.World);
                    model.layer = UILayer;
                } else {
                    model = GameObject.Instantiate(Resources.Load(modelType)) as GameObject;
                }

                model.transform.position = new Vector3((float)(locNode.Longitude - UTME_Zero), z, (float)(locNode.Latitude - UTMN_Zero));
                model.transform.parent = SingleObjectParent.transform;

                model.name = "SO_" + modelType;
            }
        }
    }

    /// <summary>
    /// Determine if members of a list of Vector2s are in a clockwise order
    /// </summary>
    /// <param name="aVertices">List of vectors to check</param>
    /// <returns></returns>
    private bool IsClockwise(List<Vector2> aVertices) {
        double sum = 0.0;
        for (int i = 0; i < aVertices.Count; i++) {
            Vector2 v1 = aVertices[i];
            Vector2 v2 = aVertices[(i + 1) % aVertices.Count];
            sum += (v2.x - v1.x) * (v2.y + v1.y);
        }
        return sum > 0.0;
    }

    /// <summary>
    /// Generate RoadMesh
    /// </summary>
    /// <param name="aSegments"></param>
    /// <returns></returns>
    public List<int> GenerateRoadMesh(int aSegments) {

        List<int> lineTri = new List<int>();
        var n = aSegments;

        for (int i = 0; i < n - 1; i++) {
        }
        return lineTri;
    }

    /// <summary>
    /// Build NavMesh
    /// </summary>
    /// <param name="xform"></param>
    /// <param name="aX"></param>
    /// <param name="aY"></param>
    private void BuildNavMesh(Transform xform, double aX, double aY) {
        navMeshDataInstance.Remove();

        Vector3 boundsSize = isGeneratingFromFile ? new Vector3(100000, 5, 100000): new Vector3(700, 5, 700);
        aX += boundsSize.x / 2;
        aY += boundsSize.z / 2;

        Vector3 boundsCenter = new Vector3((float) aY, boundsSize.y / 2, (float) aX);
        Bounds navMeshBounds = new Bounds(boundsCenter, boundsSize);

        List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(
            xform,
            includedLayerMask,
            NavMeshCollectGeometry.RenderMeshes,
            0,
            new List<NavMeshBuildMarkup>(),
            buildSources);

        NavMeshData navData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0),
            buildSources,
            navMeshBounds,
            Vector3.down,
            Quaternion.Euler(Vector3.up));

        navMeshDataInstance = NavMesh.AddNavMeshData(navData);
    }

    /// <summary>
    /// Clear lists so they won't stay in memory
    /// </summary>
    private void ClearLists() {

        allnodes = new List<Node>();

        allways = new List<Way>();

        waterrelations = new List<Relation>();

        wetlandrelations = new List<Relation>();

        green1relations = new List<Relation>();

        green2relations = new List<Relation>();

        green3relations = new List<Relation>();

        brownrelations = new List<Relation>();

        waterrelationsObjects = new List<Transform>();
        wetlandrelationsObjects = new List<Transform>();
        green1relationsObjects = new List<Transform>();
        green2relationsObjects = new List<Transform>();
        green3relationsObjects = new List<Transform>();
        brownrelationsObjects = new List<Transform>();
        green1waysObjects = new List<Transform>();
        green2waysObjects = new List<Transform>();
        green3waysObjects = new List<Transform>();
        brownwaysObjects = new List<Transform>();
        parkinglotwaysObjects = new List<Transform>();
        waterwayObjects = new List<Transform>();
        wetlandwayObjects = new List<Transform>();
        alreadyMovedObjects = new List<Transform>();

        green1ways = new List<Way>();
        green2ways = new List<Way>();
        green3ways = new List<Way>();
        brownways = new List<Way>();
        waterways = new List<Way>();
        wetlandways = new List<Way>();
        parkinglotWays = new List<Way>();

        waylists = new List<WayList>();
        buildingInfos = new List<BuildingInfo>();
        buildingInfoObjects = new List<GameObject>();
    }
}
