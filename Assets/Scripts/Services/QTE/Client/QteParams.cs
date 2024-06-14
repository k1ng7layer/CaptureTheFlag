namespace Services.QTE.Client
{
    public readonly struct QteParams
    {
        public readonly float ZoneStart;
        public readonly float ZoneWidth;

        public QteParams(float zoneStart, float zoneWidth)
        {
            ZoneStart = zoneStart;
            ZoneWidth = zoneWidth;
        }
    }
}