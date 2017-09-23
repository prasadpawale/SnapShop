using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnapShop.Design.Common
{
    public enum FeatureType
    {
        TYPE_UNSPECIFIED, FACE_DETECTION, LANDMARK_DETECTION, LOGO_DETECTION, LABEL_DETECTION, TEXT_DETECTION, SAFE_SEARCH_DETECTION, IMAGE_PROPERTIES
    }
}
namespace SnapShop.Design.Common.Request
{
    public class Rootobject
    {
        public List<Request> requests { get; set; }
    }

    public class Request
    {
        public Image image { get; set; }
        public List<Feature> features { get; set; }
    }

    public class Image
    {
        public Source source { get; set; }
    }

    public class Source
    {
        public string imageUri { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
    }
    public class APIParameters
    {
        public string base64ImageString { get; set; }
    }
}

namespace SnapShop.Design.Common.Response
{

    public class Rootobject
    {
        public List<Response> responses { get; set; }
    }

    public class Response
    {
        public List<Labelannotation> labelAnnotations { get; set; }
    }

    public class Labelannotation
    {
        public string mid { get; set; }
        public string description { get; set; }
        public float score { get; set; }
    }
     
}


