using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
namespace Moo.API
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IAPI”。
    [ServiceContract]
    public interface IAPI
    {
        [OperationContract]
        string Login(string userName,string password);
        [OperationContract]
        void AddTrustedUser(string sToken, int userID);
        [OperationContract]
        void RemoveTrustedUser(string sToken, int userID);
        [OperationContract]
        int CreateProblem(string sToken, string name, string type, string content);
        [OperationContract]
        int CreateTranditionalTestCase(string sToken, int problemID, byte[] input, byte[] answer, int timeLimit, int memoryLimit, int score);
    }
}
