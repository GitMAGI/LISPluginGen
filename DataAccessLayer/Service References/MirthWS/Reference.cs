﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws.connectors.connect.mirth.com/", ConfigurationName="MirthWS.DefaultAcceptMessage")]
    public interface DefaultAcceptMessage {
        
        // CODEGEN: Generating message contract since element name arg0 from namespace  is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponse acceptMessage(Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponse> acceptMessageAsync(Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class acceptMessageRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="acceptMessage", Namespace="http://ws.connectors.connect.mirth.com/", Order=0)]
        public Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequestBody Body;
        
        public acceptMessageRequest() {
        }
        
        public acceptMessageRequest(Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class acceptMessageRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string arg0;
        
        public acceptMessageRequestBody() {
        }
        
        public acceptMessageRequestBody(string arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class acceptMessageResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="acceptMessageResponse", Namespace="http://ws.connectors.connect.mirth.com/", Order=0)]
        public Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponseBody Body;
        
        public acceptMessageResponse() {
        }
        
        public acceptMessageResponse(Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class acceptMessageResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public acceptMessageResponseBody() {
        }
        
        public acceptMessageResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DefaultAcceptMessageChannel : Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.DefaultAcceptMessage, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DefaultAcceptMessageClient : System.ServiceModel.ClientBase<Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.DefaultAcceptMessage>, Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.DefaultAcceptMessage {
        
        public DefaultAcceptMessageClient() {
        }
        
        public DefaultAcceptMessageClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DefaultAcceptMessageClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DefaultAcceptMessageClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DefaultAcceptMessageClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponse Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.DefaultAcceptMessage.acceptMessage(Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest request) {
            return base.Channel.acceptMessage(request);
        }
        
        public string acceptMessage(string arg0) {
            Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest inValue = new Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest();
            inValue.Body = new Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequestBody();
            inValue.Body.arg0 = arg0;
            Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponse retVal = ((Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.DefaultAcceptMessage)(this)).acceptMessage(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponse> Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.DefaultAcceptMessage.acceptMessageAsync(Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest request) {
            return base.Channel.acceptMessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageResponse> acceptMessageAsync(string arg0) {
            Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest inValue = new Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequest();
            inValue.Body = new Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.acceptMessageRequestBody();
            inValue.Body.arg0 = arg0;
            return ((Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer.MirthWS.DefaultAcceptMessage)(this)).acceptMessageAsync(inValue);
        }
    }
}
