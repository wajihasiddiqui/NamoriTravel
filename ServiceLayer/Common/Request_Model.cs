namespace ServiceLayer.Common
{
   public class Request_Model
    {
        public string Status { get; set; }

        public int Code { get; set; }

        public object Body { get; set; }

        public string Message { get; set; }
    }
}
