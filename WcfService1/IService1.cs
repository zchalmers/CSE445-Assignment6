using System.ServiceModel;

namespace Assignment6
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string WebDownload(string url);

        [OperationContract]
        string WordFilter(string text);
    }
}
