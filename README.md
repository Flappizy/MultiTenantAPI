# Introduction
This is a simple multi tenant API that implements AUTH flow using JSON web Token, this API uses the 
AuthPermissions.AspNetCore library created by * [Jon P. Smith](https://github.com/JonPSmith/AuthPermissions.AspNetCore)

# ERD
The below image shows the Entity Relationship between the models that make up the Database Schema within the 
AuthPermissions.AspNetCore

<img src="images/ER Diagram.png" height="75" width="200"/>

# How to work with the code
When running the code, you have to first get authenticated, the below shows how to do that
 ```
    Steps:
    * Run the code and open the Swagger UI
    * Execute the Authentication Endpoint ("api/authenticate/authuser") the enpoint expects an object {email: "F1an@arsenal.com", password: "F1an@arsenal.com"}, use one of the autogenerated users [Email:F1an@arsenal.com Password: F1an@arsenal.com, Email:F1an@chelsea.com Password: F1an@chelsea.com], you can test with the other users, you can get them in the "PermissionCode/AppAuthSetupData.cs" folder 
    * The endpoint returns a JSON Web Token for the user, put this token in the Authorize box in swagger
    * Test the Tenant User endpoint ("api/TenantUser/usertenant") this endpoint returns the Tenant Name of the logged in user
```
