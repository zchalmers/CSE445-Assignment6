using System.ServiceModel;
using System.ServiceModel.Web;

namespace WeatherService
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        [WebGet(
            UriTemplate = "Weather5day?zipcode={zipcode}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string[] Weather5day(string zipcode);
    }
}
