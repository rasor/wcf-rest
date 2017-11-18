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


## Refs 
- Postman: <https://www.getpostman.com/> or <https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop?hl=en>
- Swagger4WCF: <https://www.codeproject.com/Tips/1190441/How-to-generate-basic-swagger-yaml-description-for>

The End
