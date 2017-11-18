HowTo create a RESTFULL WCF Service

-------- TFS Changeset 1 - The templates ---------------------
1. Create an empty web:
- File - New - Project - ASP.NET Web Application - Empty
=> This gives you a project without Global.Ajax and startup files
2. Add Ajax-enabled WCF service
- Solution - RightClick project - Add - New Item (Ctrl-Shft-A) - WCF Service (Ajax-enabled)
=> This gives you a WCF service with webHttpBinding and a ref to System.ServiceModel.Web

-------- TFS Changeset 2 - The first response ---------------------
2. In RestService1.svc.cs add below [OperationContract]:
[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
3. Set a breakpoint in DoWork() and debug project (F5)
4. Open http://localhost:15563/RestService1.svc/DoWork in Chrome
=> Breakpoint is hit. Response: {"d":null}
5. GET http://localhost:15563/RestService1.svc/DoWork in Postman
=> Response: {"d":null}

The End