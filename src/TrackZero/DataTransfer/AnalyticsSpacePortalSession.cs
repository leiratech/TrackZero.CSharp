namespace TrackZero.DataTransfer
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalyticsSpacePortalSession
    {
        /// <summary>
        /// A session key used for authorization, there is no direct use of this key.
        /// </summary>
        public string SessionKey { get; set; }

        /// <summary>
        /// The URL to redirect the user to. This URL will allow the user to access the specific Analytics Space Session for the specified TTL in the request.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The URL to use in <IFrame /> tag if you plan on embedding TrackZero Dashboards page inside your web page. This URL is scoped to the specified  Analytics Space Session for the specified TTL in the request.
        /// </summary>
        public string EmbeddedDashboarsdUrl { get; set; }

        /// <summary>
        /// The URL to use in <IFrame /> tag if you plan on embedding TrackZero Reports page inside your web page. This URL is scoped to the specified  Analytics Space Session for the specified TTL in the request.
        /// </summary>
        public string EmbeddedReportsUrl { get; set; }
    }

}
