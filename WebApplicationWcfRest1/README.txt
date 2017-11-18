HowTo create a RESTFULL WCF Service - Visual Studio 2017.4.1

-------- TFS Changeset 1 - The templates ---------------------
1. Create an empty web:
- File - New - Project - ASP.NET Web Application - Empty
=> This gives you a project without Global.Ajax and startup files
2. Add Ajax-enabled WCF service
- Solution - RightClick project - Add - New Item (Ctrl-Shft-A) - WCF Service (Ajax-enabled) - You could call it "RestService1"
=> This gives you a WCF service with webHttpBinding and a ref to System.ServiceModel.Web

-------- TFS Changeset 2 - The first response ---------------------
3. Change from SOAP to REST response
- In RestService1.svc.cs add below [OperationContract]:
[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
- Set a breakpoint in DoWork() and debug project (F5)
- Open http://localhost:15563/RestService1.svc/DoWork in Chrome
=> Breakpoint is hit. Response: {"d":null}
- GET http://localhost:15563/RestService1.svc/DoWork in Postman
=> Response: {"d":null}

-------- TFS Changeset 3 - change response to <empty> for void function ---------------------
4. Change from Ajax to REST client
- In web.config <behavior>: Replace <enableWebScript /> with <webHttp />
- Debug project (F5)
- GET http://localhost:15563/RestService1.svc/DoWork in Postman
=> Response: <empty>

-------- TFS Changeset 4 - Add Sample Interface ---------------------
5. Add a sample interface
- Paste \models\Book.cs from https://www.codeproject.com/Tips/1190441/How-to-generate-basic-swagger-yaml-description-for
- Paste \interfaces\IBookService.cs from https://www.codeproject.com/Tips/1190441/How-to-generate-basic-swagger-yaml-description-for
- Notice the code in IBookService in this project is changed a bit to allow to POST and PUT a Book
- In RestService1.svc.cs inherit from interface:
	public class RestService1 : IBookService
- Click on LigthBulp - Implement Interface
6. Change contract. In web.config change 
	from <service contract="WebApplicationWcfRest1.RestService1
	to <service contract="WebApplicationWcfRest1.interfaces.IBookService
7. Implement GetBookById(). Add line:
	return new Book() {Id = 1, Name= "The incredible stamp" };
8. Debug project (F5)
- GET http://localhost:15563/RestService1.svc/Book/1 in Postman
=> Response: {"Id": 1, "Name": "The incredible stamp"}
- POST http://localhost:15563/RestService1.svc/Book in Postman - Set Body to "raw" and write "{"Id": 2, "Name": "The invincible stamp"}"
=> Notice - you receive the object as a Book in C#

-------- TFS Changeset 5 - Create Swagger Yaml ---------------------
9. Create Swagger.yaml - this is the wsdl for REST
- In Project Properties (Alt-Enter) - Build - Selt "XML Documentation file" - Clear the path
- Install https://www.nuget.org/packages/Swagger4WCF into the project containing the interfaces
- Build project
=> The yaml file is in \bin\WebApplicationWcfRest1.IBookService.yaml
10. Edit yaml file
- Replace host from localhost to localhost:15563 (or to the test- or prod server host)
- Replace basePath from  /IBookService to /RestService1.svc
- Replace all paths from e.g. "/GetBooksList:" to /Book: (as you wrote in UriTemplate)
- Group operations with same path together and delete the duplicate paths
- Those paths having path parameters e.g. "/{id}" change parameters from "in: query" to "in: path"
- Save the yaml file into \interfaces\ - update version number each time you send a new version to the client
11. Test the yaml file
- Goto http://editor.swagger.io/
- Replace left pane with the content of the yaml file
=> In top of right pane: The should be no errors

-------- TFS Changeset 6 - Move contracts to new library ---------------------
12. Swagger4WCF does not work well with Unity.WCF, so we move the contracts to a new library
- File - New - Project (Ctrl-Shft-N) - Class Library - Name: Contracts
- Drag'n'drop folder interfaces to Contracts
- Drag'n'drop folder models to Contracts
- Add Refs to project Contracts:
-- System.ServiceModel
-- System.ServiceModel.Web
-- System.Runtime.Serialization
13. Create Swagger.yaml - this is the wsdl for REST
- In Project Properties (Alt-Enter) - Build - Selt "XML Documentation file" - Clear the path
- Install https://www.nuget.org/packages/Swagger4WCF into the project containing the interfaces
- Build project
=> The yaml file is in \bin\WebApplicationWcfRest1.IBookService.yaml
14. Remove Swagger4WCF from project WebApplicationWcfRest1
- In project WebApplicationWcfRest1 add ref to project Contracts
- In packages.config remove line having Swagger4WCF
- Rebuild Solution

Ref: 
- Swagger4WCF: https://www.codeproject.com/Tips/1190441/How-to-generate-basic-swagger-yaml-description-for
- NuGet: https://www.nuget.org/packages/Swagger4WCF

The End