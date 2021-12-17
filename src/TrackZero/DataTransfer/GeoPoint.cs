namespace TrackZero.DataTransfer
{
    /// <summary>
    /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
    /// </summary>
    public class GeoPoint
    {
        /// <summary>
        /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
        /// </summary>
        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
        /// </summary>
        public double Longitude { get; set; }
    }
}
