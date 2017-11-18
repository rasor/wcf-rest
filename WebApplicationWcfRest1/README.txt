HowTo create a RESTFULL WCF Service

-------- TFS Changeset 1 ---------------------
1. Create an empty web:
- File - New - Project - ASP.NET Web Application - Empty
=> This gives you a project without Global.Ajax and startup files
2. Add Ajax-enabled WCF service
- Solution - RightClick project - Add - New Item (Ctrl-Sht-A) - WCF Service (Ajax-enabled)
=> This gives you a WCF service with webHttpBinding
