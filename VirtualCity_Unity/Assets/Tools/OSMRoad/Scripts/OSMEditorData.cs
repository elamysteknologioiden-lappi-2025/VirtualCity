/******************************************************************************
 * File         : OSMEditorData.cs            
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

public class OSMEditorData : ScriptableObject {

    [Header("Area Coordinates")]
    public double minLat;
    public double minLon;
    public double maxLat;
    public double maxLon;

    [Header("Materials")]
    public Material waterMaterial;
    public Material roadMaterial;
    public Material railwayMaterial;
    public Material wallMaterial;
    public Material roofMaterial;
    public Material green1Material;
    public Material green2Material;
    public Material green3Material;
    public Material brownMaterial;
    public Material wetlandMaterial;
    // public string osmDataFile;

    [Header("Layers")]

    [Layer]
    public int waterLayer = 4;

    [Layer]
    public int roadLayer = 0;

    [Layer]
    public int railwayLayer = 0;

    [Layer]
    public int buildingLayer = 0;

    [Layer]
    public int greenway1Layer = 0;

    [Layer]
    public int greenway2Layer = 0;

    [Layer]
    public int greenway3Layer = 0;

    [Layer]
    public int brownwayLayer = 0;

    [Layer]
    public int wetlandLayer = 0;

    [Header("Info Text Colors")]
    public Color32 buildingInfoTextColor = Color.red;
    public Color32 parkingLotTextColor = Color.blue;




}
