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

The End
