using ServiceLayer.Common;
namespace ServiceLayer.Helper
{
    public static class Request_Response
    {
        public static Request_Model Get_Response(string Status, string Message, int ErrorCode, object Body)
        {
            Request_Model request_Model = new Request_Model
            {
                Status = Status,
                Message = Message,
                Code = ErrorCode,
                Body = Body
            };

            return request_Model;
        }
    }
}
