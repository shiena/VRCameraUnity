namespace PhotoCamera.DataStore
{
    public interface IGalleryDataStore
    {
        string StoragePath { get; }
        void RegisterImage(string path);
    }
}