namespace TrackZero.DataTransfer
{
    /// <summary>
    /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
    /// </summary>
    public class GeographyAutomaticReferencing
    {
        /// <summary>
        /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
        /// </summary>
        public GeographyAutomaticReferencing()
        {

        }

        /// <summary>
        /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
        /// </summary>
        public GeographyAutomaticReferencing(double latitude, double longitude)
        {
            this.GeoPoint = new GeoPoint(latitude, longitude);
        }

        /// <summary>
        /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
        /// </summary>
        public GeoPoint GeoPoint { get; set; }
    }
}
