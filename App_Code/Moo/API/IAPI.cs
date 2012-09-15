using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
namespace Moo.API
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IAPI”。
    [ServiceContract]
    public interface IAPI
    {
        [OperationContract]
        string Login(string userName, string password);
        [OperationContract]
        void AddTrustedUser(string sToken, int userID);
        [OperationContract]
        void RemoveTrustedUser(string sToken, int userID);
        [OperationContract]
        List<BriefUserInfo> ListTrustedUser(string sToken);
        [OperationContract]
        BriefUserInfo GetUserByName(string sToken, string userName);
        [OperationContract]
        int CreateProblem(string sToken, string name, string type, string content);
        [OperationContract]
        int CreateTranditionalTestCase(string sToken, int problemID, byte[] input, byte[] answer, int timeLimit, int memoryLimit, int score);
        [OperationContract]
        int CreateSpecialJudgedTestCase(string sToken, int problemID, int judgerID, byte[] input, byte[] answer, int timeLimit, int memoryLimit);
    }
    [DataContract]
    public class BriefUserInfo
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
