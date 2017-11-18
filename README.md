# wcf-rest

HowTo create a REST service from a WCF service including Swagger.yaml

If you have an existing service you create yet an endpoint so you can keep the exising the existing service.

In most cases the best thing is to create yet a WCF service using the same contract as your original WCF service. Just start from bullet 2 then.

## The templates

1. Create an empty web:
- File - New - Project - ASP.NET Web Application - Empty  
- => This gives you a project without Global.Ajax and startup files
2. Add Ajax-enabled WCF service
- Solution - RightClick project - Add - New Item (Ctrl-Shft-A) - WCF Service (Ajax-enabled) - You could call it `RestService1`  
- => This gives you a WCF service with webHttpBinding and a ref to System.ServiceModel.Web

## The first response

3. Change from SOAP to REST response
- In `RestService1.svc.cs` add below `[OperationContract]`:
```CSharp
[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
```
- Set a breakpoint in DoWork() and debug project (F5)
- Open <http://localhost:15563/RestService1.svc/DoWork> in Chrome
- => Breakpoint is hit. Response: {"d":null}
- GET <http://localhost:15563/RestService1.svc/DoWork> in Postman
- => Response: {"d":null}

## Change response to <empty> for void functions

4. Change from Ajax to REST client
- In web.config `<behavior>`: Replace `<enableWebScript />` with `<webHttp />`
- Debug project (F5)
- GET http://localhost:15563/RestService1.svc/DoWork in Postman
- => Response: `<empty>`

## Add Sample Interface

5. Add a sample interface
- Paste `\models\Book.cs` from <https://www.codeproject.com/Tips/1190441/How-to-generate-basic-swagger-yaml-description-for>
- Paste `\interfaces\IBookService.cs` from <https://www.codeproject.com/Tips/1190441/How-to-generate-basic-swagger-yaml-description-for>
- Notice the code in `IBookService` in this project is changed a bit to allow to POST and PUT a `Book` object using JSON in the body of the request.
- In `RestService1.svc.cs` inherit from interface:
```CSharp
	public class RestService1 : IBookService
```
- Click on LigthBulp - Implement Interface
6. Change contract. In `web.config` change 
	from `<service contract="WebApplicationWcfRest1.RestService1`  
	to `<service contract="WebApplicationWcfRest1.interfaces.IBookService`
7. Implement `GetBookById()`. Add line:
```CSharp
	return new Book() {Id = 1, Name= "The incredible stamp" };
```
8. Debug project (F5)
- GET <http://localhost:15563/RestService1.svc/Book/1> in Postman
- => Response: `{"Id": 1, "Name": "The incredible stamp"}`
- POST <http://localhost:15563/RestService1.svc/Book> in Postman - Set Body to `raw` and write `{"Id": 2, "Name": "The invincible stamp"}`
- => Notice - you receive the object as a `Book` in C#

## Create Swagger Yaml 

9. Create Swagger.yaml - this is the wsdl for REST
- In Project Properties (Alt-Enter) - Build - Select `XML Documentation file` - Clear the path
- Install <https://www.nuget.org/packages/Swagger4WCF> into the project containing the interfaces
- Build project
- => The yaml file is in `\bin\WebApplicationWcfRest1.IBookService.yaml`
10. Edit yaml file
- Replace `host` from `localhost` to `localhost:15563` (or to the test- or prod server host)
- Replace `basePath` from  `/IBookService` to `/RestService1.svc`
- Replace all `path`s from e.g. `/GetBooksList:` to `/Book:` (as you wrote in UriTemplate)
- Group operations with same `path` together and delete the duplicate paths
- Those paths having path parameters e.g. `/{id}` change parameters from `in: query` to `in: path`
- Save the yaml file into `\interfaces\` - update version number each time you send a new version to the client
11. Test the yaml file
- Goto <http://editor.swagger.io/> 
- Replace left pane with the content of the yaml file (if you use chrome, you can paste)
- => In top of right pane: The should be no errors

## Move contracts to new library 

12. Swagger4WCF does not work well with Unity.WCF, so we move the contracts to a new library
- File - New - Project (Ctrl-Shft-N) - Class Library - Name: Contracts
- Drag'n'drop folder interfaces to Contracts
- Drag'n'drop folder models to Contracts
- Add Refs to project Contracts:
  - `System.ServiceModel`
  - `System.ServiceModel.Web`
  - `System.Runtime.Serialization`
13. Create Swagger.yaml - this is the wsdl for REST
- In Project Properties (Alt-Enter) - Build - Selt "XML Documentation file" - Clear the path
- Install <https://www.nuget.org/packages/Swagger4WCF> into the project containing the interfaces (Contracts)
- Build project
- => The yaml file is in `\bin\WebApplicationWcfRest1.IBookService.yaml`
14. Remove `Swagger4WCF` from project `WebApplicationWcfRest1`
- In project `WebApplicationWcfRest1` add ref to project `Contracts`
- In `packages.config` remove line having `Swagger4WCF`
- Rebuild Solution

## Add dependency injection with Unity 
15. Remove `Swagger4WCF` from service project
- Unload project `WebApplicationWcfRest1`
  - Remove two lines containing `Swagger4WCF` near bottom
16. Add dependency injection
- Install <https://www.nuget.org/packages/Unity.Wcf> into the project containing the services (`WebApplicationWcfRest1`)
- => This created file `WcfServiceFactory.cs`
- View Markup of `RestService1.svc`
- Replace: `CodeBehind="RestService1.svc.cs"`
  - with: `Factory="WebApplicationWcfRest1.WcfServiceFactory"`
- In `WcfServiceFactory.cs` register the service:
```CSharp
               .RegisterType<IBookService, RestService1>();
```
- Build the solution
17. Debug project (F5)
- GET <http://localhost:15563/RestService1.svc/Book/1> in Postman
- => Response: `{"Id": 1, "Name": "The incredible stamp"}`

## Use HTTP Status Codes 

18. Add HTTP Status Codes to your service
- In your service `RestService1.svc.cs` - method `AddBook()` add content
```CSharp
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created; // 201
            if (book.Name == "The incredible stamp") {   // Book exist
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Conflict; // 409
            }
```
  - In method `UpdateBook()` add content
```CSharp
            if (book.Id == 2) { // Book does not exist - 404
                WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound("Resource not found");
            } else if (book.Name == "") { // Invalid request
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.MethodNotAllowed; // 405
            }
```
  - In method `GetBookById()` add content
```CSharp
            if (id == "2") { // Book does not exist - 404
                WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound("Resource not found");
                return null;
            } else {
                return new Book() { Id = 1, Name = "The incredible stamp" };
            }
```
- Follow the guidance for HTTP Status Codes in <https://developers.redhat.com/blog/2017/01/19/applying-api-best-practices-in-fuse/>
- => Test the change using Postman
19. Update your yaml with the Status Codes
- In `IBookService.yaml`
```yaml
    put:
      responses:
        201:
          description: "Book created"
        409:
          description: "Book exist"
    post:
      responses:
        404:
          description: "Book not found"
        405:
          description: "Validation exception"
    get:
      responses:
        404:
          description: "Book not found"
```

## Refs 
- Postman: <https://www.getpostman.com/> or <https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop?hl=en>
- Swagger4WCF: <https://www.codeproject.com/Tips/1190441/How-to-generate-basic-swagger-yaml-description-for>
- NuGet Swagger4WCF: <https://www.nuget.org/packages/Swagger4WCF>
- Unity.WCF: <https://www.devtrends.co.uk/blog/introducing-unity.wcf-providing-easy-ioc-integration-for-your-wcf-services>
- Set StatusCode: <https://codeblitz.wordpress.com/2009/04/27/how-to-host-and-consume-wcf-restful-services/>
- API Best Practices: <https://developers.redhat.com/blog/2017/01/19/applying-api-best-practices-in-fuse/#>

The End
