namespace TrackZero.Abstract
{
    internal interface IEntityReference
    {
        public string Type { get; set; }
        public object Id { get; }
    }
}
