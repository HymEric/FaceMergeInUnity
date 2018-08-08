using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonParse
{
    public class FaceMerge
    {
        public string img_base64 { get; set; }
        public string img_base64_thumb { get; set; }
        public string img_url { get; set; }
        public string img_url_thumb { get; set; }
        public string ret { get; set; }
        public string msg { get; set; }
        public static FaceMerge ParseJsonFaceMerge(string json)
        {
            return LitJson.JsonMapper.ToObject<FaceMerge>(json);
        }
    }

    public class Multifaceidentify
    {
        public string session_id { get; set; }
        public Results[] results { get; set; }
        public int errorcode { get; set; }
        public string errormsg { get; set; }
        public int group_size { get; set; }
        public int time_ms { get; set; }
        public static Multifaceidentify ParseMultifaceidentify(string json)
        {
            return LitJson.JsonMapper.ToObject<Multifaceidentify>(json);
        }

        public class Results {
            public string[] candidates { get; set; }
            public Face_rect face_rect { get; set; }
            public int id { get; set; }
            public int errorcode { get; set; }

            public class Face_rect
            {
                public int x { get; set; }
                public int y { get; set; }
                public int width { get; set; }
                public int height { get; set; }
            }
        }
    }
}
