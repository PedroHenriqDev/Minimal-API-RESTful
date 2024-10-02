# Minimal API and Anemic Domain Architecture with ASP.NET Core

### Project Overview
This project aims to demonstrate the feasibility of utilizing an **anemic domain** alongside a **Minimal API** for consuming data represented by that domain. The entire project ecosystem is built using **.NET**. 

### Key Features
It's important to note that while this API employs an anemic domain, it incorporates various resources such as:

- **CQRS Pattern**
- **Mediator Pattern**
- **Endpoint Separation** through Extension Methods
- **Filters** for Data Injection
- **Global Exception Handling Middleware**
- **Integration and Unit Testing Coverage**
- **AutoMapper**
- **Repository Pattern**
- **Fail-Fast Validation**
- **Docker** for Containerization
- **Versioning**
- **OpenAPI (Swagger)**
- **Response Compression**
- **Profile-Based Authentication and Authorization** with JWT tokens

### API Endpoints
Below are the API endpoints being consumed:

<hr/>

![Image](https://github.com/PedroHenriqDev/Minimal-API-RESTful/blob/master/doc/images/api-in-swagger.png)

<hr/>

![Image](https://github.com/PedroHenriqDev/Minimal-API-RESTful/blob/master/doc/images/api-in-swagger(1).png)

<hr/>

![Image](https://github.com/PedroHenriqDev/Minimal-API-RESTful/blob/master/doc/images/api-in-swagger-stats.png)

<hr/>

![Image](https://github.com/PedroHenriqDev/Minimal-API-RESTful/blob/master/doc/images/api-in-swagger-stats(1).png)

### Installation Instructions
To set up the project locally, follow these steps:

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/PedroHenriqDev/Minimal-API-RESTful
   cd Minimal-API-RESTful

<hr/>

   
2.  **Create a local database**

   
   - preferences to postgreSQL
  
<hr/>
     
3.  **Apply Migrations**



   ```bash
   cd Minimal-API-RESTful
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add ExampleName --project Catalogue.Infrastructure -s Catalogue.API -c AppDbContext
   dotnet ef update database --project Catalogue.Infrastructure -s Catalogue.API -c AppDbContext
   
   