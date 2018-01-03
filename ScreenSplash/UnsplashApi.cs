using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSplash
{
    public class UnsplashApi
    {
        private IRestClient _client;
        private string _applicationId;
        private double _width;
        private double _height;

        public UnsplashApi(double width, double height)
        {
            _client = new RestClient("https://api.unsplash.com");
            _applicationId = ConfigurationManager.AppSettings.Get("applicationId");
            _width = width;
            _height = height;
        }
        
        public List<Photo> RandomPhotos()
        {
            var request = new RestRequest("/photos/random", Method.GET);
            request.AddParameter("w", _width);
            request.AddParameter("h", _height);
            request.AddParameter("count", "30");
            request.AddHeader("Authorization", "Client-ID " + _applicationId);
            request.AddHeader("Accept-Version", "v1");

            var response = _client.Execute<List<Photo>>(request);

            return response.Data;
        }
    }
}
