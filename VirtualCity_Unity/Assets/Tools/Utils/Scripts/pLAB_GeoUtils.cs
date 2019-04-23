/******************************************************************************
 * File         : pLAB_GeoUtils.cs            
 * Lisence      : BSD 3-Clause License
 * Copyright    : Lapland University of Applied Sciences
 * Authors      : Toni Westerlund (toni.westerlund@lapinamk.fi)
 * 
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// pLAB_GeoUtils
/// </summary>
public class pLAB_GeoUtils : MonoBehaviour {

    #region // Class Attributes

    #endregion

    #region // From Base Class


    #endregion

    #region // Private Functions
    #endregion

    #region // Protected Functions
    #endregion

    #region // Public Functions

    /// <summary>
    ///
    /// Convert lat/long to UTM coords.  Equations from USGS Bulletin 1532
    /// 
    /// East Longitudes are positive, West longitudes are negative.
    /// North latitudes are positive, South latitudes are negative
    /// at and Long are in fractional degrees
    /// 
    /// written by Chuck Gantz- chuck.gantz@globalstar.com
    /// 
    /// </summary>
    /// <param name="Lat"></param>
    /// <param name="Long"></param>
    /// <param name="UTMNorthing"></param>
    /// <param name="UTMEasting"></param>
    public static void LatLongtoUTM(double Lat, double Long, out double UTMNorthing, out double UTMEasting) {
        double a = 6378137; 
        double eccSquared = 0.00669438;
        double k0 = 0.9996;

        double LongOrigin;
        double eccPrimeSquared;
        double N, T, C, A, M;

        double LongTemp = (Long + 180) - ((int)((Long + 180) / 360)) * 360 - 180;

        double LatRad = Lat * Mathf.Deg2Rad;
        double LongRad = LongTemp * Mathf.Deg2Rad;
        double LongOriginRad;
        int ZoneNumber;
        ZoneNumber = ((int)((LongTemp + 180) / 6)) + 1;
        if (Lat >= 56.0 && Lat < 64.0 && LongTemp >= 3.0 && LongTemp < 12.0)
            ZoneNumber = 32;
        if (Lat >= 72.0 && Lat < 84.0)
        {
            if (LongTemp >= 0.0 && LongTemp < 9.0) ZoneNumber = 31;
            else if (LongTemp >= 9.0 && LongTemp < 21.0) ZoneNumber = 33;
            else if (LongTemp >= 21.0 && LongTemp < 33.0) ZoneNumber = 35;
            else if (LongTemp >= 33.0 && LongTemp < 42.0) ZoneNumber = 37;
        }
        LongOrigin = (ZoneNumber - 1) * 6 - 180 + 3;
        LongOriginRad = LongOrigin * Mathf.Deg2Rad;
        eccPrimeSquared = (eccSquared) / (1 - eccSquared);

        N = a / Math.Sqrt(1 - eccSquared * Math.Sin(LatRad) * Math.Sin(LatRad));
        T = Math.Tan(LatRad) * Math.Tan(LatRad);
        C = eccPrimeSquared * Math.Cos(LatRad) * Math.Cos(LatRad);
        A = Math.Cos(LatRad) * (LongRad - LongOriginRad);

        M = a * ((1 - eccSquared / 4 - 3 * eccSquared * eccSquared / 64 - 5 * eccSquared * eccSquared * eccSquared / 256) * LatRad
        - (3 * eccSquared / 8 + 3 * eccSquared * eccSquared / 32 + 45 * eccSquared * eccSquared * eccSquared / 1024) * Math.Sin(2 * LatRad)
        + (15 * eccSquared * eccSquared / 256 + 45 * eccSquared * eccSquared * eccSquared / 1024) * Math.Sin(4 * LatRad)
        - (35 * eccSquared * eccSquared * eccSquared / 3072) * Math.Sin(6 * LatRad));

        UTMEasting = (double)(k0 * N * (A + (1 - T + C) * A * A * A / 6
        + (5 - 18 * T + T * T + 72 * C - 58 * eccPrimeSquared) * A * A * A * A * A / 120)
        + 500000.0);

        UTMNorthing = (double)(k0 * (M + N * Math.Tan(LatRad) * (A * A / 2 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24
        + (61 - 58 * T + T * T + 600 * C - 330 * eccPrimeSquared) * A * A * A * A * A * A / 720)));
        if (Lat < 0)
            UTMNorthing += 10000000.0; 
    }
    #endregion
}
