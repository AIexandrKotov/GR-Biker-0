namespace Biker_0
{
    public class Road
    {
        public enum RoadQuality
        {
            Ground,
            Asphalt,
            Railroad,
        }

        public long idLeft, idRight;
        public RoadQuality Quality = RoadQuality.Ground;

        public Road() { }
        public Road(long left, long right)
        {
            idLeft = left;
            idRight = right;
        }
        public Road(long left, long right, RoadQuality quality)
        {
            idLeft = left;
            idRight = right;
            Quality = quality;
        }

        public Road Reverse()
        {
            return new Road(idRight, idLeft, Quality);
        }
    }
}
