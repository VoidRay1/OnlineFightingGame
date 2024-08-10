public interface ISaveService
{
    void Save(string fileName, object data);
    T Load<T>(string fileName);
}