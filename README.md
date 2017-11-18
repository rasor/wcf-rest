# wcf-rest

HowTo create a REST service from a WCF service including Swagger.yaml

If you have an existing service you create yet an endpoint so you can keep the exising the existing service.

In most cases the best thing is to create yet a WCF service using the same contract as your original WCF service. Just start from bullet 2 then.

## The templates

1. Create an empty web:
- File - New - Project - ASP.NET Web Application - Empty  
- => This gives you a project without Global.Ajax and startup files
2. Add Ajax-enabled WCF service
- Solution - RightClick project - Add - New Item (Ctrl-Shft-A) - WCF Service (Ajax-enabled) - You could call it "RestService1"  
- => This gives you a WCF service with webHttpBinding and a ref to System.ServiceModel.Web

## The first response

3. Change from SOAP to REST response
- In RestService1.svc.cs add below [OperationContract]:
[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
- Set a breakpoint in DoWork() and debug project (F5)
- Open http://localhost:15563/RestService1.svc/DoWork in Chrome
- => Breakpoint is hit. Response: {"d":null}
- GET http://localhost:15563/RestService1.svc/DoWork in Postman
- => Response: {"d":null}

The End
