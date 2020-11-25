namespace PhotoCamera.Repository
{
    public interface IPhotographyDataStore
    {
        bool IsWritable();
        void Save(string ext, sbyte[] bytes);
    }
}