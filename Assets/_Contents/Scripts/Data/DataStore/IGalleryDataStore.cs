namespace PhotoCamera.DataStore
{
    public interface IGalleryDataStore
    {
        void SaveImage(string path, sbyte[] data);
    }
}