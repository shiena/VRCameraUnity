namespace PhotoCamera.Repository
{
    public interface IPhotographyDataStore
    {
        bool IsWritable();
        void Save(byte[] bytes);
    }
}