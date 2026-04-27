
// NaturalHazardsController.cs - RESTful Web API Controller
// Author: Andrew Courter
// Course: CSE445 - Distributed Software Development
//
// Description: Implements the Natural Hazards elective service (RESTful).
//
// API Endpoint: GET api/NaturalHazards/GetHazards?lat={lat}&lng={lng}
//
// External APIs Used (free, no API key required):
//   - https://earthquake.usgs.gov/fdsnws/event/1/query
//   - https://volcanoes.usgs.gov/vsc/api/volcanoApi/volcanoesGVP


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assignment6.Controllers
{
    /// RESTful API controller for the Natural Hazards service.
    /// Provides geospatial natural hazard risk assessment by combining
    /// data from multiple USGS APIs.
   
    public class NaturalHazardsController : ApiController
    {
        
        // Configuration Constants
        

        // Search radius for earthquakes (in km) around the given coordinates
        private const double EARTHQUAKE_SEARCH_RADIUS_KM = 200;

        // Number of days to look back for recent earthquakes
        private const int EARTHQUAKE_LOOKBACK_DAYS = 365;

        // Maximum number of earthquake results to return
        private const int MAX_EARTHQUAKE_RESULTS = 10;

        // Maximum distance (in km) to search for volcanoes
        private const double VOLCANO_SEARCH_RADIUS_KM = 300;

       
        // Main API Endpoint
        

        /// <summary>
        /// Returns a natural hazard risk report for the given coordinates.
        ///
        /// REST Endpoint: GET api/NaturalHazards/GetHazards?lat=33.4255&lng=-111.9400
       
        /// <param name="lat">Latitude coordinate (-90 to 90)</param>
        /// <param name="lng">Longitude coordinate (-180 to 180)</param>
        /// <returns>A formatted natural hazard risk report string</returns>
        [HttpGet]
        [Route("api/NaturalHazards/GetHazards")]
        public IHttpActionResult GetHazards(double lat, double lng)
        {
            //  Input Validation 
            // Latitude must be between -90 (South Pole) and 90 (North Pole)
            if (lat < -90 || lat > 90)
            {
                return BadRequest("Error: Latitude must be between -90 and 90.");
            }

            // Longitude must be between -180 and 180
            if (lng < -180 || lng > 180)
            {
                return BadRequest("Error: Longitude must be between -180 and 180.");
            }

            // Build the hazard report by calling multiple APIs
            StringBuilder report = new StringBuilder();
            report.AppendLine("=== NATURAL HAZARD RISK REPORT ===");
            report.AppendLine(string.Format("Location: {0:F4}, {1:F4}", lat, lng));
            report.AppendLine(string.Format("Report Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            report.AppendLine();

            // Track hazard counts for overall risk calculation
            int earthquakeCount = 0;
            double maxMagnitude = 0;
            int volcanoCount = 0;

            //  Section 1: Earthquake Data from USGS 
            try
            {
                report.AppendLine("--- EARTHQUAKE ACTIVITY ---");
                report.AppendLine(string.Format("(Recent earthquakes within {0} km, past {1} days)",
                    EARTHQUAKE_SEARCH_RADIUS_KM, EARTHQUAKE_LOOKBACK_DAYS));
                report.AppendLine();

                // Construct the USGS Earthquake API URL
                // API docs: https://earthquake.usgs.gov/fdsnws/event/1/
                // Parameters:
                //   format=geojson: return data in GeoJSON format
                //   latitude/longitude: center of search
                //   maxradiuskm: search radius in kilometers
                //   starttime: how far back to search
                //   orderby=magnitude: sort by magnitude (largest first)
                //   limit: max number of results
                DateTime startDate = DateTime.Now.AddDays(-EARTHQUAKE_LOOKBACK_DAYS);
                string earthquakeUrl = string.Format(
                    CultureInfo.InvariantCulture,
                    "https://earthquake.usgs.gov/fdsnws/event/1/query?" +
                    "format=geojson&latitude={0}&longitude={1}" +
                    "&maxradiuskm={2}&starttime={3:yyyy-MM-dd}" +
                    "&orderby=magnitude&limit={4}",
                    lat, lng, EARTHQUAKE_SEARCH_RADIUS_KM,
                    startDate, MAX_EARTHQUAKE_RESULTS);

                // Download the earthquake data
                WebClient client = new WebClient();
                string earthquakeJson = client.DownloadString(earthquakeUrl);

                // Parse the GeoJSON response
                JObject earthquakeData = JObject.Parse(earthquakeJson);
                JArray features = (JArray)earthquakeData["features"];

                earthquakeCount = features.Count;

                if (earthquakeCount == 0)
                {
                    report.AppendLine("No significant earthquakes recorded in this area recently.");
                }
                else
                {
                    report.AppendLine(string.Format("Found {0} earthquake(s):", earthquakeCount));
                    report.AppendLine();

                    // Process each earthquake feature
                    foreach (JToken feature in features)
                    {
                        JToken props = feature["properties"];

                        // Extract earthquake properties from the GeoJSON
                        double magnitude = props["mag"] != null ? (double)props["mag"] : 0;
                        string place = props["place"] != null ? props["place"].ToString() : "Unknown location";
                        long timeMs = props["time"] != null ? (long)props["time"] : 0;

                        // Track the maximum magnitude for risk calculation
                        if (magnitude > maxMagnitude)
                        {
                            maxMagnitude = magnitude;
                        }

                        // Convert Unix timestamp (milliseconds) to readable date
                        DateTime quakeTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                            .AddMilliseconds(timeMs);

                        // Format the earthquake entry
                        report.AppendLine(string.Format(
                            "  Magnitude {0:F1} - {1} ({2:yyyy-MM-dd})",
                            magnitude, place, quakeTime));
                    }
                }
                report.AppendLine();
            }
            catch (Exception ex)
            {
                // If earthquake API call fails, note the error but continue
                
                report.AppendLine("  [Earthquake data unavailable: " + ex.Message + "]");
                report.AppendLine();
            }

            //  Section 2: Volcano Data from USGS 
            try
            {
                report.AppendLine("--- VOLCANIC ACTIVITY ---");
                report.AppendLine(string.Format("(Known volcanoes within {0} km)", VOLCANO_SEARCH_RADIUS_KM));
                report.AppendLine();

                // Call the USGS Volcano Hazards Program API
                
                string volcanoUrl =
                    "https://volcanoes.usgs.gov/vsc/api/volcanoApi/volcanoesGVP";

                WebClient volcClient = new WebClient();
                string volcanoJson = volcClient.DownloadString(volcanoUrl);

                // Parse the volcano JSON response
                JArray volcanoArray = JArray.Parse(volcanoJson);

                // Filter volcanoes by distance from the target coordinates
              
                List<string> nearbyVolcanoes = new List<string>();

                foreach (JToken volcano in volcanoArray)
                {
                    // Extract volcano coordinates
                    double vLat = volcano["latitude"] != null ? (double)volcano["latitude"] : 0;
                    double vLng = volcano["longitude"] != null ? (double)volcano["longitude"] : 0;

                    // Calculate distance using Haversine formula
                    double distance = HaversineDistance(lat, lng, vLat, vLng);

                    // Only include volcanoes within search radius
                    if (distance <= VOLCANO_SEARCH_RADIUS_KM)
                    {
                        string vName = volcano["vName"] != null ?
                            volcano["vName"].ToString() : "Unknown Volcano";
                        string vType = volcano["primaryVType"] != null ?
                            volcano["primaryVType"].ToString() : "Unknown type";
                        string vElevation = volcano["elevation"] != null ?
                            volcano["elevation"].ToString() : "N/A";

                        nearbyVolcanoes.Add(string.Format(
                            "  {0} ({1}) - Elevation: {2}m - Distance: {3:F0} km",
                            vName, vType, vElevation, distance));
                    }
                }

                volcanoCount = nearbyVolcanoes.Count;

                if (volcanoCount == 0)
                {
                    report.AppendLine("No known volcanoes found within search radius.");
                }
                else
                {
                    report.AppendLine(string.Format("Found {0} volcano(es):", volcanoCount));
                    report.AppendLine();

                    // Sort by distance (already formatted in string, sort alphabetically works too)
                    foreach (string v in nearbyVolcanoes)
                    {
                        report.AppendLine(v);
                    }
                }
                report.AppendLine();
            }
            catch (Exception ex)
            {
                // If volcano API fails, note error and continue with report
                report.AppendLine("  [Volcano data unavailable: " + ex.Message + "]");
                report.AppendLine();
            }

            // ---- Section 3: Overall Risk Assessment ----
            report.AppendLine("--- OVERALL RISK ASSESSMENT ---");

            // Calculate risk level based on earthquake and volcano data
           
            string riskLevel = CalculateRiskLevel(earthquakeCount, maxMagnitude, volcanoCount);

            report.AppendLine(string.Format("Earthquake Activity: {0} events (max magnitude: {1:F1})",
                earthquakeCount, maxMagnitude));
            report.AppendLine(string.Format("Nearby Volcanoes: {0}", volcanoCount));
            report.AppendLine(string.Format("Overall Natural Hazard Risk: {0}", riskLevel));
            report.AppendLine();
            report.AppendLine("=== END OF REPORT ===");

            return Ok(report.ToString());
        }

        
        // Helper Methods
        
        /// <param name="lat1">Latitude of point 1 (degrees)</param>
        /// <param name="lng1">Longitude of point 1 (degrees)</param>
        /// <param name="lat2">Latitude of point 2 (degrees)</param>
        /// <param name="lng2">Longitude of point 2 (degrees)</param>
        /// <returns>Distance in kilometers</returns>
        private double HaversineDistance(double lat1, double lng1, double lat2, double lng2)
        {
            const double EarthRadiusKm = 6371.0;

            // Convert degrees to radians
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLng = DegreesToRadians(lng2 - lng1);
            double rLat1 = DegreesToRadians(lat1);
            double rLat2 = DegreesToRadians(lat2);

            // Haversine formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(rLat1) * Math.Cos(rLat2) *
                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

      
        /// <param name="quakeCount">Number of recent earthquakes</param>
        /// <param name="maxMag">Maximum earthquake magnitude</param>
        /// <param name="volcCount">Number of nearby volcanoes</param>
        /// <returns>Risk level string</returns>
        private string CalculateRiskLevel(int quakeCount, double maxMag, int volcCount)
        {
            int riskScore = 0;

            // Earthquake risk scoring
            if (quakeCount > 0) riskScore += 1;      
            if (quakeCount > 5) riskScore += 1;       
            if (maxMag >= 3.0) riskScore += 1;       
            if (maxMag >= 5.0) riskScore += 2;        
            if (maxMag >= 7.0) riskScore += 2;        

            // Volcano risk scoring
            if (volcCount > 0) riskScore += 1;        
            if (volcCount > 3) riskScore += 1;         

            // Determine risk level from combined score
            if (riskScore >= 5) return "VERY HIGH";
            if (riskScore >= 3) return "HIGH";
            if (riskScore >= 1) return "MODERATE";
            return "LOW";
        }
    }
}
