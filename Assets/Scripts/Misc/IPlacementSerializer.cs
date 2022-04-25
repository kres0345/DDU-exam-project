namespace FactoryGame.Misc
{
    public interface IPlacementSerializer
    {
        public string GetSerializedData();

        public void SetSerializedData(string serializedData);
    }
}
