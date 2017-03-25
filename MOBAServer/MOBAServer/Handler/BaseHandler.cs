using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.OpCode;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public abstract class BaseHandler
    {
        public OperationCode OpCode;

        public abstract void OnOperationRequest(OperationRequest request,
            SendParameters sendParameters, MobaClient peer);
    }
}
